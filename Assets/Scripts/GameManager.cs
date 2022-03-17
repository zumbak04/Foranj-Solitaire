using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private Desk sequence;
    private Desk bank;

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
        sequence = BoardGenerator.instance.sequence;
        bank = BoardGenerator.instance.bank;

        InitGame();
    }

    public void InitGame()
    {
        if(bank.TryFindCardOnTop(out Card card))
        {
            sequence.MoveCardOnTop(card);
        }
    }
    public void ClickOnCard(Card card)
    {
        if(CanGoToSequence(card) || card.desk == bank)
        {
            sequence.MoveCardOnTop(card);
        }
    }
    public bool CanGoToSequence(Card card)
    {
        if (card.desk != sequence)
        {
            if (sequence.TryFindCardOnTop(out Card topCard))
            {
                return topCard.NextToCard(card);
            }
            else
            {
                Debug.LogWarning("������� ����� ���");
            }
        }
        return false;
    }
}
