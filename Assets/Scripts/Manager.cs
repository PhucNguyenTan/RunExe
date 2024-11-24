using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private Item itemPrefab;
    [SerializeField]
    private ScrollRect errorScroll;
    [SerializeField]
    private Transform sectionButton;
    [SerializeField]
    private TMP_Text tmpTextTitle;
    [SerializeField]
    private TMP_Text tmpTextError;

    private void Start()
    {
        SetResolution();
        CreateButtons();
    }
    private void SetResolution()
    {
        errorScroll.gameObject.SetActive(false);
        sectionButton.gameObject.SetActive(true);
        Screen.SetResolution(1280, 720, false);
    }

    private void CreateButtons()
    {
        var appData = DataUtils.GetData<AppData>("app_data");
        var items   = DataUtils.GetDatas<ItemData>("item_data");
        if (appData == null )
        {
            ShowError("Dữ liệu data của app bị lỗi hoặc không tìm thấy!");
            return;
        }
        if (items == null)
        {
            ShowError("Dữ liệu data của exe bị lỗi hoặc không tìm thấy!");
            return;
        }

        tmpTextTitle.text = appData.title;

        var appPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath, ".."));
        var defaultPath = System.IO.Path.Combine(appPath, "..");

        var startPath = Directory.Exists(appData.startPath) ?
                     appData.startPath :
                     defaultPath;
        ShowError($"Đường dẫn: {startPath}");
        var exes = FindExeFiles(startPath);

        foreach (var item in items)
        {
            if (exes.TryGetValue(item.exeName, out var path))
            {
                var button = Instantiate(itemPrefab, sectionButton);
                button.Setup(item.text, () => RunExe(path[0]), GetSprite(item.imageName));

                if(path.Count > 1)
                    ShowError($"Có {path.Count} exe trùng tên {item.exeName}");
            }
            else
            {
                var button = Instantiate(itemPrefab, sectionButton);
                button.Setup(item.text, null, null);
                ShowError($"Không tìm thấy file: {item.exeName}");
            }
        }
    }

    private Sprite GetSprite(string spriteName)
    {
        var sprite = Resources.Load<Sprite>($"Images/{spriteName}");

        if (sprite == null)
        {
            string path = Path.Combine(Application.streamingAssetsPath, spriteName + ".png");
            if (File.Exists(path))
            {
                byte[] fileData = File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(fileData))
                {
                    sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
        }
        if (sprite == null)
        {
            ShowError($"Không tìm thấy file hình: {spriteName}");
        }
        return sprite;
    }

    public Dictionary<string, List<string>> FindExeFiles(string directoryPath)
    {
        var exes = new List<(string name, string path)>();
        if (Directory.Exists(directoryPath))
        {
            try
            {
                var files = Directory.GetFiles(directoryPath, "*.exe", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    var filePath = Path.GetFullPath(file);
                    exes.Add((fileName, filePath));
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

        return exes
            .GroupBy(exe => exe.name)
            .ToDictionary(
                group => group.Key, 
                group => group.Select(item => item.path).ToList());
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

    private void ShowError(string error, bool isActive = true)
    {
        var newError = Instantiate(tmpTextError, errorScroll.content);
        newError.text = error;
        errorScroll.gameObject.SetActive(isActive);
    }
}
