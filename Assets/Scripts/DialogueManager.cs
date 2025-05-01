using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogueUI;
    public TMP_Text dialogueText;
    public Transform choicesContainer;
    public Button choiceButtonPrefab;

    private Story story;
    private int currentDoor;

    /// <summary>
    /// Called by DoorInteraction (e.g. "door3").
    /// </summary>
    public void StartDialogue(string knotName)
    {
        currentDoor = int.Parse(knotName.Replace("door", ""));
        story = GameManager.Instance.GetStoryForDoor(currentDoor);
        story.ChoosePathString(knotName);

        dialogueUI.SetActive(true);
        RefreshView();
    }

    void RefreshView()
    {
        foreach (Transform t in choicesContainer)
            Destroy(t.gameObject);

        if (story.canContinue)
            dialogueText.text = story.Continue().Trim();
        else
            dialogueText.text = "";

        int count = story.currentChoices.Count;
        for (int i = 0; i < count; i++)
        {
            int choiceIndex = i;  // capture
            var choice = story.currentChoices[i];
            var btn = Instantiate(choiceButtonPrefab, choicesContainer);
            btn.GetComponentInChildren<TMP_Text>().text = choice.text.Trim();
            btn.onClick.AddListener(() => OnClickChoice(choiceIndex));
        }

        if (!story.canContinue && count == 0)
            EndDialogue();
    }

    void OnClickChoice(int choiceIndex)
    {
        bool choseYes     = (choiceIndex == 0);
        bool isCorrect    = GameManager.Instance.IsCorrectDoor(currentDoor);

        // advance Ink
        story.ChooseChoiceIndex(choiceIndex);
        RefreshView();

        // apply game rules
        if (isCorrect)
        {
            if (choseYes)
                GameManager.Instance.ReportVictory(currentDoor);
            else
                GameManager.Instance.RegisterWrongAnswer();
        }
        else
        {
            if (choseYes)
                GameManager.Instance.RegisterWrongAnswer();
            // “No” on wrong door is fine
        }

        EndDialogue();
    }

    void EndDialogue()
    {
        dialogueUI.SetActive(false);
    }
}