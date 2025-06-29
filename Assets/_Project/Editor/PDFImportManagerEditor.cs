
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PDFImportManager))]
public class PDFImportManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PDFImportManager manager = (PDFImportManager)target;

        if (GUILayout.Button("Refresh PDFs"))
        {
            manager.ProcessNewPDFs();
        }
    }
}