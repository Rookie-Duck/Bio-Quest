using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class OrganExplorerUIController : MonoBehaviour
{
    public Image imageDisplay;
    public TextMeshProUGUI textDisplay;
    public GameObject videoPanel;
    public VideoPlayer videoPlayer;

    private int currentSlide = 0;
    private OrganExplorerData organ;

    // Setup UI dengan data organ
    public void Setup(OrganExplorerData data)
    {
        organ = data;
        currentSlide = 0;
        ShowSlide(currentSlide);
    }

    // Fungsi untuk pindah ke slide berikutnya
    public void NextSlide()
    {
        if (currentSlide < 2)
        {
            currentSlide++;
            ShowSlide(currentSlide);
        }
    }

    // Fungsi untuk pindah ke slide sebelumnya (optional)
    public void PrevSlide()
    {
        if (currentSlide > 0)
        {
            currentSlide--;
            ShowSlide(currentSlide);
        }
    }

    // Tampilkan slide sesuai index
    void ShowSlide(int index)
    {
        videoPanel.SetActive(false);
        videoPlayer.Stop();

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
                imageDisplay.sprite = null;
                textDisplay.text = organ.organName;
                videoPanel.SetActive(true);
                videoPlayer.clip = organ.videoClip;
                videoPlayer.Play();
                break;
        }
    }
}
