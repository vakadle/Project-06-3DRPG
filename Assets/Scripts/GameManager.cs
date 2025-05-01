using UnityEngine;
using Ink.Runtime;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Ink Story")]
    [Tooltip("The compiled JSON generated from your Ink script")]
    public TextAsset inkJSONAsset;

    [Header("UI")]
    [Tooltip("TextMeshProUGUI that shows the count of wrong answers")]
    public TMP_Text progressText;
    [Tooltip("Panel to show when the player wins or on Game Over")]
    public GameObject winPanel;
    [Tooltip("TextMeshProUGUI inside winPanel for custom message")]
    public TMP_Text winPanelText;

    [Header("Intro UI")]
    [Tooltip("Panel with instructions and Start button")]
    public GameObject instructionPanel;

    [Header("Game Rules")]
    [Tooltip("How many wrong answers before Game Over")]
    public int maxWrongAnswers = 3;

    // The six canonical clues in order door1→door6
    private readonly string[] masterClues = {
        "yellow flower",
        "stone",
        "barrel",
        "vase",
        "sack of grain",
        "coat rack with clothes"
    };

    private string[] doorClues;
    private bool[]   doorIsCorrect;
    private int      wrongAnswers = 0;

    void Awake()
    {
        // Singleton
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // Hide win/GameOver panel
        if (winPanel != null) winPanel.SetActive(false);

        // Ensure winPanelText is hooked up
        if (winPanelText == null && winPanel != null)
            winPanelText = winPanel.GetComponentInChildren<TMP_Text>();

        // Initialize progress text
        if (progressText != null)
            progressText.text = $"0/{maxWrongAnswers}";

        // Choose a random correct door (index 0–5)
        int correctIndex = Random.Range(0, masterClues.Length);

        // Build list of wrong clues by excluding the correct door’s clue
        var wrongClues = new List<string>();
        for (int i = 0; i < masterClues.Length; i++)
        {
            if (i != correctIndex)
                wrongClues.Add(masterClues[i]);
        }
        Shuffle(wrongClues);

        // Allocate arrays and assign
        doorClues     = new string[6];
        doorIsCorrect = new bool[6];
        int w = 0;
        for (int i = 0; i < 6; i++)
        {
            if (i == correctIndex)
            {
                doorClues[i]     = masterClues[i];
                doorIsCorrect[i] = true;
            }
            else
            {
                doorClues[i]     = wrongClues[w++];
                doorIsCorrect[i] = false;
            }
        }

        // Debug assignment
        for (int i = 0; i < 6; i++)
            Debug.Log($"[Init] Door {i+1}: clue=\"{doorClues[i]}\", correct={doorIsCorrect[i]}");
    }

    void Start()
    {
        // Show intro, hide gameplay UI until Start is pressed
        if (instructionPanel != null) instructionPanel.SetActive(true);
        if (progressText    != null) progressText.gameObject.SetActive(false);
        if (winPanel        != null) winPanel.SetActive(false);
    }

    /// <summary>
    /// Called by the Start button’s OnClick in the Inspector.
    /// Begins the game by hiding instructions and revealing progress UI.
    /// </summary>
    public void BeginGame()
    {
        if (instructionPanel != null) instructionPanel.SetActive(false);
        if (progressText     != null) progressText.gameObject.SetActive(true);
        Debug.Log("[GameManager] Game started");
    }

    // Fisher–Yates shuffle for List<string>
    private void Shuffle(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var tmp = list[i];
            list[i]   = list[j];
            list[j]   = tmp;
        }
    }

    /// <summary>
    /// Returns a fresh Ink Story for doorNumber (1–6),
    /// with doorXClue and doorXClue_correct injected.
    /// </summary>
    public Story GetStoryForDoor(int doorNumber)
    {
        var story = new Story(inkJSONAsset.text);
        for (int i = 1; i <= 6; i++)
        {
            story.variablesState[$"door{i}Clue"]         = doorClues[i - 1];
            story.variablesState[$"door{i}Clue_correct"] = doorIsCorrect[i - 1];
        }
        Debug.Log($"[GetStory] door {doorNumber}: clue=\"{doorClues[doorNumber-1]}\", correct={doorIsCorrect[doorNumber-1]}");
        return story;
    }

    /// <summary>
    /// Returns true if that door was chosen correct at startup.
    /// </summary>
    public bool IsCorrectDoor(int doorNumber)
    {
        return doorIsCorrect[doorNumber - 1];
    }

    /// <summary>
    /// Called when the player gives a wrong answer.
    /// Updates UI and triggers Game Over if limit reached.
    /// </summary>
    public void RegisterWrongAnswer()
    {
        wrongAnswers++;
        Debug.Log($"Wrong answers: {wrongAnswers}/{maxWrongAnswers}");
        if (progressText != null)
            progressText.text = $"{wrongAnswers}/{maxWrongAnswers}";
        if (wrongAnswers >= maxWrongAnswers)
            ShowWinPanel("Game Over\nToo many wrong answers.");
    }

    /// <summary>
    /// Called when the player picks "Yes" on the correct door.
    /// </summary>
    public void ReportVictory(int doorNumber)
    {
        Debug.Log($"🎉 VICTORY! You found your house at door {doorNumber}! 🎉");
        ShowWinPanel("Congratulations!\nYou found your house!");
    }

    private void ShowWinPanel(string message)
    {
        if (winPanelText != null) winPanelText.text = message;
        if (winPanel    != null) winPanel.SetActive(true);
    }

    /// <summary>
    /// Wire your Restart button's OnClick() to this in the Inspector.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}