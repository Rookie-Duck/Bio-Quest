using System.Collections.Generic;
using UnityEngine;

public class MatchTheOrganResultChecker : MonoBehaviour
{
    [System.Serializable]
    public class OrganResult
    {
        public string socketTag;   // e.g., "Socket_Brain"
        public string organTag;    // e.g., "Brain"
        public bool isCorrect;
    }

    public static List<OrganResult> globalResults = new List<OrganResult>();

    private void OnTriggerEnter(Collider other)
    {
        string socketTag = gameObject.tag; // e.g., "Socket_Brain"
        string expectedTag = socketTag.Replace("Socket_", "");

        string enteredTag = other.gameObject.tag; // e.g., "Brain"

        // Hapus entry lama untuk socket ini (bukan untuk organTag!)
        globalResults.RemoveAll(r => r.socketTag == socketTag);

        OrganResult result = new OrganResult
        {
            socketTag = socketTag,
            organTag = enteredTag,
            isCorrect = enteredTag == expectedTag
        };

        globalResults.Add(result);

        Debug.Log($"[ResultChecker] Socket {socketTag} menerima organ {enteredTag} → {(result.isCorrect ? "CORRECT" : "WRONG")}");
    }

    private void OnTriggerExit(Collider other)
    {
        string socketTag = gameObject.tag;
        string exitedTag = other.gameObject.tag;

        // Hapus entry jika yang keluar adalah organ yang sedang dicatat di socket ini
        globalResults.RemoveAll(r => r.socketTag == socketTag && r.organTag == exitedTag);

        Debug.Log($"[ResultChecker] Socket {socketTag} organ {exitedTag} dilepas");
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
