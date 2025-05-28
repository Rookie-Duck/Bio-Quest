using System.Collections.Generic;
using UnityEngine;

public class MatchTheOrganShuffler : MonoBehaviour
{
    public Transform organParent; // Parent dari semua organ
    public BoxCollider shuffleArea; // Area tempat organ akan diacak
    public float minDistance = 0.15f; // Jarak minimum antar organ
    public int maxAttempts = 20; // Usaha maksimal mencari posisi yang tidak tabrakan

    private List<Transform> organsToShuffle = new List<Transform>();
    private List<Vector3> usedPositions = new List<Vector3>();

    private readonly string[] organTags = new string[]
    {
        "Brain", "NoseCavity", "OralCavity", "Trachea", "Lungs", "Heart",
        "Liver", "Stomach", "Gallbladder", "Pancreas", "Kidney", "LargeIntestine", "SmallIntestine"
    };

    void Awake()
    {
        organsToShuffle.Clear();
        AddOrgansRecursive(organParent);
    }

    void AddOrgansRecursive(Transform parent)
    {
        foreach (Transform t in parent)
        {
            foreach (string tag in organTags)
            {
                if (t.CompareTag(tag))
                {
                    organsToShuffle.Add(t);
                    break;
                }
            }

            if (t.childCount > 0)
            {
                AddOrgansRecursive(t); // cek anak-anaknya juga
            }
        }
    }

    public void ShuffleOrgans()
    {
        usedPositions.Clear();

        foreach (Transform organ in organsToShuffle)
        {
            Vector3 newPosition = GetValidRandomPosition();

            if (newPosition != Vector3.zero)
            {
                organ.localPosition = newPosition;
            }
            else
            {
                Debug.LogWarning($"Organ {organ.name} gagal diacak setelah {maxAttempts} percobaan.");
            }
        }
    }

    private Vector3 GetValidRandomPosition()
    {
        Bounds bounds = shuffleArea.bounds;
        Vector3 localCenter = shuffleArea.transform.InverseTransformPoint(bounds.center);
        Vector3 localSize = bounds.size;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 localPos = new Vector3(
                Random.Range(-localSize.x / 2f, localSize.x / 2f),
                Random.Range(-localSize.y / 2f, localSize.y / 2f),
                Random.Range(-localSize.z / 2f, localSize.z / 2f)
            );

            Vector3 worldPos = shuffleArea.transform.TransformPoint(localCenter + localPos);

            if (IsFarEnough(worldPos))
            {
                usedPositions.Add(worldPos);
                return organParent.InverseTransformPoint(worldPos); // convert ke local position
            }
        }

        return Vector3.zero;
    }

    private bool IsFarEnough(Vector3 pos)
    {
        foreach (var used in usedPositions)
        {
            if (Vector3.Distance(pos, used) < minDistance)
                return false;
        }
        return true;
    }
}
