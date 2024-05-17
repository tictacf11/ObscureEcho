using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore(0, 1);
    }

    public void UpdateScore(int score, int combo)
    {
        scoreText.text = score + "";
        comboText.text = combo > 1 ? "x" + combo : "";
    }
}
