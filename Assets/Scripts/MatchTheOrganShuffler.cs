using System.Collections.Generic;
using UnityEngine;

public class MatchTheOrganShuffler : MonoBehaviour
{
    [Header("Shuffle Settings")]
    public Transform organParent;
    public BoxCollider shuffleArea;
    public float minDistance = 0.15f;
    public int maxAttempts = 20;

    [Header("Optional Object to Reset")]
    [SerializeField] private Transform bodyFront;

    private List<Transform> organsToShuffle = new List<Transform>();
    public List<Transform> OrgansToShuffle => organsToShuffle;

    private Vector3 initialBodyFrontPos;
    private Quaternion initialBodyFrontRot;

    private Dictionary<Transform, Vector3> originalPositions = new();
    private Dictionary<Transform, Quaternion> originalRotations = new();

    private readonly string[] organTags = new string[]
    {
        "Brain", "NoseCavity", "OralCavity", "Trachea", "Lungs", "Heart",
        "Liver", "Stomach", "Gallbladder", "Pancreas", "Kidney", "LargeIntestine", "SmallIntestine"
    };

    void Awake()
    {
        organsToShuffle.Clear();
        AddOrgansRecursive(organParent);

        foreach (Transform organ in organsToShuffle)
        {
            originalPositions[organ] = organ.position;
            originalRotations[organ] = organ.rotation;
        }

        if (bodyFront != null)
        {
            initialBodyFrontPos = bodyFront.position;
            initialBodyFrontRot = bodyFront.rotation;
        }
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
                AddOrgansRecursive(t);
            }
        }
    }

    public void ShuffleOrgans()
    {
        const int maxRetries = 5;
        bool success = false;

        for (int attempt = 0; attempt < maxRetries && !success; attempt++)
        {
            success = TryShuffleOnce();
        }

        if (!success)
        {
            Debug.LogWarning("Gagal mengacak semua organ setelah beberapa percobaan.");
        }
    }

    private bool TryShuffleOnce()
    {
        List<Vector3> usedPositions = new List<Vector3>();
        Bounds bounds = shuffleArea.bounds;
        bool allSuccess = true;

        foreach (Transform organ in organsToShuffle)
        {
            bool found = false;

            for (int i = 0; i < maxAttempts; i++)
            {
                Vector3 randomPoint = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y),
                    Random.Range(bounds.min.z, bounds.max.z)
                );

                bool tooClose = false;
                foreach (var used in usedPositions)
                {
                    if (Vector3.Distance(randomPoint, used) < minDistance)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    organ.position = randomPoint;
                    usedPositions.Add(randomPoint);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Debug.LogWarning($"Gagal cari posisi untuk organ: {organ.name}");
                allSuccess = false;
            }
        }

        return allSuccess;
    }

    public void ResetAllOrgans()
    {
        foreach (Transform organ in organsToShuffle)
        {
            if (originalPositions.TryGetValue(organ, out var pos))
                organ.position = pos;
            if (originalRotations.TryGetValue(organ, out var rot))
                organ.rotation = rot;
        }

        if (bodyFront != null)
        {
            bodyFront.position = initialBodyFrontPos;
            bodyFront.rotation = initialBodyFrontRot;
        }
    }
}
