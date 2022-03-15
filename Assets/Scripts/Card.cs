using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Suits
{
    Clubs,
    Diamonds,
    Hearts,
    Spades
}
public enum Cards
{
    Ace,
    Two,
    Three,
    Four,
    Five,
    Six,
    Jack,
    Queen,
    King
}

public class Card : MonoBehaviour
{
    #region Properties
    public Cards card { get; private set; }
    public Suits suit { get; private set; }
    #endregion

    #region Private Fields
    private SpriteRenderer spriteRenderer;
    private int faceUpSpriteIndex = 0;
    #endregion

    #region Public Fields
    public Sprite[] cardSprites;
    #endregion

    #region Private Methods
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeCardAndSuit(Cards.Three, Suits.Clubs);
    }
    private void ChangeFaceUpSprite(Cards card, Suits suit)
    {
        // Карты в массиве cardSprites расположены по порядку так что зная масть, мы можем сместить начальную позицию в массиве что бы достать верный спрайт
        faceUpSpriteIndex = 1;
        int cardsOfSameSuit = 13;
        int suitIndex = Convert.ToInt32(suit);
        int cardIndex = Convert.ToInt32(card);

        faceUpSpriteIndex += cardsOfSameSuit * suitIndex;
        faceUpSpriteIndex += cardIndex;

        if (faceUpSpriteIndex < cardSprites.Length)
        {
            spriteRenderer.sprite = cardSprites[faceUpSpriteIndex];
        }
        else
        {
            Debug.LogError($"spriteIndex, {faceUpSpriteIndex}, wend beyond cardSprites array");
        }
    }
    private void OnMouseEnter()
    {
        TurnFaceUp();
    }
    private void OnMouseExit()
    {
        TurnFaceDown();
    }
    #endregion

    #region Public Methods
    public void ChangeCardAndSuit(Cards card, Suits suit)
    {
        this.card = card;
        this.suit = suit;

        ChangeFaceUpSprite(card, suit);
    }
    public void TurnFaceUp()
    {
        spriteRenderer.sprite = cardSprites[faceUpSpriteIndex];
    }
    public void TurnFaceDown()
    {
        spriteRenderer.sprite = cardSprites[0];
    }
    #endregion
}
