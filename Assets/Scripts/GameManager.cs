using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    #region Public Fields
    public static GameManager instance = null;
    #endregion

    #region Private Fields
    private UIManager uIManager;
    private BoardHolder boardHolder;
    private bool isGameStarted;
    #endregion

    #region Public Methods
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

        boardHolder.Sequence.onCardAdd.AddListener(CheckIfLoseGame);

        if (boardHolder.Bank.TryFindCardOnTop(out Card card))
        {
            boardHolder.Sequence.MoveCardOnTop(card);
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
        if (isGameStarted && (CanGoToSequence(card) || card.desk == boardHolder.Bank))
        {
            boardHolder.Sequence.MoveCardOnTop(card);
        }
    }
    public bool CanGoToSequence(Card card)
    {
        if (card.desk != boardHolder.Sequence)
        {
            if (boardHolder.Sequence.TryFindCardOnTop(out Card topCard))
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
        if (boardHolder.Bank.Cards.Count < 1 && boardHolder.Sequence.TryFindCardOnTop(out Card sequenceCard))
        {
            Debug.Log("Проверка на проигрыш!");
            Debug.Log(sequenceCard.Value);
            foreach (Desk desk in boardHolder.Desks)
            {
                if (desk.TryFindCardOnTop(out Card deskCard))
                {
                    Debug.Log(deskCard.Value);
                }
            }

            if (boardHolder.Desks.All(desk => !desk.TryFindCardOnTop(out Card deskCard) || !sequenceCard.NextToCard(deskCard)))
            {
                Debug.Log("Проверка на проигрыш!");
                uIManager.winLoseText.text = "Вы проиграли";
                uIManager.winLoseImage.gameObject.SetActive(true);
            }
        }
    }
    #endregion

    #region Private Methods
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
    #endregion
}
