using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganHider : MonoBehaviour
{
    private int currentHideIndex = 0;

    private readonly string[] orderedHideList = new string[]
    {
        "BodyFront", 
        "Lungs", "Heart", "Liver", "Stomach", "Gallbladder", "Pancreas", "Kidney",
        "LargeIntestine", "SmallIntestine",
        "BodyBack" 
    };

    public void HideNext()
    {
        if (currentHideIndex >= orderedHideList.Length)
        {
            Debug.Log("Semua organ sudah di-hide.");
            return;
        }

        string tag = orderedHideList[currentHideIndex];
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

        if (targets.Length == 0)
        {
            Debug.LogWarning($"Tidak ada GameObject dengan tag '{tag}' ditemukan.");
        }
        else
        {
            foreach (GameObject obj in targets)
            {
                obj.SetActive(false);
                Debug.Log($"Hide: {obj.name} (Tag: {tag})");
            }
        }

        currentHideIndex++;
    }

    public void UnhideAll()
    {
        foreach (string tag in orderedHideList)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in targets)
            {
                obj.SetActive(true);
            }
        }

        currentHideIndex = 0;
        Debug.Log("Semua organ & body berhasil di-unhide dan reset.");
    }
}
