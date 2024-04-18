using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyMoveGamepadText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyInteractGamepadText;
    [SerializeField] private TextMeshProUGUI keyAltInteractText;
    [SerializeField] private TextMeshProUGUI keyAltInteractGamepadText;
    [SerializeField] private TextMeshProUGUI keyEscapeText;
    [SerializeField] private TextMeshProUGUI keyEscapeGamepadText;


    private void Start()
    {
        GameInput.Instance.OnBindingRebind += Instance_OnBindingRebind;
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;
        UpdateVisual();
        Show();
    }

    private void GameHandler_OnStateChanged(object sender, EventArgs e)
    {
        if (GameHandler.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void Instance_OnBindingRebind(object sender, EventArgs e)
    {
        UpdateVisual();
    }
    private void UpdateVisual()
    {
        keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        keyInteractGamepadText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Interact);
        keyAltInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        keyAltInteractGamepadText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_InteractAlternate);
        keyEscapeText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
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
