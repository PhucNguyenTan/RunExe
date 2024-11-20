using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField]
    private TMP_Text tmpText;
    [SerializeField]
    private Image icon;

    private Button button;

    private Action action;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Action);
    }

    public void Setup(string text, Action action, Sprite sprite)
    {
        tmpText.text = text;
        this.action = action;
        if (sprite != null)
            icon.sprite = sprite;
    }

    private void Action()
    {
        Debug.Log("Click Button");
        action?.Invoke();
    }
}
