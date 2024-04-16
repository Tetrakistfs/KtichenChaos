using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start()
    {
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;
        Show();
    }

    private void GameHandler_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameHandler.Instance.IsCountdownToStartActive())
            Show();
        else
            Hide();
    }

    private void Update()
    {
        countdownText.text = Mathf.Ceil(GameHandler.Instance.GetCountdownToStartTimer()).ToString();
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



