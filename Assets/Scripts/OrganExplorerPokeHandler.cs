using UnityEngine;

public class OrganExplorerPokeHandler : MonoBehaviour
{
    public OrganExplorerData organData;
    public GameObject uiPrefab;
    private GameObject currentUI;
    public Transform uiSpawnPoint;

    private int pokeCount = 0;
    private OrganExplorerUIController uiController;

    public void OnPoke()
    {
        pokeCount++;

        if (currentUI == null)
        {
            currentUI = Instantiate(uiPrefab, uiSpawnPoint.position, uiSpawnPoint.rotation);
            uiController = currentUI.GetComponent<OrganExplorerUIController>();
            uiController.Setup(organData);
            pokeCount = 1; // reset count karena UI baru muncul
        }
        else
        {
            if (pokeCount == 2)
            {
                CloseUI();
                pokeCount = 0;
            }
        }
    }

    void CloseUI()
    {
        if (currentUI != null)
            Destroy(currentUI);
        currentUI = null;
        uiController = null;
    }
}
