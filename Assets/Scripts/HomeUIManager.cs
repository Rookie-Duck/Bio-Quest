using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUIManager : MonoBehaviour
{
    [Header("Canvas References")]
    public GameObject mainMenuCanvas;      // [Canvas]_Main_Menu
    public GameObject selectLevelCanvas;   // [Canvas]_Select_Level

    [Header("Camera Rig Control")]
    public Transform rigTransform;

    [Header("Home Position")]
    public Vector3 homePosition;
    public float homeYRotation;


    [System.Serializable]
    public class ModeData
    {
        public Vector3 position;
        public float yRotation;
        public bool isInteractable;
    }

    public List<ModeData> modeDataList = new List<ModeData>(); // 4 entries

    public void OnHomeButtonPressed()
    {
        StartCoroutine(SwitchCanvas(null, mainMenuCanvas));
    }

    public void OnResumePressed()
    {
        mainMenuCanvas.SetActive(false);
    }

    public void OnSelectLevelPressed()
    {
        StartCoroutine(SwitchCanvas(mainMenuCanvas, selectLevelCanvas));
    }

    public void OnCloseSelectLevelPressed()
    {
        StartCoroutine(SwitchCanvas(selectLevelCanvas, mainMenuCanvas));
    }

    public void SelectMode(int index)
    {
        if (index >= 0 && index < modeDataList.Count && modeDataList[index].isInteractable)
        {
            rigTransform.position = modeDataList[index].position;
            rigTransform.eulerAngles = new Vector3(0, modeDataList[index].yRotation, 0);
        }
    }

    private IEnumerator SwitchCanvas(GameObject currentCanvas, GameObject nextCanvas)
    {
        yield return new WaitForSeconds(0.1f);

        if (currentCanvas != null)
            currentCanvas.SetActive(false);

        if (nextCanvas != null)
            nextCanvas.SetActive(true);
    }

    public void TeleportToHome()
    {
        // Kamu boleh bikin modeDataList[0] = Home position
        // Atau hardcode manual kalo mau bener2 aman:
        rigTransform.position = homePosition;
        rigTransform.eulerAngles = new Vector3(0, homeYRotation, 0);
    }


}
