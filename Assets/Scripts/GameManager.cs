using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private UIManager uIManager;
    private BoardHolder boardHolder;
    private bool isGameStarted;

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
        InitGame();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitGame();
        StartGame();
    }

    public void InitGame()
    {
        boardHolder = Instantiate(GameAssets.instance.boardHolder).GetComponent<BoardHolder>();
        uIManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
    }
    public void StartGame()
    {
        isGameStarted = true;
        uIManager.startButton.gameObject.SetActive(false);
        uIManager.restartButton.gameObject.SetActive(true);

        boardHolder.sequence.onCardAdd.AddListener(CheckIfLoseGame);

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
        if(CanGoToSequence(card) || card.desk == boardHolder.bank || isGameStarted)
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
        uIManager.winLoseImage.gameObject.SetActive(true);
    }
    public void CheckIfLoseGame()
    {
        if (boardHolder.bank.Cards.Count < 1 && boardHolder.sequence.TryFindCardOnTop(out Card sequenceCard))
        {
            Debug.Log("Проверка на проигрыш!");
            Debug.Log(sequenceCard.Value);
            foreach (Desk desk in boardHolder.desks)
            {
                if(desk.TryFindCardOnTop(out Card deskCard))
                {
                    Debug.Log(deskCard.Value);
                }
            }

            if (boardHolder.desks.All(desk => !desk.TryFindCardOnTop(out Card deskCard) || !sequenceCard.NextToCard(deskCard)))
            {
                Debug.Log("Проверка на проигрыш!");
                uIManager.winLoseText.text = "Вы проиграли";
                uIManager.winLoseImage.gameObject.SetActive(true);
            }
        }
    }
}
