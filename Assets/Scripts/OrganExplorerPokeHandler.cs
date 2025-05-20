using UnityEngine;

public class OrganExplorerPokeHandler : MonoBehaviour
{
    public OrganExplorerData organData;
    public GameObject uiPrefab;
    public Transform uiSpawnPoint;

    private GameObject spawnedUI;

    public void OnPoke()
    {
        // Kalau UI sudah aktif dan ini organ yang sama → tutup
        if (spawnedUI != null && OrganExplorerUIManager.Instance.IsUIActive())
        {
            OrganExplorerUIManager.Instance.CloseUI();
            spawnedUI = null;
        }
        else
        {
            // Spawn baru dan register ke manager
            GameObject newUI = Instantiate(uiPrefab, uiSpawnPoint.position, uiSpawnPoint.rotation);
            var uiController = newUI.GetComponent<OrganExplorerUIController>();
            uiController.Setup(organData);

            OrganExplorerUIManager.Instance.ShowUI(newUI);
            spawnedUI = newUI;
        }
    }
}
