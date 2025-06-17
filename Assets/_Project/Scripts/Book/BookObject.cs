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
    private float _spineWidth;// = 0.2f;
    private float _spineSizeMargin = 0.001f;

    [Header("Chaff")]
    [SerializeField] private Transform _top;
    [SerializeField] private Transform _bottomPivit, _topPivit, _flipPivit;
    [SerializeField] private Transform _topCover, _bottomCover;
    [SerializeField] private Transform _spine, _spine_left_pivit, _spine_right_pivit;

    [Header("Pages")]
    [SerializeField] private Transform _bottomPages, _topPages, _flipPaper;
    [SerializeField] private MeshRenderer _bottomRend, _topRend, _flipRend;
    [SerializeField] private float _pageThickness = 0.01f;

    [Header("Progrssion")]
    [Range(0, 1)]
    [SerializeField] private float _openPercentage = 0f;
    [SerializeField] private bool _forceOpen;
    [SerializeField] private int _totalPages = 100;
    [SerializeField] private int _currentPage = 0;

    internal void SetPageTexture(PageType type, Texture2D tex)
    {
        MeshRenderer obj = null;
        switch (type)
        {
            case PageType.LeftPage:
                obj = _topRend;
                break;
            case PageType.RightPage:
                obj = _bottomRend;
                break;
            case PageType.LeftFlip:
                break;
            case PageType.RightFlip:
                break;
            default:
                break;
        }

        obj.material.SetTexture("_PaperTexture", tex);
        Debug.Log("Set Texture");
    }

    internal void SetOpenPercentage(float value) { _openPercentage = value; }
    internal void SetCurrentPage(int page) { _currentPage = page; }
    internal void SetTotalPages(int total) { _totalPages = total; }

    internal void UpdateBook()
    {
        float widthOffset = -_bookSize.x / 2f;
        float progression = Mathf.Clamp01((float)_currentPage / _totalPages);
        float readProgression = (2f * progression) - 1f;

        _spineWidth = (_pageThickness * _totalPages + _chaffThickness) / 2f;

        UpdateSpine(readProgression);
        UpdateCovers(widthOffset);
        UpdatePageStacks(widthOffset);
        UpdateFlipPage(-widthOffset);
    }

    private void UpdateSpine(float readProgression)
    {
        _spine.localScale = new Vector3(_bookSize.y + _spineSizeMargin, _chaffThickness + _spineSizeMargin, _spineWidth * 2f);
        _spine.localRotation = Quaternion.Euler(90f * readProgression, 0f, 0f);
        _spine.localPosition = new Vector3(0f, 0f, _spineWidth * readProgression);
    }

    private void UpdateCovers(float widthOffset)
    {
        _topCover.localScale = new Vector3(_bookSize.y, _chaffThickness, _bookSize.x);
        _bottomCover.localScale = new Vector3(_bookSize.y, _chaffThickness, _bookSize.x);

        _topCover.localPosition = new Vector3(0f, 0f, widthOffset);
        _topPivit.position = _spine_left_pivit.position;

        _bottomCover.localPosition = new Vector3(0f, 0f, widthOffset);
        _bottomPivit.position = _spine_right_pivit.position;
    }

    private void UpdatePageStacks(float widthOffset)
    {
        // Right Side (Bottom Pages)
        float rightHeight = _pageThickness * (_totalPages - _currentPage);
        _bottomPages.localScale = new Vector3(_bookSize.y, rightHeight, _bookSize.x - _chaffThickness);
        _bottomPages.localPosition = new Vector3(0f, rightHeight / 2f + _chaffThickness / 2f, widthOffset);
        _bottomRend.enabled = _currentPage != _totalPages;

        // Left Side (Top Pages)
        float leftHeight = _pageThickness * _currentPage;
        _topPages.localScale = new Vector3(_bookSize.y, leftHeight, _bookSize.x - _chaffThickness);
        _topPages.localPosition = new Vector3(0f, -leftHeight / 2f - _chaffThickness / 2f, widthOffset);
        _topRend.enabled = _currentPage != 0;
    }

    private void UpdateFlipPage(float positionZ)
    {
        _flipPaper.localPosition = new Vector3(0f, 0f, positionZ);
    }


    private void OnValidate()
    {
        //if (!Application.isEditor || Application.isPlaying) { return; }

        if (_bottomRend == null) { _bottomRend = _bottomPages.GetComponent<MeshRenderer>(); }
        if (_topRend == null) { _topRend = _topPages.GetComponent<MeshRenderer>(); }

        // Cap the number of pages
        if (_totalPages < 1) { _totalPages = 1; }

        // Cap the currentPage
        _currentPage = Mathf.Clamp(_currentPage, 0, _totalPages);

        UpdateBook();
    }

}