using UnityEngine;

public class HomePanelPoke : MonoBehaviour
{
    public HomeUIManager homeUIManager;

    public void OnPoke()
    {
        if (homeUIManager != null)
        {
            homeUIManager.TeleportToHome();
        }
        else
        {
            Debug.LogWarning("HomeUIManager belum di-assign ke HomePanelPoke");
        }
    }
}
