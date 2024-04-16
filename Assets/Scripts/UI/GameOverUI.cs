using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI recipesDeliveredText;

    private void Start()
    {
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;
        Hide();
    }

    private void GameHandler_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameHandler.Instance.IsGameOver())
        {
            Show();
            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulDeliveryAmount().ToString();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
