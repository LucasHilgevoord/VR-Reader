using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class ImageContent
{
    public const string ParentFolderPath = "ScriptableObjects/Book/ImageContent/";
    [FolderPath(ParentFolder = ParentFolderPath)]
    public string ImageFolderPath;

    public Texture2D[] contentSprites;
    
    internal void UpdateContent()
    {
        Debug.Log("Update Content");

        // Check if the file path exists
        string fullPath = $"{Application.dataPath}/Resources/{ParentFolderPath + ImageFolderPath}";
        Debug.Log($"Searching for images at path: {fullPath}");
        if (!System.IO.Directory.Exists(fullPath))
        {
            Debug.LogError($"Path: {fullPath} does not exist!");
            return;
        }

        object[] loadedImages = Resources.LoadAll(ParentFolderPath + ImageFolderPath, typeof(Texture2D));
        Texture2D[] images = new Texture2D[loadedImages.Length];
        for (int i = 0; i < loadedImages.Length; i++)
        {
            images[i] = (Texture2D)loadedImages[i];
        }
        contentSprites = images;
        //ConvertToSprite(images);
    }

    //private void ConvertToSprite(Texture2D[] images)
    //{
    //    contentSprites = new Sprite[images.Length];
    //    Vector2 center = new Vector2(0.5f, 0.5f);
    //    for (int i = 0; i < images.Length; i++)
    //    {
    //        Texture2D tex = images[i];
    //        contentSprites[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), center, 100);
    //    }
    //}
}
