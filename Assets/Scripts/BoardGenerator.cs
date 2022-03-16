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

    public List<GameObject> deskLocators;

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
        deskLocators.AddRange(GameObject.FindGameObjectsWithTag("Desk"));

        GenerateBoard();
    }

    private void GenerateBoard()
    {
        unresolvedCardsOnBoard = maxCardsOnBoard;
        cardOrderInLayer = 0;
        List<Cards> bank = new List<Cards>();
        List<Cards> board = new List<Cards>();

        while (unresolvedCardsOnBoard > 0)
        {
            List<Cards> newCards = GenerateOneSequence();
            bank.Add(newCards[0]);
            newCards.RemoveAt(0);
            board.AddRange(newCards);
        }

        foreach (Cards card in board)
        {
            int deskIndex = UnityEngine.Random.Range(0, deskLocators.Count);
            var deskCom = deskLocators[deskIndex].GetComponent<Desk>();
            deskCom.GenerateCard(card, cardOrderInLayer);

            cardOrderInLayer++;
        }
    }
    private List<Cards> GenerateOneSequence()
    {
        int sequence = UnityEngine.Random.Range(minSequence, maxSequence + 1);
        if(unresolvedCardsOnBoard < minSequence)
        {
            sequence += unresolvedCardsOnBoard;
        }
        List<Cards> cards = new List<Cards>();

        for(int i = 0; i < sequence; i++)
        {
            cards.Add((Cards)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Cards)).Length));
        }

        unresolvedCardsOnBoard -= sequence;

        return cards;
    }
    private void GenerateCard(Cards card)
    {

    }
}
