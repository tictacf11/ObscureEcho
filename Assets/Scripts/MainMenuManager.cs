using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager: MonoBehaviour
{
    [SerializeField] GameConfiguration gameConfiguration;
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject NewGamePanel;
    [SerializeField] private Button loadGameButton;
    private string saveFilePath;

    private void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.dat");
        if (File.Exists(saveFilePath)) loadGameButton.interactable = true;
        else loadGameButton.interactable = false;    
    }

    public void OpenNewGameMenu()
    {
        startMenuPanel.SetActive(false);
        NewGamePanel.SetActive(true);
    }

    public void BackToStartMenu()
    {
        startMenuPanel.SetActive(true);
        NewGamePanel.SetActive(false);
    }

    public void StartLoadedGame()
    {
        gameConfiguration.loadGame = true;
        SceneManager.LoadScene(1);
    }

    public void StartNewGame(BoardConfiguration boardConfig)
    {
        gameConfiguration.boardConfig = boardConfig;
        gameConfiguration.loadGame = false;
        SceneManager.LoadScene(1);
    }
}