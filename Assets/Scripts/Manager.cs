using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private Item itemPrefab;
    [SerializeField]
    private TMP_Text tmpTextError;
    [SerializeField]
    private Transform sectionError;
    [SerializeField]
    private Transform sectionButton;
    [SerializeField]
    private TMP_Text tmpTextTitle;


    private void Start()
    {
        SetResolution();
        CreateButtons();
    }
    private void SetResolution()
    {
        sectionError.gameObject.SetActive(false);
        sectionButton.gameObject.SetActive(true);
        Screen.SetResolution(1280, 720, false);
    }

    private void CreateButtons()
    {
        var appData = JsonDataHelper.ReadData<AppData>("app_data");
        if (appData == null )
        {
            ShowError("Dữ liệu data của app bị lỗi hoặc không tìm thấy!");
            return;
        }
        var items = JsonDataHelper.ReadDatas<ItemData>("item_data");
        if (items == null)
        {
            ShowError("Dữ liệu data của exe bị lỗi hoặc không tìm thấy!");
            return;
        }

        tmpTextTitle.text = appData.title;

        var appPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath, ".."));
        var findPath = System.IO.Path.Combine(appPath, "..");

        var startPath = string.IsNullOrEmpty(appData.testPath) ?
                     findPath : 
                     appData.testPath;
        var exes = FindExeFiles(startPath);

        foreach (var item in items)
        {
            if (exes.TryGetValue(item.exeName, out var path))
            {
                var button = Instantiate(itemPrefab, sectionButton);
                var sprite = Resources.Load<Sprite>($"Images/{item.imageName}");
                button.Setup(item.text, () => RunExe(path), sprite);
                if(sprite == null)
                    ShowError($"Không tìm thấy file hình: {item.imageName}");
            }
            else
            {
                var button = Instantiate(itemPrefab, sectionButton);
                button.Setup(item.text, null, null);
                ShowError($"Không tìm thấy file: {item.exeName}");
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
                var files = Directory.GetFiles(directoryPath, "*.exe", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    var filePath = Path.GetFullPath(file);
                    if(!exeFiles.ContainsKey(fileName))
                        exeFiles.Add(fileName, filePath);
                }
            }
            catch (System.Exception ex)
            {
                ShowError($"Lỗi Exception khi tìm file: {ex.Message}");
            }
        }
        else
        {
            ShowError($"Không tìm thấy địa chỉ folder: {directoryPath}");
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

    private void ShowError(string error)
    {
        tmpTextError.text += $"\\r\\n {error}";
        var isShowText = !string.IsNullOrEmpty(tmpTextError.text);
        sectionError.gameObject.SetActive(isShowText);
    }
}
