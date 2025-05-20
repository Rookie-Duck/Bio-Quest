using UnityEngine;

public class OrganExplorerUIManager : MonoBehaviour
{
    public static OrganExplorerUIManager Instance;

    private GameObject currentUI;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void ShowUI(GameObject ui)
    {
        if (currentUI != null)
            Destroy(currentUI);

        currentUI = ui;
    }

    public void CloseUI()
    {
        if (currentUI != null)
        {
            Destroy(currentUI);
            currentUI = null;
        }
    }

    public bool IsUIActive()
    {
        return currentUI != null;
    }
}
