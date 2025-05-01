using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoorInteraction : MonoBehaviour
{
    [Header("Ink & UI")]
    public string knotName;
    public DialogueManager dialogueManager;

    [Header("Highlight & Prompt")]
    public GameObject highlightBox;      // tvůj child pro zvýraznění
    public GameObject promptUI;          // tvůj PromptText GameObject

    private bool playerInRange = false;

    void Reset()
    {
        Collider c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            highlightBox.SetActive(true);
            promptUI.SetActive(true);
            Debug.Log($"[DoorInteraction] Entered '{gameObject.name}'");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            highlightBox.SetActive(false);
            promptUI.SetActive(false);
            Debug.Log($"[DoorInteraction] Exited '{gameObject.name}'");
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"[DoorInteraction] Starting dialogue '{knotName}'");
            promptUI.SetActive(false);
            dialogueManager.StartDialogue(knotName);
        }
    }
}