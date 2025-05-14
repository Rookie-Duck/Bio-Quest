using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public struct UIPanel
    {
        public string panelName;
        public GameObject panelObject;
    }

    [System.Serializable]
    public struct UIImageSwap
    {
        public string key;
        public Image targetImage;
        public Sprite sprite;
    }

    [System.Serializable]
    public struct UITextSwap
    {
        public string key;
        public TextMeshProUGUI targetText;
        public string newText;
    }

    [Header("UI Panels")]
    public UIPanel[] panels;
    private Dictionary<string, GameObject> panelDict = new Dictionary<string, GameObject>();

    [Header("Optional Image Swaps")]
    public UIImageSwap[] imageSwaps;
    private Dictionary<string, UIImageSwap> imageDict = new Dictionary<string, UIImageSwap>();

    [Header("Optional Text Swaps")]
    public UITextSwap[] textSwaps;
    private Dictionary<string, UITextSwap> textDict = new Dictionary<string, UITextSwap>();

    void Awake()
    {
        foreach (var p in panels)
        {
            if (!panelDict.ContainsKey(p.panelName))
                panelDict.Add(p.panelName, p.panelObject);
        }

        foreach (var i in imageSwaps)
        {
            if (!imageDict.ContainsKey(i.key))
                imageDict.Add(i.key, i);
        }

        foreach (var t in textSwaps)
        {
            if (!textDict.ContainsKey(t.key))
                textDict.Add(t.key, t);
        }
    }

    public void ShowPanel(string panelName)
    {
        foreach (var pair in panelDict)
        {
            bool isActive = pair.Key == panelName;
            pair.Value.SetActive(isActive);
        }
        Debug.Log($"[UI] Switched to panel: {panelName}");
    }

    public void SwapImage(string key)
    {
        if (imageDict.TryGetValue(key, out var data))
        {
            data.targetImage.sprite = data.sprite;
        }
    }

    public void SwapText(string key)
    {
        if (textDict.TryGetValue(key, out var data))
        {
            data.targetText.text = data.newText;
        }
    }

    public void QuitApp()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    // Optional: Delayed transitions (for fade, etc) can be added later
}
