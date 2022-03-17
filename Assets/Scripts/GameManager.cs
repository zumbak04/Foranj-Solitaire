using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private Desk sequence;
    private Desk bank;
    private BoardHolder boardHolder;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartGame();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartGame();
    }

    public void StartGame()
    {
        Instantiate(GameAssets.instance.boardHolder);

        sequence = GameObject.FindGameObjectWithTag("Sequence").GetComponent<Desk>();
        bank = GameObject.FindGameObjectWithTag("Bank").GetComponent<Desk>();

        if (bank.TryFindCardOnTop(out Card card))
        {
            sequence.MoveCardOnTop(card);
        }
        else
        {
            Debug.LogError("Банк пуст!");
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
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
                Debug.LogWarning("Верхний карты нет");
            }
        }
        return false;
    }
}
