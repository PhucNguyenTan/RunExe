using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField]
    private TMP_Text tmpText;
    [SerializeField]
    private Button button;

    private Action action;

    private void Awake()
    {
        button.onClick.AddListener(Action);
    }

    public void Setup(string text, Action action)
    {
        tmpText.text = text;
        this.action = action;
    }

    private void Action()
    {
        Debug.Log("Click Button");
        action?.Invoke();
    }
}
