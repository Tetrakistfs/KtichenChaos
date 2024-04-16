using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    private PlayerInputActions playerInputActions;


    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.AlternateInteraction.performed += AlternateInteract_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        //this is to fix a issue where if you go to the main menu and then play the game again it will throw a reference error
        // this is because the objects gets destroyed on main menu screen load and then the game tries to access the objects by their reference buy they have been destroyed.
        // this is solved by unsuscribing to the actions and then disposing them to save memory.

        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.AlternateInteraction.performed -= AlternateInteract_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void AlternateInteract_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
        // this is same as
        // if(OnInteractAction != null) {
        //     OnInteractAction(this, EventArgs.Empty);
        // }
    }


    public Vector2 GetMovementVectorNoramlized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
