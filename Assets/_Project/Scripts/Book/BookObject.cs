using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField] private Transform _bottomPivit, _topPivit;
    [SerializeField] private Transform _topChaff, _bottomChaff;
    [SerializeField] private Transform _spine;

    [Header("Pages")]
    [SerializeField] private Transform _bottomPages, _topPages;
    [SerializeField] private MeshRenderer _bottomRend, _topRend;
    [SerializeField] private float _pageThickness = 0.01f;
    [SerializeField] private int _numberOfPages = 10;
    [SerializeField] private int _currentPage = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenBook() 
    {
        
    }

    private void OnValidate()
    {
        if (_bottomRend == null) { _bottomRend = _bottomPages.GetComponent<MeshRenderer>(); }
        if (_topRend == null) { _topRend = _topPages.GetComponent<MeshRenderer>(); }

        // Cap the number of pages
        if (_numberOfPages < 1) { _numberOfPages = 1; }

        // Cap the currentPage
        _currentPage = Mathf.Clamp(_currentPage, 0, _numberOfPages);

        // Scale the book
        _spineWidth = _pageThickness * _numberOfPages + _chaffThickness;

        _topChaff.localScale = new Vector3(_bookSize.y, _chaffThickness, _bookSize.x);
        _bottomChaff.localScale = new Vector3(_bookSize.y, _chaffThickness, _bookSize.x);
        _spine.localScale = new Vector3(_bookSize.y + _spineSizeMargin, _chaffThickness + _spineSizeMargin, _spineWidth);

        _topPivit.localPosition = new Vector3(0, 0, -_spineWidth);
        _topChaff.localPosition = new Vector3(0, 0, -_bookSize.x / 2);
        _bottomPivit.localPosition = new Vector3(0, 0, -_bookSize.x / 2);
        _spine.localPosition = new Vector3(0, 0, -_spineWidth / 2);

        _bottomPages.localScale = new Vector3(_bookSize.y, _pageThickness * (_numberOfPages - _currentPage), _bookSize.x - _chaffThickness);
        _bottomPages.localPosition = new Vector3(0, _bottomPages.localScale.y / 2 + _chaffThickness / 2, 0);
        _bottomRend.enabled = _bottomPages.localScale.y == 0 ? false : true;

        _topPages.localScale = new Vector3(_bookSize.y, _pageThickness * _currentPage, _bookSize.x - _chaffThickness);
        _topPages.localPosition = new Vector3(0, -_topPages.localScale.y / 2 - _chaffThickness / 2, -_bookSize.x / 2);
        _topRend.enabled = _topPages.localScale.y == 0 ? false : true;


        // Open the top chaff to the assigned openPercentage
        float pageProgression = (float)_currentPage / _numberOfPages;
        float xPivitLerp = Mathf.Lerp(-90, 90, _openPercentage - (_forceOpen ? 1 : pageProgression));
        _topPivit.localRotation = Quaternion.Euler(xPivitLerp, 0, 0);

        // Rotate the top pivit on the x axis between -270 and -180 based on what the currentpage progression is with the number of pages in the book
        float xLerp = Mathf.Lerp(-270, -180, (_forceOpen ? 1 : (pageProgression)) * (_openPercentage * 2));
        _top.localRotation = Quaternion.Euler(xLerp, 0, 0);


        _topPages.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_progress_d", pageProgression);
    }

}