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
    private int cardOrderInLayer;

    private int minCardsInBank;
    private int maxCardsInBank;

    public List<GameObject> desks;
    public GameObject bank;
    public GameObject sequence;

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
        desks = new List<GameObject>();
        desks.AddRange(GameObject.FindGameObjectsWithTag("Desk"));
        bank = GameObject.FindGameObjectWithTag("Bank");
        sequence = GameObject.FindGameObjectWithTag("Sequence");

        GenerateBoard();
    }

    private void GenerateBoard()
    {
        unresolvedCardsOnBoard = maxCardsOnBoard;
        cardOrderInLayer = 0;
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
            deskCom.GenerateCard(card, cardOrderInLayer);

            cardOrderInLayer++;
        }

        foreach (Cards card in cardsToBoard)
        {
            int deskIndex = UnityEngine.Random.Range(0, desks.Count);
            var deskCom = desks[deskIndex].GetComponent<Desk>();
            deskCom.GenerateCard(card, cardOrderInLayer);

            cardOrderInLayer++;
        }
    }
    private List<Cards> GenerateOneSequence()
    {
        int sequenceNumber = UnityEngine.Random.Range(minSequence, maxSequence + 1);
        int lastCardEnumIndex = Enum.GetNames(typeof(Cards)).Length - 1;
        //bool sequenceDown = (UnityEngine.Random.Range(0, 2) == 1);
        //bool sequenceTurn = (UnityEngine.Random.Range(0, 2) == 1);

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
            int nextCardIndex = prevCardIndex - 1;

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
