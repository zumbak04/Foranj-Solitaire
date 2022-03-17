using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

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
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King
}

public class Card : MonoBehaviour
{
    #region Properties
    public Cards card { get; private set; }
    public Suits suit { get; private set; }
    public Card Child { get; private set; }
    public Card Parent { get; private set; }
    #endregion

    #region Private Fields
    public SpriteRenderer spriteRenderer;
    public Collider2D collider;
    private int faceUpSpriteIndex = 0;
    #endregion

    #region Public Fields
    public Sprite[] cardSprites;
    public Desk desk;
    #endregion

    #region Private Methods
    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }
    private void ChangeFaceUpSprite(Cards card, Suits suit)
    {
        // Карты в массиве cardSprites расположены по порядку так что зная масть, мы можем сместить начальную позицию в массиве что бы достать верный спрайт
        faceUpSpriteIndex = 1;
        int cardsOfSameSuit = Enum.GetNames(typeof(Cards)).Length;
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
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.instance.ClickOnCard(this);
        }
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
        collider.enabled = true;
    }
    public void TurnFaceDown()
    {
        spriteRenderer.sprite = cardSprites[0];
        collider.enabled = false;
    }
    public void SetChild(Card newChild)
    {
        if (!(Child is null))
        {
            Child.SetParent(null);
        }
        if (!(newChild is null))
        {
            newChild.SetParent(this);
        }
    }
    public void SetParent(Card newParent)
    {
        if (!(Parent is null))
        {
            Parent.Child = null;
        }

        Parent = newParent;
        if (!(newParent is null))
        {
            newParent.Child = this;
        }
    }
    public bool NextToCard(Card card)
    {
        int cardIndex = Convert.ToInt32(this.card);
        int nextCardIndex = Convert.ToInt32(card.card);
        int indexDif = cardIndex - nextCardIndex;
        int lastEnumIndex = Enum.GetNames(typeof(Cards)).Length - 1;
        Debug.Log(indexDif);

        return (indexDif == 1 || indexDif == -1 || indexDif == lastEnumIndex || indexDif == -lastEnumIndex);
    }
    public int GetCardIndex()
    {
        return Convert.ToInt32(card);
    }
    #endregion
}
