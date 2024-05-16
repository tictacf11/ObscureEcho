using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float timeToValidateMatch = .5f;
    [SerializeField] List<Sprite> cardsSprites;
    [SerializeField] Sprite cardsBackSprite;
    [SerializeField] List<Card> cards;

    Card currentSelectedCard;

    private void Start()
    {
        SpawnAndInitializeCard(0, 0);
        SpawnAndInitializeCard(1, 0);
        SpawnAndInitializeCard(2, 1);
        SpawnAndInitializeCard(3, 1);
    }

    private void SpawnAndInitializeCard(int cardIndex, int cardId)
    {
        Card card = cards[cardIndex];
        card.Initialize(cardId, cardsBackSprite, cardsSprites[cardId]);
        card.OnCardSelected += OnCardSelected;
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
        yield return new WaitForSeconds(.5f);
        card1.enabled = false;
        card2.enabled = false;
    }

    IEnumerator OnInvalidMatchRoutine(Card card1, Card card2)
    {
        print("It's no match...");
        yield return new WaitForSeconds(timeToValidateMatch);
        card1.FlipToBack();
        card2.FlipToBack();
    }
}
