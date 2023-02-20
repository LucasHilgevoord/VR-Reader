using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour
{
    public Book book;
    public BookData bookData; // TODO: This should be set somewhere else.

    public void Update()
    {
        
    }

    private void Awake()
    {
        InitializeBook();
    }

    private void InitializeBook()
    {
        book.Initialize(bookData);
    }

    private void NextPage()
    {

    }

    private void PreviousPage()
    {

    }


}
