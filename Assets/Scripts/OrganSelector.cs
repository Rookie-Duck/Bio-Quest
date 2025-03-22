using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrganSelector : MonoBehaviour
{
    public string correctOrganTag; // Tag of the correct organ
    public TMP_Text feedbackText; // Reference to the TMP Text element for feedback
    public TMP_Text organQuestionText; // Reference to the TMP Text element for the organ question

    private void Start()
    {
        // Set the initial question
        organQuestionText.text = "Find the " + correctOrganTag;
    }

    public void WhenSelect()
    {
        // Check if the selected organ matches the correct organ
        if (gameObject.CompareTag(correctOrganTag))
        {
            // Correct selection
            feedbackText.color = Color.green;
            feedbackText.text = "Correct!";
            // You can add additional logic here, such as moving to the next question
        }
        else
        {
            // Incorrect selection
            feedbackText.color = Color.red;
            feedbackText.text = "Try again!";
        }
    }
}
