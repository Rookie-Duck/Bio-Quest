using UnityEngine;

public class HomeButtonPoke : MonoBehaviour
{
    private ButtonManager buttonManager;

    private void Awake()
    {
        buttonManager = FindObjectOfType<ButtonManager>();
        if (buttonManager != null)
            Debug.Log("ButtonManager ditemukan oleh HomeButtonPoke");
        else
            Debug.LogWarning("ButtonManager TIDAK ditemukan oleh HomeButtonPoke");
    }

    public void OnPoke()
    {
        if (buttonManager != null)
        {
            Debug.Log("HomeButtonPoke memanggil OnHomeButtonPressed di ButtonManager");
            buttonManager.OnHomeButtonPressed();
        }
        else
        {
            Debug.LogWarning("ButtonManager tidak ditemukan saat OnPoke");
        }
    }
}
