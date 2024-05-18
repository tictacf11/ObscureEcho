using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameConfiguration gameConfiguration;
    [SerializeField] UIController uiController;
    [SerializeField] float timeToValidateMatch = .2f;
    [SerializeField] List<Sprite> cardsSprites;
    [SerializeField] Sprite cardsBackSprite;

    int columns;
    int rows;
    [SerializeField] DynamicGridLayoutGroup cardsGrid;
    [SerializeField] Card cardPrefab;
    [SerializeField] int scoreByMatch;

    List<Card> cards;
    Card currentSelectedCard;
    int totalPairsNumber;
    int currentMatches;

    int currentScore;
    int currentCombo;
    private string saveFilePath;
    private bool gameHasEnded = false;

    private void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.dat");

        if (gameConfiguration.loadGame && File.Exists(saveFilePath))
        {
            LoadGame();
        }
        else
        {
            rows = gameConfiguration.boardConfig.rows;
            columns = gameConfiguration.boardConfig.columns;
            InitializeBoard();
            InitializeCards();
            currentMatches = 0;
            currentScore = 0;
            currentCombo = 1;
        }
        File.Delete(saveFilePath);

    }

    private void InitializeBoard()
    {
        cardsGrid.UpdateCellSizeByRowsAndColumns(rows, columns);
        int cardNumber = columns * rows;
        totalPairsNumber = cardNumber / 2;
    }

    private void InitializeCards()
    {
        List<int> cardIds = SelectCardsIds(totalPairsNumber);
        ShuffleCardsIdsList(cardIds);
        SpawnAndInitializeCards(cardIds);
    }

    // selecting the cards that will used in the puzzle
    private List<int> SelectCardsIds(int numberOfPairs)
    {
        List<int> cardIds = new List<int>();
        for (int i = 0; i < totalPairsNumber; i++)
        {
            cardIds.Add(i);
            cardIds.Add(i);
        }
        return cardIds;
    }

    // shuffle the disposition of cards
    private void ShuffleCardsIdsList(List<int> cardIds)
    {
        for (int i = cardIds.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int tempCardId = cardIds[i];
            cardIds[i] = cardIds[randomIndex];
            cardIds[randomIndex] = tempCardId;
        }
    }

    private void SpawnAndInitializeCards(List<int> cardIds)
    {
        cards = new List<Card>();

        for (int i = 0; i < cardIds.Count; i++)
        {
            SpawnAndInitializeCard(cardIds[i]);
        }
    }

    private void SpawnAndInitializeCard(int cardId)
    {
        Card card = Instantiate(cardPrefab, cardsGrid.transform, false);
        card.Initialize(cardId, cardsBackSprite, cardsSprites[cardId]);
        card.OnCardSelected += OnCardSelected;
        cards.Add(card);
    }

    private void OnCardSelected(Card card)
    {
        if (!currentSelectedCard)
        {
            card.FlipToFront();
            currentSelectedCard = card;
        }
        else
        {
            bool thereIsAMach = IsAMatch(card);
            Card previousSelectedCard = currentSelectedCard;
            card.FlipToFront(() =>
            {
                if (thereIsAMach) StartCoroutine(OnValidMatchRoutine(card, previousSelectedCard));
                else StartCoroutine(OnInvalidMatchRoutine(card, previousSelectedCard));
            });
            currentSelectedCard = null;
        }
    }

    private bool IsAMatch(Card card)
    {
        return card.CardId == currentSelectedCard.CardId;
    }

    IEnumerator OnValidMatchRoutine(Card card1, Card card2)
    {
        currentMatches++;
        bool gameEnded = currentMatches >= totalPairsNumber;
        currentScore += currentCombo * scoreByMatch;
        uiController?.UpdateScore(currentScore, currentCombo);
        currentCombo *= 2;

        card1.IsMatched = true;
        card2.IsMatched = true;

        AudioManager.instance.PlaySound(AudioManager.instance.matchSound);
        yield return new WaitForSeconds(timeToValidateMatch);        
        if (gameEnded) EndGame(true);
    }

    IEnumerator OnInvalidMatchRoutine(Card card1, Card card2)
    {
        if (currentCombo > 1)
        {
            currentCombo = 1;
            uiController?.UpdateScore(currentScore, currentCombo);
        }
        AudioManager.instance.PlaySound(AudioManager.instance.mismatchSound);
        yield return new WaitForSeconds(timeToValidateMatch);
        card1.FlipToBack();
        card2.FlipToBack();
    }

    private void EndGame(bool allPairsWereFound)
    {
        gameHasEnded = true;
        AudioManager.instance.PlaySound(AudioManager.instance.gameOverSound);
        uiController.DisplayGameOverPanel();
    }

    private void OnApplicationQuit()
    {
        if (!gameHasEnded) SaveGame();
    }

    public void SaveGame()
    {
        int[] cardIds = new int[rows * columns];
        int[] disabledCardsIndexes = new int[currentMatches * 2];

        for (int i = 0, j = 0; i < cards.Count; i++)
        {
            cardIds[i] = cards[i].CardId;
            if (cards[i].IsMatched)
            {
                disabledCardsIndexes[j] = i;
                j++;
            }
        }

        GameState gameState = new GameState()
        {
            columns = columns,
            rows = rows,
            score = currentScore,
            combo = currentCombo,
            cardIds = cardIds,
            disabledCardsIndexes = disabledCardsIndexes,
        };

        using (FileStream fs = new FileStream(saveFilePath, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, gameState);
        }
    }

    private void LoadGame()
    {
        using (FileStream fs = new FileStream(saveFilePath, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            GameState gameState = (GameState)formatter.Deserialize(fs);

            rows = gameState.rows;
            columns = gameState.columns;
            InitializeBoard();

            SpawnAndInitializeCards(gameState.cardIds.ToList());
            currentMatches = gameState.disabledCardsIndexes.Length / 2;
            for (int i = 0; i < gameState.disabledCardsIndexes.Length; i++)
            {
                cards[gameState.disabledCardsIndexes[i]].enabled = false;
            }

            currentScore = gameState.score;
            currentCombo = gameState.combo;
            uiController.UpdateScore(currentScore, currentCombo / 2);

        }
    }
}
