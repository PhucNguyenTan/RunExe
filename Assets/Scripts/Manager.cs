using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private Item itemPrefab;

    private string testPath = "D:\\_build";

    private List<ItemData> items = new List<ItemData>
    {
        new ItemData(1, "appicn_phantuhuuco", "organic-molecules.exe" , "Hữu cơ"),
        new ItemData(2, "appicn_rutherford_bohr", "excretory-system.exe" ,"Rutherford"),
        new ItemData(3, "appicn_donchathopchat", "simple_compound.exe" ,"Đơn chất"),
        new ItemData(4, "appicn_binhdienphan", "ProjectOptics.exe" ,"Điện phan6"),
        new ItemData(5, "appicn_tinhbot", "ProjectOptics_2.exe" ,"Tinh bột"),
    };

    private void Start()
    {
        Screen.SetResolution(1280, 720, false);
        var appPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath, ".."));
        var startPath = System.IO.Path.Combine(appPath, "..");

        var exes = FindExeFiles(testPath);

        foreach (var item in items)
        {
            if (exes.TryGetValue(item.exeName, out var path))
            {
                var button = Instantiate(itemPrefab, transform);
                var sprite = Resources.Load<Sprite>(item.imageName);
                button.Setup(item.text, () => RunExe(path), sprite);
            }
            else
            {
                var button = Instantiate(itemPrefab, transform);
                button.Setup($"Không tìm thấy file {item.exeName}", null, null);
            }
        }
    }

    public Dictionary<string, string> FindExeFiles(string directoryPath)
    {
        var exeFiles = new Dictionary<string, string>();

        if (Directory.Exists(directoryPath))
        {
            try
            {
                string[] files = Directory.GetFiles(directoryPath, "*.exe", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string filePath = Path.GetFullPath(file);
                    if(!exeFiles.ContainsKey(fileName))
                        exeFiles.Add(fileName, filePath);
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
