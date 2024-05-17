using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIController uiController;
    [SerializeField] float timeToValidateMatch = .5f;
    [SerializeField] List<Sprite> cardsSprites;
    [SerializeField] Sprite cardsBackSprite;

    [SerializeField] int colums;
    [SerializeField] int rows;
    [SerializeField] DynamicGridLayoutGroup cardsGrid;
    [SerializeField] Card cardPrefab;
    [SerializeField] int scoreByMatch;

    List<Card> cards;
    Card currentSelectedCard;
    int totalPairsNumber;
    int currentMatches;

    int currentScore;
    int currentCombo;

    private void Start()
    {
        cardsGrid.UpdateCellSizeByRowsAndColumns(rows, colums);
        int cardNumber = colums * rows;
        totalPairsNumber = cardNumber / 2;
        currentMatches = 0; 

        List<int> cardIds = SelectCardsIds(totalPairsNumber);
        ShuffleCardsIdsList(cardIds);
        SpawnAndInitializeCards(cardIds);
        currentScore = 0;   
        currentCombo = 1;
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
            int randomIndex = Random.Range(0, i+1);
            int tempCardId = cardIds[i];
            cardIds[i] = cardIds[randomIndex];
            cardIds[randomIndex] = tempCardId;
        }
    }

    private void SpawnAndInitializeCards(List<int> cardIds)
    {
        cards = new List<Card>();

        for (int i = 0;i < cardIds.Count; i++)
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
        card.FlipToFront();
        if(!currentSelectedCard) 
        {
            currentSelectedCard = card;
        }
        else
        {
            if (IsAMatch(card)) StartCoroutine(OnValidMatchRoutine(card, currentSelectedCard));
            else StartCoroutine(OnInvalidMatchRoutine(card, currentSelectedCard));
            currentSelectedCard = null;
        }
    }

    private bool IsAMatch(Card card)
    {
        return card.CardId == currentSelectedCard.CardId;
    }

    IEnumerator OnValidMatchRoutine(Card card1, Card card2)
    {
        print("It's a match!");

        currentMatches++;
        currentScore += currentCombo * scoreByMatch;
        uiController?.UpdateScore(currentScore, currentCombo);
        currentCombo *= 2;

        yield return new WaitForSeconds(.5f);
        card1.enabled = false;
        card2.enabled = false;
        if (currentMatches >= totalPairsNumber) EndGame(true);
    }

    IEnumerator OnInvalidMatchRoutine(Card card1, Card card2)
    {
        print("It's no match...");
        if(currentCombo > 1)
        {
            currentCombo = 1;
            uiController?.UpdateScore(currentScore, currentCombo);
        }
        yield return new WaitForSeconds(timeToValidateMatch);
        card1.FlipToBack();
        card2.FlipToBack();
    }

    private void EndGame(bool allPairsWereFound)
    {
        if (allPairsWereFound) print("Bravo! You matched everything");
        else print("Time out... better luck next time");
    }
}
