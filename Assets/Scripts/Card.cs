using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [NonSerialized] public int CardId;
    [NonSerialized] public bool IsMatched;
    bool isFaceDown = true;
    Image image;
    Sprite backImageSprite;
    Sprite frontImageSprite;
    Button button;
    public Action<Card> OnCardSelected;
    CardAnimator animator;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        animator = GetComponent<CardAnimator>();

        button.onClick.AddListener(SelectCard);
    }

    public void Initialize(int id, Sprite backImage, Sprite frontImage)
    {
        CardId = id;
        backImageSprite = backImage;
        frontImageSprite = frontImage;

        image.sprite = isFaceDown ? backImage : frontImage;
    }

    private void SelectCard()
    {
        OnCardSelected?.Invoke(this);
    }

    public void FlipToFront(Action onFlipEnd = null)
    {
        if (!isFaceDown) return;
        button.enabled = false;
        isFaceDown = false;
        animator.Flip(
            () => image.sprite = frontImageSprite,
            onFlipEnd,
            false
            );
    }

    public void FlipToBack(Action onFlipEnd = null)
    {
        if (isFaceDown) return;
        isFaceDown = true;
        onFlipEnd += () => button.enabled = true;
        animator.Flip(
            () => image.sprite = backImageSprite,
            onFlipEnd
            );
    }

    private void OnDisable()
    {
        IsMatched = true;
        image.enabled = false;
        button.enabled = false;
    }
}
