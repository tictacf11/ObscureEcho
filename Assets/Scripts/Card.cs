using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [NonSerialized]public int CardId;
    bool isFaceDown = true;
    Image image;
    Sprite backImageSprite;
    Sprite frontImageSprite;
    Button button;
    public Action<Card> OnCardSelected;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

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

    public void FlipToFront()
    {
        if(!isFaceDown) return;
        button.enabled = false;
        isFaceDown = false;
        image.sprite = frontImageSprite;
    }

    public void FlipToBack()
    {
        if(isFaceDown) return;
        isFaceDown = true;
        image.sprite = backImageSprite;
        button.enabled = true;
    }

    private void OnDisable()
    {
        image.enabled = false;
        button.enabled = false;
    }
}
