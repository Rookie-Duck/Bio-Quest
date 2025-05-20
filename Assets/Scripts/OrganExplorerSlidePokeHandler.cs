using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganExplorerSlidePokeHandler : MonoBehaviour
{
    public OrganExplorerUIController uiController; // assign dari inspector atau runtime
    public bool isNext = true; // true = next slide, false = prev slide

    public void OnPoke()
    {
        if (uiController == null) return;

        if (isNext)
            uiController.NextSlide();
        else
            uiController.PrevSlide();
    }
}
