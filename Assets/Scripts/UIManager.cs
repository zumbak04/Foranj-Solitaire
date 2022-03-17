using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button restartButton;
    public Image winImage;

    private void Start()
    {
        restartButton.onClick.AddListener(GameManager.instance.RestartGame);
    }
}
