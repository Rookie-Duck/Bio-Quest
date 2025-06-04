using UnityEngine;
using Oculus.Interaction;

public class ResumePanelPoke : MonoBehaviour
{
    public ResumeUIManager resumeUIManager;
    public enum ResumeAction { Resume, SelectLevel, ReturnHome }
    public ResumeAction action;

    public SelectLevelUIManager selectLevelUIManager;

    public void OnPoke()
    {
        if (resumeUIManager == null) return;

        switch (action)
        {
            case ResumeAction.Resume:
                resumeUIManager.OnResumePressed();
                break;
            case ResumeAction.SelectLevel:
                resumeUIManager.OnSelectLevelPressed();
                break;
            case ResumeAction.ReturnHome:
                resumeUIManager.OnReturnHomePressed();
                break;
        }
    }
}
