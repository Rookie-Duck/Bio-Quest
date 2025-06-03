using System.Collections.Generic;
using UnityEngine;

public class MatchTheOrganResultChecker : MonoBehaviour
{
    [System.Serializable]
    public class OrganResult
    {
        public string organTag;    // e.g., "Brain"
        public string attachedTag; // e.g., "Brain" if matched
        public bool isCorrect;
    }

    public static List<OrganResult> globalResults = new List<OrganResult>();

    private void OnTriggerEnter(Collider other)
    {
        string socketTag = gameObject.tag; // e.g., "Socket_Brain"
        string expectedTag = socketTag.Replace("Socket_", "");

        string enteredTag = other.gameObject.tag; // e.g., "Brain"

        // Hindari duplikat jika sudah pernah masuk
        globalResults.RemoveAll(r => r.organTag == expectedTag);

        OrganResult result = new OrganResult
        {
            organTag = expectedTag,
            attachedTag = enteredTag,
            isCorrect = enteredTag == expectedTag
        };

        globalResults.Add(result);
    }

    private void OnTriggerExit(Collider other)
    {
        string socketTag = gameObject.tag;
        string expectedTag = socketTag.Replace("Socket_", "");

        globalResults.RemoveAll(r => r.organTag == expectedTag);
    }

    public static List<OrganResult> GetResults()
    {
        return globalResults;
    }

    public static void ClearResults()
    {
        globalResults.Clear();
    }

}
