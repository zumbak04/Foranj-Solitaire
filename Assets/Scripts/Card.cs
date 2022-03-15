using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Cards suit { get; private set; }
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

        ChangeCardAndSuit(Cards.Ace, Suits.Hearts);
    }
    private void ChangeFaceUpSprite(Cards card, Suits suit)
    {
        // Карты в массиве cardSprites расположены по порядку так что зная масть, мы можем сместить начальную позицию в массиве что бы достать верный спрайт
        faceUpSpriteIndex = 1;
        int cardsOfSameSuit = 13;
        switch (suit)
        {
            case Suits.Clubs:
                break;
            case Suits.Diamonds:
                faceUpSpriteIndex += cardsOfSameSuit * 1;
                break;
            case Suits.Hearts:
                faceUpSpriteIndex += cardsOfSameSuit * 2;
                break;
            case Suits.Spades:
                faceUpSpriteIndex += cardsOfSameSuit * 3;
                break;
        }
        switch (card)
        {
            case Cards.Ace:
                break;
            case Cards.Two:
                faceUpSpriteIndex += 1;
                break;
            case Cards.Three:
                faceUpSpriteIndex += 2;
                break;
            case Cards.Four:
                faceUpSpriteIndex += 3;
                break;
            case Cards.Five:
                faceUpSpriteIndex += 4;
                break;
            case Cards.Six:
                faceUpSpriteIndex += 5;
                break;
            case Cards.Jack:
                faceUpSpriteIndex += 6;
                break;
            case Cards.Queen:
                faceUpSpriteIndex += 7;
                break;
            case Cards.King:
                faceUpSpriteIndex += 8;
                break;
        }

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
        this.suit = card;

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
