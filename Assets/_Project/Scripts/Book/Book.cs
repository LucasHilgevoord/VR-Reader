using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public BookData bookData;
    public BookObject bookObject;

    private int _currentPage = 0;
    
    internal void Initialize(BookData data)
    {
        Debug.Log("Initializing Book");
        bookData = data;
        LoadBookContent();
        SetupBook();
    }

    private void LoadBookContent()
    {
        switch (bookData.contentType)
        {
            case ContentType.Image:
                //bookData.imageContent.ConvertToSprite();
                break;
            case ContentType.Text:
                break;
            case ContentType.PDF:
                break;
            default:
                break;
        }

        // TODO: Wait until this has been finished
    }

    private void SetupBook()
    {
        // Start with the book closed
        bookObject.SetOpenPercentage(value: 0);
        bookObject.SetTotalPages(bookData.totalPages);
        UpdateBook();
    }

    internal void OpenBook()
    {
        
    }

    private void UpdateBook()
    {
        bookObject.SetCurrentPage(_currentPage);

        // Set the right page to the first page if we have no progress yet, otherwise make it the next one.
        if (_currentPage != bookData.totalPages - 1)
        {
            int page = _currentPage == 0 ? _currentPage : _currentPage + 1;
            bookObject.SetPageTexture(PageType.RightPage, bookData.imageContent.contentTextures[page]);
        }

        // No need to set the texture if we have no progress yet.
        if (_currentPage > 0)
            bookObject.SetPageTexture(PageType.LeftPage, bookData.imageContent.contentTextures[_currentPage]);

        bookObject.UpdateBook();
    }

    internal void NextPage()
    {
        if (_currentPage == bookData.totalPages - 1) { return; }
        Debug.Log("Next page");
        _currentPage++;
        UpdateBook();
    }

    internal void PrevPage()
    {
        if (_currentPage == 0) { return; }
        Debug.Log("Prev page");
        _currentPage--;
        UpdateBook();
    }
}
