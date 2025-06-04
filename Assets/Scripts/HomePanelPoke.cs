using UnityEngine;

public class HomePanelPoke : MonoBehaviour
{
    public SelectLevelUIManager uiManager;
    public enum ActionType { Resume, SelectLevel, Home }
    public ActionType action;

    public void OnPoke()
    {
        switch (action)
        {
            case ActionType.Resume:
                uiManager.OnResumePressed();
                break;
            case ActionType.SelectLevel:
                uiManager.OnSelectLevelPressed();
                break;
            case ActionType.Home:
                uiManager.OnHomePressed();
                break;
        }
    }
}
