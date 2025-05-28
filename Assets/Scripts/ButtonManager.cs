using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public MatchTheOrganShuffler organShuffler;

    public Transform humanOrgansParent;
    public GameObject findTheOrganGameManager;

    private Dictionary<Transform, Vector3> initialPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Quaternion> initialRotations = new Dictionary<Transform, Quaternion>();

    private int clickCount = 0;

    void Awake()
    {
        // Simpan posisi & rotasi awal organ secara rekursif
        SaveInitialTransformsRecursive(humanOrgansParent);
    }

    // Fungsi untuk mengulang posisi semua organ
    public void ResetAllOrgans()
    {
        foreach (var entry in initialPositions)
        {
            Transform organ = entry.Key;
            organ.localPosition = entry.Value;
            organ.localRotation = initialRotations[organ];

            if (!organ.gameObject.activeSelf)
                organ.gameObject.SetActive(true);
        }
    }

    // Fungsi untuk menyembunyikan bagian tubuh bertahap
    public void HideButton()
    {
        clickCount++;

        switch (clickCount)
        {
            case 1:
                SetBodyPartActive("Body-Front", false);
                break;
            case 2:
                SetBodyPartActive("Body-Back", false);
                break;
            case 3:
                ResetAndUnhideTagged("BodyFront");
                ResetAndUnhideTagged("BodyBack");
                clickCount = 0;
                break;
        }
    }

    private void ResetAndUnhideTagged(string tag)
    {
        foreach (var entry in initialPositions)
        {
            Transform t = entry.Key;
            if (t.CompareTag(tag))
            {
                t.localPosition = initialPositions[t];
                t.localRotation = initialRotations[t];
                t.gameObject.SetActive(true);
            }
        }
    }

    private void SetBodyPartActive(string name, bool isActive)
    {
        SetActiveRecursive(humanOrgansParent, name, isActive);
    }

    private void SetActiveRecursive(Transform parent, string name, bool isActive)
    {
        foreach (Transform t in parent)
        {
            if (t.name == name)
            {
                t.gameObject.SetActive(isActive);
                return;
            }

            if (t.childCount > 0)
                SetActiveRecursive(t, name, isActive);
        }
    }

    private void SaveInitialTransformsRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            initialPositions[child] = child.localPosition;
            initialRotations[child] = child.localRotation;

            if (child.childCount > 0)
                SaveInitialTransformsRecursive(child);
        }
    }

    // Mode: Find The Organ
    public void StartFindTheOrganGame()
    {
        ResetAllOrgans();

        if (findTheOrganGameManager != null)
        {
            findTheOrganGameManager.GetComponent<FindTheOrganGameManager>().RestartGame();
        }
        else
        {
            Debug.LogWarning("FindTheOrganGameManager belum di-assign!");
        }
    }

    // 🇬🇧 Set bahasa Inggris + mulai ulang
    public void SetLanguageToEnglish()
    {
        var organGame = findTheOrganGameManager.GetComponent<FindTheOrganGameManager>();
        if (organGame != null)
        {
            organGame.SetLanguage("english");
            StartFindTheOrganGame();
        }
    }

    // 🇮🇩 Set bahasa Indonesia + mulai ulang
    public void SetLanguageToIndonesian()
    {
        var organGame = findTheOrganGameManager.GetComponent<FindTheOrganGameManager>();
        if (organGame != null)
        {
            organGame.SetLanguage("indonesian");
            StartFindTheOrganGame();
        }
    }

    // Mode: Match The Organ
    public void ShuffleOrgansFromButton()
    {
        if (organShuffler != null)
            organShuffler.ShuffleOrgans();
        else
            Debug.LogWarning("Shuffler belum di-assign!");
    }
}
