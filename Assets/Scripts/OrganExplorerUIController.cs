using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class OrganExplorerUIController : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI textDisplay;
    public Image imageDisplay;
    public GameObject videoPanel;
    public VideoPlayer videoPlayer;

    public GameObject nextButtonPanel;
    public GameObject prevButtonPanel;

    private int currentSlide = 0;
    private OrganExplorerData organ;

    public void Setup(OrganExplorerData data)
    {
        organ = data;
        currentSlide = 0;
        ShowSlide(currentSlide);
    }

    public void NextSlide()
    {
        if (currentSlide < 2)
        {
            currentSlide++;
            ShowSlide(currentSlide);
        }
    }

    public void PrevSlide()
    {
        if (currentSlide > 0)
        {
            currentSlide--;
            ShowSlide(currentSlide);
        }
    }

    void ShowSlide(int index)
    {
        // Reset semua visual
        videoPanel.SetActive(false);
        videoPlayer.Stop();
        imageDisplay.enabled = true;
        textDisplay.enabled = true;

        titleText.text = organ.organName;

        switch (index)
        {
            case 0:
                imageDisplay.sprite = organ.imageSlide1;
                textDisplay.text = organ.definition;
                break;
            case 1:
                imageDisplay.sprite = organ.imageSlide2;
                textDisplay.text = organ.function;
                break;
            case 2:
                imageDisplay.enabled = false;
                textDisplay.enabled = false;
                videoPanel.SetActive(true);
                videoPlayer.clip = organ.videoClip;
                videoPlayer.Play();
                break;
        }

        // Atur visibilitas tombol navigasi
        prevButtonPanel.SetActive(index > 0);         // Hanya tampil di slide 2–3
        nextButtonPanel.SetActive(index < 2);         // Hanya tampil di slide 1–2
    }

    public void CloseSelf()
    {
        if (OrganExplorerUIManager.Instance != null)
        {
            OrganExplorerUIManager.Instance.CloseUI();
        }
    }

}
