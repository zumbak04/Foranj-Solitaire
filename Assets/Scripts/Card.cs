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
public enum Values
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
    public Values Value { get; private set; }
    public Suits Suit { get; private set; }
    public Card Child { get; private set; }
    public Card Parent { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Collider2D CardCollider { get; private set; }
    #endregion

    #region Private Fields
    private int faceUpSpriteIndex = 0;
    #endregion

    #region Public Fields
    [SerializeField]
    private Sprite[] cardSprites;
    public Desk desk;
    #endregion

    #region Public Methods
    public void ChangeCardAndSuit(Values card, Suits suit)
    {
        this.Value = card;
        this.Suit = suit;

        ChangeFaceUpSprite(card, suit);
    }
    public void TurnFaceUp()
    {
        SpriteRenderer.sprite = cardSprites[faceUpSpriteIndex];
        CardCollider.enabled = true;
    }
    public void TurnFaceDown()
    {
        SpriteRenderer.sprite = cardSprites[0];
        CardCollider.enabled = false;
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
        int cardIndex = Convert.ToInt32(this.Value);
        int nextCardIndex = Convert.ToInt32(card.Value);
        int indexDif = cardIndex - nextCardIndex;
        int lastEnumIndex = Enum.GetNames(typeof(Values)).Length - 1;

        return (indexDif == 1 || indexDif == -1 || indexDif == lastEnumIndex || indexDif == -lastEnumIndex);
    }
    public int GetCardIndex()
    {
        return Convert.ToInt32(Value);
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CardCollider = GetComponent<Collider2D>();
    }
    private void ChangeFaceUpSprite(Values card, Suits suit)
    {
        // Карты в массиве cardSprites расположены по порядку так что зная масть, мы можем сместить начальную позицию в массиве что бы достать верный спрайт
        faceUpSpriteIndex = 1;
        int cardsOfSameSuit = Enum.GetNames(typeof(Values)).Length;
        int suitIndex = Convert.ToInt32(suit);
        int cardIndex = Convert.ToInt32(card);

        faceUpSpriteIndex += cardsOfSameSuit * suitIndex;
        faceUpSpriteIndex += cardIndex;

        if (faceUpSpriteIndex < cardSprites.Length)
        {
            SpriteRenderer.sprite = cardSprites[faceUpSpriteIndex];
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
}
