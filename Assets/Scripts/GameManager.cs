using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] float timeToValidateMatch = .5f;
    [SerializeField] List<Sprite> cardsSprites;
    [SerializeField] Sprite cardsBackSprite;

    [SerializeField] int colums;
    [SerializeField] int rows;
    [SerializeField] GridLayoutGroup cardsGrid;
    [SerializeField] Card cardPrefab;

    List<Card> cards;
    Card currentSelectedCard;
    int totalPairsNumber;
    int currentMatches;

    private void Start()
    {
        cardsGrid.constraintCount = colums;
        int cardNumber = colums * rows;
        totalPairsNumber = cardNumber / 2;
        currentMatches = 0; 

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
        yield return new WaitForSeconds(.5f);
        card1.enabled = false;
        card2.enabled = false;
        if (currentMatches >= totalPairsNumber) EndGame(true);
    }

    IEnumerator OnInvalidMatchRoutine(Card card1, Card card2)
    {
        print("It's no match...");
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
