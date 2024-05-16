using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = 0 + "";
        comboText.text = "";
    }

    public void UpdateScore(int score, int combo)
    {
        score.text = score + "";
    }
}
