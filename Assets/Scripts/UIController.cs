using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreDisplay;
    [SerializeField] private OnOffButton audioButton;

    // Start is called before the first frame update
    void Awake()
    {
        UpdateScore(0, 1);
        if(audioButton != null)
        {
            audioButton.IsOn = !AudioManager.instance.IsMuted;
            audioButton.OnSetOn.AddListener(AudioManager.instance.Unmute);
            audioButton.OnSetOff.AddListener(AudioManager.instance.Mute);
        }  
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
