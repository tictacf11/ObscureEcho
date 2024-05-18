using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreDisplay;

    // Start is called before the first frame update
    void Awake()
    {
        UpdateScore(0, 1);
    }

    public void UpdateScore(int score, int combo)
    {
        scoreText.text = score + "";
        comboText.text = combo > 1 ? "x" + combo : "";
    }

    public void DisplayGameOverPanel()
    {
        finalScoreDisplay.text = scoreText.text;
        gameOverPanel.SetActive(true);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
