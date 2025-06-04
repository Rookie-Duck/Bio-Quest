using UnityEngine;

public class ModePanelPoke : MonoBehaviour
{
    public UIManager uiManager;
    public SelectLevelUIManager selectLevelUI;
    public int panelSlotIndex;

    public void OnPoke()
    {
        if (uiManager != null)
        {
            uiManager.ManualSelectMode(panelSlotIndex);
        }
        else if (selectLevelUI != null)
        {
            
        }
        else
        {
            Debug.LogWarning("UIManager atau SelectLevelUIManager belum di-assign ke ModePanelPoke");
        }
    }
}
