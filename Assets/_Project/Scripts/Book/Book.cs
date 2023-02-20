using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public BookData bookData;
    public BookObject bookObject;

    private int _currentPage;
    
    internal void Initialize(BookData data)
    {
        Debug.Log("Initializing Book");
        bookData = data;
        LoadBookContent();
        SetupBook();
        SetBookContent();
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

    }

    private void SetBookContent()
    {
        bookObject.SetPageTexture(PageType.LeftPage, bookData.imageContent.contentTextures[0]);
        bookObject.SetPageTexture(PageType.RightPage, bookData.imageContent.contentTextures[1]);
    }
}
