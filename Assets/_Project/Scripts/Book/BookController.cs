using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour
{
    public Book book;
    public BookData bookData; // TODO: This should be set somewhere else.

    private void Awake()
    {
        InitializeBook();
    }

    private void InitializeBook()
    {
        book.Initialize(bookData);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            book.NextPage();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            book.PrevPage();
        }
    }
}
