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

    public Texture2D[] contentTextures;
    internal Sprite[] contentSprites;
    
    internal void UpdateContent()
    {
        Debug.Log("Update Content");
        // TODO: Check if something has changed in the content.

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
        contentTextures = images;
    }

    internal void ConvertToSprite()
    {
        if (contentTextures.Length < 0)
            throw new Exception("No textures have been found");

        contentSprites = new Sprite[contentTextures.Length];
        Vector2 center = new Vector2(0.5f, 0.5f);
        for (int i = 0; i < contentTextures.Length; i++)
        {
            Texture2D tex = contentTextures[i];
            contentSprites[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), center, 100);
        }
    }
}
