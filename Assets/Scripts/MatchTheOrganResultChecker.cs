using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class MatchTheOrganResultChecker : MonoBehaviour
{
    public List<SnapInteractor> organSockets; // Assign via Inspector

    [System.Serializable]
    public class OrganResult
    {
        public string organTag;
        public bool isCorrect;
    }

    public List<OrganResult> GetOrganResults()
    {
        List<OrganResult> results = new List<OrganResult>();

        foreach (SnapInteractor socket in organSockets)
        {
            GameObject snappedObj = socket.HasInteractable ? socket.Interactable.gameObject : null;

            string socketTag = socket.gameObject.tag;                // e.g., "Socket_Brain"
            string expectedTag = socketTag.Replace("Socket_", "");   // e.g., "Brain"
            string organTag = snappedObj != null ? snappedObj.tag : "None";

            bool isCorrect = organTag == expectedTag;

            results.Add(new OrganResult
            {
                organTag = expectedTag,
                isCorrect = isCorrect
            });
        }

        return results;
    }
}
