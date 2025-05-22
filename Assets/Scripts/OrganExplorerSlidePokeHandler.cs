using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganExplorerSlidePokeHandler : MonoBehaviour
{
    public OrganExplorerUIController uiController;
    public enum PokeAction { Next, Prev, Close }
    public PokeAction action;

    public void OnPoke()
    {
        if (uiController == null) return;

        switch (action)
        {
            case PokeAction.Next:
                uiController.NextSlide();
                break;
            case PokeAction.Prev:
                uiController.PrevSlide();
                break;
            case PokeAction.Close:
                uiController.CloseSelf();
                break;
        }
    }
    /*public OrganExplorerUIController uiController; // assign dari inspector atau runtime
    public bool isNext = true; // true = next slide, false = prev slide

    public void OnPoke()
    {
        if (uiController == null) return;

        if (isNext)
            uiController.NextSlide();
        else
            uiController.PrevSlide();
    }*/
}
