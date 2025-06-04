using System.Collections;
using UnityEngine;

public class ResumeUIManager : MonoBehaviour
{
    [Header("Canvas References")]
    public GameObject mainMenuCanvas;       // [Canvas]_Main_Menu
    public GameObject selectLevelCanvas;    // [Canvas]_Select_Level

    [Header("Camera Rig Control")]
    public Transform rigTransform;          // assign XR Rig / OVRCameraRig di Inspector

    [Header("Main Menu Positioning")]
    public Vector3 homePosition;
    public float homeYRotation;

    // Dipanggil dari [Button]_Home
    public void OnHomeButtonPressed()
    {
        mainMenuCanvas.SetActive(true);
    }

    // Dipanggil dari [Panel]_Resume
    public void OnResumePressed()
    {
        mainMenuCanvas.SetActive(false);
        selectLevelCanvas.SetActive(false);
    }

    // Dipanggil dari [Panel]_Select_Level
    public void OnSelectLevelPressed()
    {
        StartCoroutine(SwitchCanvas(mainMenuCanvas, selectLevelCanvas));
    }

    // Dipanggil dari [Panel]_Home
    public void OnReturnHomePressed()
    {
        rigTransform.position = homePosition;
        rigTransform.eulerAngles = new Vector3(0, homeYRotation, 0);
        mainMenuCanvas.SetActive(false);
        selectLevelCanvas.SetActive(false);
    }

    private IEnumerator SwitchCanvas(GameObject fromCanvas, GameObject toCanvas)
    {
        yield return new WaitForSeconds(0.1f);
        fromCanvas.SetActive(false);
        toCanvas.SetActive(true);
    }
}
