using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleUIOnSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject uiObject; // The UI GameObject to toggle
    private bool isActive = false; // Track the active state
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // This method will be called when the object is selected
    public void OnSelect()
    {
        // Toggle the active state
        isActive = !isActive;
        uiObject.SetActive(isActive);
    }
}
