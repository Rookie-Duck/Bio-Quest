using UnityEngine;

public class ModePanelPoke : MonoBehaviour
{
    public UIManager uiManager;
    public int panelSlotIndex;

    public void OnPoke()
    {

        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        if (uiManager != null)
        {
            uiManager.ManualSelectMode(panelSlotIndex);
        }
        else
        {
            Debug.LogWarning("UIManager atau HomeUIManager belum ditemukan di scene");
        }
    }
}
