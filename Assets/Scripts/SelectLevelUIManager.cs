using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevelUIManager : MonoBehaviour
{
    [Header("Canvas References")]
    public GameObject mainMenuCanvas;       // [Canvas]_Main_Menu
    public GameObject selectLevelCanvas;    // [Canvas]_Select_Level

    [Header("XR Rig")]
    public Transform rigTransform;          // assign XR Rig atau parent camera rig kamu

    [Header("Posisi Main Menu")]
    public Vector3 mainMenuPosition;
    public float mainMenuYRotation = 0f;

    public void OnResumePressed()
    {
        mainMenuCanvas.SetActive(false);
    }

    public void OnSelectLevelPressed()
    {
        StartCoroutine(SwitchCanvas(mainMenuCanvas, selectLevelCanvas));
    }

    public void OnBackToMainMenuPressed()
    {
        StartCoroutine(SwitchCanvas(selectLevelCanvas, mainMenuCanvas));
    }

    public void OnHomePressed()
    {
        rigTransform.position = mainMenuPosition;
        rigTransform.eulerAngles = new Vector3(0, mainMenuYRotation, 0);
    }

    private IEnumerator SwitchCanvas(GameObject from, GameObject to)
    {
        yield return new WaitForSeconds(0.1f);
        from.SetActive(false);
        to.SetActive(true);
    }
}
