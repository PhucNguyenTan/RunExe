using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text tmpCount;

    [SerializeField]
    private Button closeButton;
    [SerializeField] 
    private TMP_Text tmpPath;
    [SerializeField]
    private Item itemPrefab;
    [SerializeField]
    private ScrollRect scrollView;

    private string startPath = "D:\\Unity Project";

    private void Start()
    {
        Screen.SetResolution(500, 500, false);
        closeButton.onClick.AddListener(Quit);
        var appPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath, ".."));
        var startPath = System.IO.Path.Combine(appPath, "..");

        var exes = FindExeFiles(startPath);
        
        tmpCount.text = $"Found {exes.Count} exe";
        tmpPath.text = startPath;

        foreach (var exe in exes)
        {
            var (name, path) = exe;
            var item = Instantiate(itemPrefab, scrollView.content);
            item.Setup(name, () => RunExe(path));
        }
    }

    public List<(string, string)> FindExeFiles(string directoryPath)
    {
        var exeFiles = new List<(string name, string path)>();

        if (Directory.Exists(directoryPath))
        {
            try
            {
                string[] files = Directory.GetFiles(directoryPath, "*.exe", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string filePath = Path.GetFullPath(file);
                    exeFiles.Add((fileName, filePath));
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"Error finding exe files: {ex.Message}");
            }
        }
        else
        {
            UnityEngine.Debug.LogError($"Directory not found: {directoryPath}");
        }

        return exeFiles;
    }

    public void RunExe(string exePath)
    {
        Process process = Process.Start(exePath);
        if (process != null)
        {
            UnityEngine.Debug.Log("Executable started successfully.");
        }
    }

    private void Quit()
    {
        Application.Quit();
    }
}
