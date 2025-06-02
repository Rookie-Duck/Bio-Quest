using Oculus.Interaction.Samples;
using System.Collections.Generic;
using UnityEngine;


public class MatchTheOrganGameManager : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject startCanvas;
    public GameObject resultCanvas;

    public MatchTheOrganResultChecker resultChecker;
    public MatchTheOrganResultUI resultUI;

    private bool gameRunning = false;

    public void StartMatchOrganGame()
    {
        if (gameRunning) return;

        startCanvas.SetActive(true);
        resultCanvas.SetActive(false);

        gameRunning = true;
    }

    public void OnCancel()
    {
        startCanvas.SetActive(false);
        resultCanvas.SetActive(false);
        gameRunning = false;
    }

    public void OnFinish()
    {
        startCanvas.SetActive(false);
        resultCanvas.SetActive(true);
        ShowResults();
        gameRunning = false;
    }

    private void ShowResults()
    {
        var results = resultChecker.GetOrganResults();
        resultUI.ShowResults(results);
    }
}
