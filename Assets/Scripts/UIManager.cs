using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button restartButton;
    public Button startButton;
    public Image winLoseImage;
    public Text winLoseText;

    private void Start()
    {
        restartButton.onClick.AddListener(GameManager.instance.RestartGame);
        startButton.onClick.AddListener(GameManager.instance.StartGame);
    }
}
