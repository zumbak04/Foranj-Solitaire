using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardGenerator : MonoBehaviour
{
    public static BoardGenerator instance = null;

    private int minSequence = 2;
    private int maxSequence = 7;

    private int unresolvedCardsOnBoard;
    private int maxCardsOnBoard = 40;

    private int minCardsInBank;
    private int maxCardsInBank;

    public List<Desk> desks;
    public Desk bank;
    public Desk sequence;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        minCardsInBank = Mathf.CeilToInt((float)maxCardsOnBoard / maxSequence);
        maxCardsInBank = Mathf.CeilToInt((float)maxCardsOnBoard / minSequence);
        desks = new List<Desk>();
        foreach(GameObject deskObj in GameObject.FindGameObjectsWithTag("Desk"))
        {
            desks.Add(deskObj.GetComponent<Desk>());
        }
        bank = GameObject.FindGameObjectWithTag("Bank").GetComponent<Desk>();
        sequence = GameObject.FindGameObjectWithTag("Sequence").GetComponent<Desk>();

        GenerateBoard();
    }

    private void GenerateBoard()
    {
        unresolvedCardsOnBoard = maxCardsOnBoard;
        List<Cards> cardsToBank = new List<Cards>();
        List<Cards> cardsToBoard = new List<Cards>();

        while (unresolvedCardsOnBoard > 0)
        {
            List<Cards> newCards = GenerateOneSequence();

            string sequenceString = "";
            foreach (Cards card in newCards)
            {
                sequenceString += $"{card}, ";
            }
            Debug.Log(sequenceString);

            int lastCardIndex = newCards.Count - 1;
            cardsToBank.Add(newCards[lastCardIndex]);
            newCards.RemoveAt(lastCardIndex);
            cardsToBoard.AddRange(newCards);
        }

        foreach (Cards card in cardsToBank)
        {
            var deskCom = bank.GetComponent<Desk>();
            deskCom.GenerateCard(card);
        }

        foreach (Cards card in cardsToBoard)
        {
            int deskIndex = UnityEngine.Random.Range(0, desks.Count);
            var deskCom = desks[deskIndex];
            deskCom.GenerateCard(card);
        }
    }
    private List<Cards> GenerateOneSequence()
    {
        int sequenceNumber = UnityEngine.Random.Range(minSequence, maxSequence + 1);
        int lastCardEnumIndex = Enum.GetNames(typeof(Cards)).Length - 1;
        bool sequenceDown = (UnityEngine.Random.value <= 0.35f);
        bool sequenceTurn = (UnityEngine.Random.value <= 0.15f);

        if (unresolvedCardsOnBoard < minSequence)
        {
            sequenceNumber += unresolvedCardsOnBoard;
        }
        List<Cards> cards = new List<Cards>();

        Cards firstCard = (Cards)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Cards)).Length);
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

            if(nextCardIndex > lastCardEnumIndex)
            {
                nextCardIndex -= lastCardEnumIndex + 1;
            }
            else if (nextCardIndex < 0)
            {
                nextCardIndex += lastCardEnumIndex + 1;
            }
            cards.Add((Cards)nextCardIndex);

            prevCardIndex = nextCardIndex;
        }

        unresolvedCardsOnBoard -= sequenceNumber;

        return cards;
    }
}
