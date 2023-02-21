using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    [Range(0, 1)]
    [SerializeField] private float _openPercentage = 0f;
    [SerializeField] private bool _forceOpen;

    [Header("Chaff")]
    [SerializeField] private Transform _top;
    [SerializeField] private Transform _bottomPivit, _topPivit, _flipPivit;
    [SerializeField] private Transform _topCover, _bottomCover;
    [SerializeField] private Transform _spine;

    [Header("Pages")]
    [SerializeField] private Transform _bottomPages, _topPages, _flipPaper;
    [SerializeField] private MeshRenderer _bottomRend, _topRend, _flipRend;
    [SerializeField] private float _pageThickness = 0.01f;
    [SerializeField] private int _totalPages = 10;
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
        float widthOffset = -_bookSize.x / 2;
        float progression = (_totalPages / (float)_currentPage);

        // Scale the book
        _spineWidth = _pageThickness * _totalPages + _chaffThickness;

        // Apply the assigned scale to the book
        _topCover.localScale = new Vector3(_bookSize.y, _chaffThickness, _bookSize.x);
        _bottomCover.localScale = new Vector3(_bookSize.y, _chaffThickness, _bookSize.x);
        _spine.localScale = new Vector3(_bookSize.y + _spineSizeMargin, _chaffThickness + _spineSizeMargin, _spineWidth);

        // Position the pivits
        _topPivit.localPosition = new Vector3(0, 0, -_spineWidth);
        _bottomPivit.localPosition = new Vector3(0, 0, 0);
        _flipPivit.localPosition = new Vector3(0, 0, _spineWidth + (_spineWidth / -progression));

        // Position the covers
        _topCover.localPosition = new Vector3(0, 0, widthOffset);
        _bottomCover.localPosition = new Vector3(0, 0, widthOffset);
        _spine.localPosition = new Vector3(0, 0, -_spineWidth / 2);

        // Set the position of the Right Side Pages
        _bottomPages.localScale = new Vector3(_bookSize.y, _pageThickness * (_totalPages - _currentPage), _bookSize.x - _chaffThickness);
        _bottomPages.localPosition = new Vector3(0, _bottomPages.localScale.y / 2 + _chaffThickness / 2, widthOffset);
        _bottomRend.enabled = _currentPage != _totalPages - 1;

        // Set the position of the Left Side Pages
        _topPages.localScale = new Vector3(_bookSize.y, _pageThickness * _currentPage, _bookSize.x - _chaffThickness);
        _topPages.localPosition = new Vector3(0, -_topPages.localScale.y / 2 - _chaffThickness / 2, widthOffset);
        _topRend.enabled = _currentPage != 0;

        // Set the position of the flip Page
        _flipPaper.localPosition = new Vector3(0, 0, -widthOffset);

        // Open the top chaff to the assigned openPercentage
        float pageProgression = (float)_currentPage / _totalPages;

        if (_currentPage <= _totalPages / 2)
        {
            float xPivitLerp = Mathf.Lerp(-90, 90, _openPercentage - (_forceOpen ? 1 : pageProgression));
            _topPivit.localRotation = Quaternion.Euler(xPivitLerp, 0, 0);
        }


        // Rotate the top pivit on the x axis between -270 and -180 based on what the currentpage progression is with the number of pages in the book
        float xLerp = Mathf.Lerp(-270, -180, (_forceOpen ? 1 : (pageProgression)) * (_openPercentage * 2));
        _top.localRotation = Quaternion.Euler(xLerp, 0, 0);

        _topPages.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_progress_d", pageProgression * _openPercentage);
    }

    private void OnValidate()
    {
        //if (!Application.isEditor || Application.isPlaying) { return; }

        if (_bottomRend == null) { _bottomRend = _bottomPages.GetComponent<MeshRenderer>(); }
        if (_topRend == null) { _topRend = _topPages.GetComponent<MeshRenderer>(); }

        // Cap the number of pages
        if (_totalPages < 1) { _totalPages = 1; }

        // Cap the currentPage
        _currentPage = Mathf.Clamp(_currentPage, 0, _totalPages - 1);

        UpdateBook();
    }

}
