using UnityEngine;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class PDFImportManager : MonoBehaviour
{
    string importPath => Path.Combine(Application.streamingAssetsPath, "Books/import");
    string exportPath => Path.Combine(Application.streamingAssetsPath, "Books/export");

    void Start()
    {
        ProcessNewPDFs();
    }

    public void ProcessNewPDFs()
    {
        if (!Directory.Exists(importPath)) Directory.CreateDirectory(importPath);
        if (!Directory.Exists(exportPath)) Directory.CreateDirectory(exportPath);
        Debug.Log($"Importing PDFs from: {importPath}");

        string[] pdfFiles = Directory.GetFiles(importPath, "*.pdf");

        foreach (string pdfFile in pdfFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(pdfFile);
            string outputFolder = Path.Combine(exportPath, fileName);

            if (!Directory.Exists(outputFolder))
            {
                Debug.Log($"New PDF found: {fileName}. Processing...");
                Directory.CreateDirectory(outputFolder);
                ConvertPDFToImages(pdfFile, outputFolder);
            }
            else
            {
                Debug.Log($"PDF '{fileName}' already processed. Skipping.");
            }
        }

        Debug.Log("Processing Complete!");
    }

    private void ConvertPDFToImages(string inputPDF, string outputDir)
    {
        string exePath = GetPlatformConverterPath();
        Debug.Log($"Looking for converter at: {exePath}");

        if (!File.Exists(exePath))
        {
            Debug.LogError("PDF converter executable not found for this platform.");
            return;
        }

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = $"\"{inputPDF}\" \"{outputDir}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                if (process == null)
                {
                    Debug.LogError("Failed to start PDF conversion process.");
                    return;
                }

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError($"PDF conversion error for '{Path.GetFileName(inputPDF)}':\n{error}");
                }
                else
                {
                    Debug.Log($"Successfully converted PDF: {Path.GetFileName(inputPDF)}");
                    Debug.Log(output);
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Exception during PDF conversion for '{Path.GetFileName(inputPDF)}':\n{ex.Message}\n{ex.StackTrace}");
        }
    }

    private string GetPlatformConverterPath()
    {
        string basePath = Path.Combine(Application.streamingAssetsPath, "Tools");

        string exeRelativePath = null;

        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                exeRelativePath = Path.Combine(basePath, "Windows/dist", "pdf_to_img.exe");
                break;

            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                exeRelativePath = Path.Combine(basePath, "macOS", "pdf_to_img.sh");
                break;

            case RuntimePlatform.LinuxPlayer:
            case RuntimePlatform.LinuxEditor:
                exeRelativePath = Path.Combine(basePath, "Linux", "pdf_to_img");
                break;

            default:
                Debug.LogError("Unsupported platform: " + Application.platform);
                return null;
        }

        string fullPath = Path.GetFullPath(exeRelativePath);
        Debug.Log("Resolved executable path: " + fullPath);
        return fullPath;
    }
}
