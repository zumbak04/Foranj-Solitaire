using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private UIManager uIManager;
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
        boardHolder = Instantiate(GameAssets.instance.boardHolder).GetComponent<BoardHolder>();
        boardHolder.onBoardEmpty.AddListener(WinGame);

        uIManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        uIManager.winImage.gameObject.SetActive(false);

        if (boardHolder.bank.TryFindCardOnTop(out Card card))
        {
            boardHolder.sequence.MoveCardOnTop(card);
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
        if(CanGoToSequence(card) || card.desk == boardHolder.bank)
        {
            boardHolder.sequence.MoveCardOnTop(card);
        }
    }
    public bool CanGoToSequence(Card card)
    {
        if (card.desk != boardHolder.sequence)
        {
            if (boardHolder.sequence.TryFindCardOnTop(out Card topCard))
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
    public void WinGame()
    {
        uIManager.winImage.gameObject.SetActive(true);
    }
}
