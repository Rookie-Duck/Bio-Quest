using UnityEngine;

public class SocketChecker : MonoBehaviour
{
    public string attachedTag = "None";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Untagged"))
        {
            attachedTag = other.tag;
            Debug.Log($"[{name}] menerima organ bertag {attachedTag}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == attachedTag)
        {
            attachedTag = "None";
            Debug.Log($"[{name}] organ dilepas");
        }
    }
}
