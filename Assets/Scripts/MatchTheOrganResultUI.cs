using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchTheOrganResultUI : MonoBehaviour
{
    [Header("UI Slot Per Slide")]
    public Image[] resultIcons;       // Assign: Check_1, Check_2, Check_3
    public TMP_Text[] resultTexts;    // Assign: Text_1, Text_2, Text_3

    [Header("Navigation")]
    public GameObject nextButton;
    public GameObject prevButton;
    public GameObject closeButton;

    [Header("Sprites")]
    public Sprite correctSprite;
    public Sprite wrongSprite;

    private List<MatchTheOrganResultChecker.OrganResult> allResults;
    private int currentPage = 0;
    private const int itemsPerPage = 3;

    public void ShowResults(List<MatchTheOrganResultChecker.OrganResult> results)
    {
        allResults = results;
        currentPage = 0;
        UpdatePage();
    }

    private void UpdatePage()
    {
        int start = currentPage * itemsPerPage;

        for (int i = 0; i < itemsPerPage; i++)
        {
            int index = start + i;

            if (index < allResults.Count)
            {
                resultIcons[i].gameObject.SetActive(true);
                resultTexts[i].gameObject.SetActive(true);

                var result = allResults[index];
                resultIcons[i].sprite = result.isCorrect ? correctSprite : wrongSprite;

                resultTexts[i].text =
                    result.attachedTag == "None"
                        ? $"{result.organTag} (Empty)"
                        : result.isCorrect
                            ? result.organTag
                            : $"{result.attachedTag} → {result.organTag}";
            }
            else
            {
                resultIcons[i].gameObject.SetActive(false);
                resultTexts[i].gameObject.SetActive(false);
            }
        }

        prevButton.SetActive(currentPage > 0);
        nextButton.SetActive((currentPage + 1) * itemsPerPage < allResults.Count);
    }

    public void NextPage() { currentPage++; UpdatePage(); }
    public void PrevPage() { currentPage--; UpdatePage(); }
    public void CloseResults()
    {
        gameObject.SetActive(false);
    }
}
