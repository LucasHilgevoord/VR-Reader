using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BookData", menuName = "ScriptableObjects/BookObject", order = 1)]
public class BookData : ScriptableObject
{
    public string title;
    public string author;
    public string description;
    public ContentType contentType;
    public int totalPages;
    
    [ShowIf("contentType", ContentType.Image)]
    public ImageContent imageContent = new ImageContent();

    [Button(ButtonSizes.Medium), GUIColor(0.4f, 0.8f, 1)]
    private void RefreshContent() { UpdateContent(); }


    public void OnValidate()
    {
        if (Application.isPlaying || !Application.isEditor) return;
        RefreshContent();
    }

    private void UpdateContent()
    {
        switch (contentType)
        {
            case ContentType.Image:
                imageContent.UpdateContent();
                totalPages = imageContent.contentTextures.Length;
                break;
            case ContentType.Text:
                break;
            case ContentType.PDF:
                break;
            default:
                break;
        }
    }
}
