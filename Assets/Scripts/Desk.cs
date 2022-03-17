using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.Events;

public class Desk : MonoBehaviour
{
    #region Private Fields
    public Vector3 offset;
    #endregion

    #region Properties
    private Vector3 NewCardPosition
    {
        get
        {
            return gameObject.transform.position + offset * Cards.Count;
        }
    }
    public List<Card> Cards { get; private set; }
    #endregion

    #region Events
    public UnityEvent onDeskEmpty;
    public UnityEvent onCardAdd;
    #endregion

    #region Private Methods
    private void Awake()
    {
        Cards = new List<Card>();
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
        Cards.Add(card);
        card.desk = this;

        //Вызывает эвент после того как карта добалена в новую колоду
        onCardAdd.Invoke();

        //Определяет порядок отрисовки
        cardCom.spriteRenderer.sortingOrder = Cards.Count;
    }
    private void RemoveCard(Card card)
    {
        card.desk = null;
        if (!(card.Parent is null))
        {
            card.Parent.TurnFaceUp();
        }
        card.SetParent(null);
        Cards.Remove(card);

        if(Cards.Count < 1)
        {
            Debug.Log("Колода опустела");
            onDeskEmpty.Invoke();
        }
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
    public void GenerateCard(Values card)
    {
        Suits suit = (Suits)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Suits)).Length);

        var newCard = Instantiate(GameAssets.instance.card, NewCardPosition, Quaternion.identity).GetComponent<Card>();

        AddCardOnTop(newCard);
        newCard.ChangeCardAndSuit(card, suit);
    }
    public bool TryFindCardOnTop(out Card card)
    {
        card = null;
        if (Cards.Count > 0)
        {
            card = Cards[Cards.Count - 1];
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
