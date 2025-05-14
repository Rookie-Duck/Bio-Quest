using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject canvasA;
    public GameObject canvasB;

    public void SwitchCanvas()
    {
        canvasA.SetActive(false);
        canvasB.SetActive(true);
        Debug.Log("Canvas Switched!");
    }
}
