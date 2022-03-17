using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Desk : MonoBehaviour
{
    #region Private Fields
    public Vector3 offset;
    protected List<Card> cards;
    #endregion

    #region Properties
    private Vector3 NewCardPosition
    {
        get
        {
            return gameObject.transform.position + offset * cards.Count;
        }
    }
    #endregion

    #region Private Methods
    private void Start()
    {
        cards = new List<Card>();
    }
    private void AddCardOnTop(Card card)
    {
        if(card.desk == this)
        {
            return;
        }

        var cardCom = card.GetComponent<Card>();

        //Делает карту дочерним элементом
        card.transform.parent = gameObject.transform;

        //Добавляет карту в колоду
        if(!(card.desk is null))
        {
            card.desk.RemoveCard(card);
        }
        if (TryFindCardOnTop(out Card parent))
        {
            card.SetParent(parent);
            parent.TurnFaceDown();
        }
        cards.Add(card);
        card.desk = this;

        //Определяет порядок отрисовки
        cardCom.spriteRenderer.sortingOrder = cards.Count;
    }
    private void RemoveCard(Card card)
    {
        card.desk = null;
        if (!(card.Parent is null))
        {
            card.Parent.TurnFaceUp();
        }
        card.SetParent(null);
        cards.Remove(card);
    }
    #endregion

    #region Public Methods
    public void MoveCardOnTop(Card card)
    {
        if (card.desk == this)
        {
            return;
        }

        card.transform.DOMove(NewCardPosition, 0.5f);
        AddCardOnTop(card);
    }
    public void GenerateCard(Cards card)
    {
        Suits suit = (Suits)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Suits)).Length);

        var newCard = Instantiate(GameAssets.instance.card, NewCardPosition, Quaternion.identity).GetComponent<Card>();

        AddCardOnTop(newCard);
        newCard.ChangeCardAndSuit(card, suit);
    }
    public bool TryFindCardOnTop(out Card card)
    {
        card = null;
        if (cards.Count > 0)
        {
            card = cards[cards.Count - 1];
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
