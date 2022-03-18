using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Linq;

public class BoardHolder : MonoBehaviour
{
    #region Private Fields
    private readonly int minSequence = 2;
    private readonly int maxSequence = 7;
    private int unresolvedCardsOnBoard;
    private readonly int maxCardsOnBoard = 40;
    #endregion

    #region Properties
    public List<Desk> Desks { get; private set; }
    public Desk Bank { get; private set; }
    public Desk Sequence { get; private set; }
    #endregion

    #region Public Methods
    public void GenerateBoard()
    {
        unresolvedCardsOnBoard = maxCardsOnBoard;
        List<Values> cardsToBank = new List<Values>();
        List<Values> cardsToBoard = new List<Values>();

        while (unresolvedCardsOnBoard > 0)
        {
            List<Values> newCards = GenerateOneSequence();

            string sequenceString = "";
            foreach (Values card in newCards)
            {
                sequenceString += $"{card}, ";
            }
            Debug.Log(sequenceString);

            int lastCardIndex = newCards.Count - 1;
            cardsToBank.Add(newCards[lastCardIndex]);
            newCards.RemoveAt(lastCardIndex);
            cardsToBoard.AddRange(newCards);
        }

        foreach (Values card in cardsToBank)
        {
            var deskCom = Bank.GetComponent<Desk>();
            deskCom.GenerateCard(card);
        }

        foreach (Values card in cardsToBoard)
        {
            int deskIndex = UnityEngine.Random.Range(0, Desks.Count);
            var deskCom = Desks[deskIndex];
            deskCom.GenerateCard(card);
        }
    }
    private void OnDeskEmpty()
    {
        if (!Desks.Any(desk => desk.Cards.Count > 0))
        {
            Debug.Log("Все колоды опустели");
            GameManager.instance.WinGame();
        }
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        Desks = new List<Desk>();

        foreach (GameObject deskObj in GameObject.FindGameObjectsWithTag("Desk"))
        {
            var deskCom = deskObj.GetComponent<Desk>();
            Desks.Add(deskCom);
            deskCom.onDeskEmpty.AddListener(OnDeskEmpty);
        }
        Bank = GameObject.FindGameObjectWithTag("Bank").GetComponent<Desk>();
        Sequence = GameObject.FindGameObjectWithTag("Sequence").GetComponent<Desk>();

        GenerateBoard();
    }
    private List<Values> GenerateOneSequence()
    {
        int sequenceNumber = UnityEngine.Random.Range(minSequence, maxSequence + 1);
        int lastEnumIndex = Enum.GetNames(typeof(Values)).Length - 1;
        bool sequenceDown = (UnityEngine.Random.value <= 0.35f);
        bool sequenceTurn = (UnityEngine.Random.value <= 0.15f);

        if (unresolvedCardsOnBoard < minSequence)
        {
            sequenceNumber += unresolvedCardsOnBoard;
        }
        List<Values> cards = new List<Values>();

        Values firstCard = (Values)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Values)).Length);
        cards.Add(firstCard);
        int prevCardIndex = Convert.ToInt32(firstCard);

        for (int i = 0; i < sequenceNumber; i++)
        {
            if (sequenceTurn && sequenceNumber / 2 <= i)
            {
                sequenceDown = !sequenceDown;
                sequenceTurn = !sequenceTurn;
            }

            int nextCardIndex;
            if (sequenceDown)
            {
                nextCardIndex = prevCardIndex + 1;
            }
            else
            {
                nextCardIndex = prevCardIndex - 1;
            }

            if(nextCardIndex > lastEnumIndex)
            {
                nextCardIndex = 0;
            }
            else if (nextCardIndex < 0)
            {
                nextCardIndex = lastEnumIndex;
            }
            cards.Add((Values)nextCardIndex);

            prevCardIndex = nextCardIndex;
        }

        unresolvedCardsOnBoard -= sequenceNumber;

        return cards;
    }
    #endregion
}
