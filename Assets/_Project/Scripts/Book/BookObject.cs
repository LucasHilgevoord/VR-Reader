using UnityEngine;

public enum PageType
{
    LeftPage,
    RightPage,
    LeftFlip,
    RightFlip
}

public class BookObject : MonoBehaviour
{
    [Header("Book")]
    [SerializeField] private Vector2 _bookSize = new Vector2(2, 1.5f);
    [SerializeField] private float _chaffThickness = 0.025f;
    [SerializeField] private bool _disablePageWeight = true;
    [SerializeField] private bool _isOnFlatSurface = true;
    private float _spineWidth;
    private float _spineSizeMargin = 0.001f;

    [Header("Chaff")]
    [SerializeField] private Transform _top, _bottom;
    [SerializeField] private Transform _bottomPivit, _topPivit, _flipPivit;
    [SerializeField] private Transform _topCover, _bottomCover;
    [SerializeField] private Transform _spine, _spine_left_pivit, _spine_right_pivit;

    [Header("Pages")]
    [SerializeField] private float _pageThickness = 0.01f;
    [SerializeField] private Transform _bottomPageStack, _topPageStack;
    [SerializeField] private Transform _bottomFlipPagePivitTracker, _topFlipPagePivitTracker;
    [SerializeField] private Transform _bottomFlipPageParent, _topFlipPageParent;
    [SerializeField] private MeshRenderer _bottomPageStackRend, _topPageStackRend;

    [Header("Progression")]
    [Range(0, 1)]
    [SerializeField] private float _openPercentage = 0f;
    [SerializeField] private int _totalPages = 100;
    [SerializeField] private int _currentPage = 0;
    bool _inFirstHalf { get { return _currentPage <= _totalPages / 2; }}

    internal void SetPageTexture(PageType type, Texture2D tex)
    {
        MeshRenderer obj = null;
        switch (type)
        {
            case PageType.LeftPage:
                obj = _topPageStackRend;
                break;
            case PageType.RightPage:
                obj = _bottomPageStackRend;
                break;
            case PageType.LeftFlip:
            case PageType.RightFlip:
            default:
                break;
        }
    }

    internal void SetOpenPercentage(float value) { _openPercentage = Mathf.Clamp01(value); }
    internal void SetCurrentPage(int page) { _currentPage = Mathf.Clamp(page, 0, _totalPages); }
    internal void SetTotalPages(int total) { _totalPages = Mathf.Max(1, total); }

    private float GetReadProgression()
    {
        float progression = Mathf.Clamp01((float)_currentPage / _totalPages);
        return (2f * progression) - 1f;
    }

    internal void UpdateBook()
    {
        float widthOffset = -_bookSize.x / 2f;
        _spineWidth = (_pageThickness * _totalPages + _chaffThickness) / 2f;

        UpdateBookSize();
        UpdateSpine();
        UpdateCovers(widthOffset);
        UpdatePageStacks(widthOffset);
        UpdateFlipPage(-widthOffset);
        UpdateOpeningMotion();
    }

    private void UpdateBookSize()
    {
        _spine.localScale = new Vector3(_bookSize.y + _spineSizeMargin, _chaffThickness + _spineSizeMargin, _spineWidth * 2f);
        _topCover.localScale = _bottomCover.localScale = new Vector3(_bookSize.y, _chaffThickness, _bookSize.x);

        float planeFactor = 10;
        _bottomFlipPageParent.localScale = _topFlipPageParent.localScale = new Vector3(_bookSize.y / planeFactor, _chaffThickness / planeFactor, (_bookSize.x - _chaffThickness) / planeFactor);

        _bottomPageStack.localScale = new Vector3(_bookSize.y, GetRightPageStackHeight(), _bookSize.x - _chaffThickness);
        _topPageStack.localScale = new Vector3(_bookSize.y, GetLeftPageStackHeight(), _bookSize.x - _chaffThickness);
    }

    private float GetLeftPageStackHeight() => _pageThickness * _currentPage;
    private float GetRightPageStackHeight() => _pageThickness * (_totalPages - _currentPage);

    private void UpdateSpine()
    {
        if (_isOnFlatSurface) {
            float tiltDirection = _inFirstHalf ? -1f : 1f;
            float maxTilt = 90f;
            float targetXRotation = Mathf.Lerp(maxTilt * GetReadProgression(), maxTilt * tiltDirection, 1f - _openPercentage);

            _spine.localRotation = Quaternion.Euler(targetXRotation, 0f, 0f);
            _spine.localPosition = new Vector3(0f, 0f, _spineWidth);
        } else
        {
            _spine.localRotation = Quaternion.identity;
            _spine.localPosition = new Vector3(0f, 0f, 0f);
        }
    }

    private void UpdateCovers(float widthOffset)
    {
        _topCover.localPosition = new Vector3(0f, 0f, widthOffset);
        _topPivit.position = _spine_left_pivit.position;

        _bottomCover.localPosition = new Vector3(0f, 0f, widthOffset);
        _bottomPivit.position = _spine_right_pivit.position;
    }

    private void UpdatePageStacks(float widthOffset)
    {
        _bottomPageStack.localPosition = new Vector3(0f, GetRightPageStackHeight() / 2f + _chaffThickness / 2f, widthOffset);
        _bottomPageStackRend.enabled = _currentPage != _totalPages;

        _topPageStack.localPosition = new Vector3(0f, -GetLeftPageStackHeight() / 2f - _chaffThickness / 2f, widthOffset);
        _topPageStackRend.enabled = _currentPage != 0;
    }

    private void UpdateFlipPage(float positionZ)
    {
        float yOffset = 0.001f;

        Vector3 bottom_tracker = _bottomFlipPagePivitTracker.position;
        bottom_tracker.y += yOffset;
        _bottomFlipPageParent.position = bottom_tracker;

        Vector3 top_tracker = _topFlipPagePivitTracker.position;
        top_tracker.y += yOffset;
        _topFlipPageParent.position = top_tracker;

        _topFlipPageParent.gameObject.SetActive(_currentPage != 0);
        _bottomFlipPageParent.gameObject.SetActive(_currentPage != _totalPages);
    }

    private void UpdateOpeningMotion()
    {
        if (_isOnFlatSurface)
        {
            float halfPoint = _totalPages / 2f;
            float pivotLerp = (_inFirstHalf ? _openPercentage : 1f - _openPercentage);
            float pivotAngle = Mathf.Lerp(0f, 180f, pivotLerp);
            if (_inFirstHalf)
            {
                _topPivit.localRotation = Quaternion.Euler(pivotAngle, 0f, 0f);
                _bottomPivit.localRotation = Quaternion.identity;
            }
            else
            {
                _topPivit.localRotation = Quaternion.Euler(180f, 0f, 0f);
                _bottomPivit.localRotation = Quaternion.Euler(pivotAngle, 0f, 0f);
            }
        } else
        {
            float topPivotAngle = Mathf.Lerp(90f, 180f, _openPercentage);
            _topPivit.localRotation = Quaternion.Euler(topPivotAngle, 0f, 0f);

            float botPivotAngle = Mathf.Lerp(0f, 90f, 1f - _openPercentage);
            _bottomPivit.localRotation = Quaternion.Euler(botPivotAngle, 0f, 0f);
        }
    }

    private void OnValidate()
    {
        if (_bottomPageStackRend == null && _bottomPageStack != null) { _bottomPageStackRend = _bottomPageStack.GetComponent<MeshRenderer>(); }
        if (_topPageStackRend == null && _topPageStack != null) { _topPageStackRend = _topPageStack.GetComponent<MeshRenderer>(); }

        _totalPages = Mathf.Max(1, _totalPages);
        _openPercentage = Mathf.Clamp01(_openPercentage);
        _currentPage = Mathf.Clamp(_currentPage, 0, _totalPages);

        UpdateBook();
    }
}
