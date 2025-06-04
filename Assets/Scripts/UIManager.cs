using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Canvas References")]
    public GameObject mainMenuCanvas;
    public GameObject playCanvas;
    public GameObject exitCanvas;

    [Header("Play Mode Panels & Images")]
    public GameObject[] modePanels;         // [Panel]_Mode_1, [Panel]_Mode_2
    public Image[] modeImages;              // Image inside the panels

    [Header("Play Mode Navigation Buttons")]
    public GameObject nextButton;
    public GameObject prevButton;
    public GameObject closeButton;

    [Header("Camera Rig Control")]
    public Transform rigTransform; // assign XR Rig / OVRCameraRig di Inspector

    [Header("Sprites for Modes")]
    public List<Sprite> modeSprites;        // Assign 4 sprites (for each mode)

    [System.Serializable]
    public class ModeData
    {
        public Vector3 position;
        public float yRotation;
        public bool isInteractable;
    }

    public List<ModeData> modeDataList = new List<ModeData>(); // 4 entries

    private int currentPage = 0;
    private const int modesPerPage = 2;

    // Called by ButtonManager
    public void OnPlayPressed()
    {
        StartCoroutine(SwitchCanvas(mainMenuCanvas, playCanvas));
        currentPage = 0;
        UpdatePlayModeSlide();
    }

    public void OnExitPressed()
    {
        StartCoroutine(SwitchCanvas(mainMenuCanvas, exitCanvas));
    }

    public void OnCloseExitPressed()
    {
        StartCoroutine(SwitchCanvas(exitCanvas, mainMenuCanvas));
    }

    public void OnClosePlayPressed()
    {
        StartCoroutine(SwitchCanvas(playCanvas, mainMenuCanvas));
    }

    public void OnExitYesPressed()
    {
        Application.Quit();
    }

    public void OnNextSlide()
    {
        currentPage++;
        UpdatePlayModeSlide();
    }

    public void OnPreviousSlide()
    {
        currentPage--;
        UpdatePlayModeSlide();
    }

    private void UpdatePlayModeSlide()
    {
        int start = currentPage * modesPerPage;

        for (int i = 0; i < modesPerPage; i++)
        {
            int modeIndex = start + i;

            if (modeIndex < modeSprites.Count && modeIndex < modeDataList.Count)
            {
                modePanels[i].SetActive(true);
                modeImages[i].sprite = modeSprites[modeIndex];

                // Assign panelSlotIndex dan uiManager ke ModePanelPoke
                ModePanelPoke poke = modePanels[i].GetComponent<ModePanelPoke>();
                if (poke != null)
                {
                    poke.panelSlotIndex = i;  // slot panel dalam slide (0 atau 1)
                    poke.uiManager = this;
                }

                // Button logic tetap seperti biasa
                Button btn = modePanels[i].GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.RemoveAllListeners();

                    if (modeDataList[modeIndex].isInteractable)
                    {
                        btn.interactable = true;
                        int capturedIndex = modeIndex;
                        btn.onClick.AddListener(() => SelectMode(capturedIndex));
                    }
                    else
                    {
                        btn.interactable = false;
                    }
                }
            }
            else
            {
                modePanels[i].SetActive(false);
            }
        }

        prevButton.SetActive(currentPage > 0);
        nextButton.SetActive((currentPage + 1) * modesPerPage < modeSprites.Count && (currentPage + 1) * modesPerPage < modeDataList.Count);
    }

    // Fungsi lama yang masih dipakai button biasa
    public void SelectMode(int index)
    {
        if (index >= 0 && index < modeDataList.Count && modeDataList[index].isInteractable)
        {
            rigTransform.position = modeDataList[index].position;
            rigTransform.eulerAngles = new Vector3(0, modeDataList[index].yRotation, 0);
        }
    }

    // Fungsi baru untuk manual select mode dari panel slot dan page saat ini
    public void ManualSelectMode(int panelSlotIndex)
    {
        int modeIndex = currentPage * modesPerPage + panelSlotIndex;

        Debug.Log($"ManualSelectMode dipanggil: currentPage={currentPage}, panelSlotIndex={panelSlotIndex}, modeIndex={modeIndex}");

        if (modeIndex >= 0 && modeIndex < modeDataList.Count && modeDataList[modeIndex].isInteractable)
        {
            rigTransform.position = modeDataList[modeIndex].position;
            rigTransform.eulerAngles = new Vector3(0, modeDataList[modeIndex].yRotation, 0);
        }
        else
        {
            Debug.LogWarning($"ManualSelectMode index {modeIndex} invalid atau tidak interactable");
        }
    }

    private System.Collections.IEnumerator SwitchCanvas(GameObject currentCanvas, GameObject nextCanvas)
    {
        yield return new WaitForSeconds(0.1f);
        currentCanvas.SetActive(false);
        nextCanvas.SetActive(true);
    }
}
