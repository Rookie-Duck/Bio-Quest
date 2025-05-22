using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonManager : MonoBehaviour
{
    public Transform humanOrgansParent;

    private Dictionary<Transform, Vector3> initialPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Quaternion> initialRotations = new Dictionary<Transform, Quaternion>();

    public GameObject findTheOrganGameManager;

    private int clickCount = 0;

    void Awake()
    {
        // Recursive simpan semua posisi & rotasi anak-anak dari humanOrgansParent
        SaveInitialTransformsRecursive(humanOrgansParent);
    }

    public void ResetAllOrgans()
    {
        foreach (var entry in initialPositions)
        {
            Transform organ = entry.Key;
            Vector3 originalPos = entry.Value;
            Quaternion originalRot = initialRotations[organ];

            organ.localPosition = originalPos;
            organ.localRotation = originalRot;

            // Optional: aktifkan lagi jika sempat dinonaktifkan
            if (!organ.gameObject.activeSelf)
                organ.gameObject.SetActive(true);
        }
    }

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
                string[] tagsToReset = { "BodyFront", "BodyBack" };
                foreach (string tag in tagsToReset)
                    ResetAndUnhideTagged(tag);

                clickCount = 0;
                break;
        }
    }

    // Hiding Object Function
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
                return; // stop setelah ketemu
            }

            if (t.childCount > 0)
                SetActiveRecursive(t, name, isActive);
        }
    }

    void SaveInitialTransformsRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            initialPositions[child] = child.localPosition;
            initialRotations[child] = child.localRotation;

            // Rekursif kalau punya anak lagi
            if (child.childCount > 0)
                SaveInitialTransformsRecursive(child);
        }
    }
    public void StartFindTheOrganGame()
    {
        ResetAllOrgans();

        if (findTheOrganGameManager != null)
        {
            findTheOrganGameManager.GetComponent<FindTheOrganGameManager>().OnPlayButtonPoke();
        }
        else
        {
            Debug.LogWarning("FindTheOrganGameManager not assigned di ButtonManager!");
        }
    }

}
