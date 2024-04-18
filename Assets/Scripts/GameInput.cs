using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PERFS_BINDINGS = "InputBindings";
    public static GameInput Instance { get; private set; }
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;
    private PlayerInputActions playerInputActions;

    // making a method to get binding irrespective of the input system.
    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
        GamePad_Interact,
        GamePad_InteractAlternate,
    }

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PERFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PERFS_BINDINGS));
        }

        playerInputActions.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.AlternateInteraction.performed += AlternateInteract_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
        // Debug.Log(GetBindingText(Binding.Interact));
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

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputActions.Player.AlternateInteraction.bindings[0].ToDisplayString();
            case Binding.GamePad_Interact:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.GamePad_InteractAlternate:
                return playerInputActions.Player.AlternateInteraction.bindings[1].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
            //incase of move this is different as moves is a composite binding(has 4 values inside the move action), but in terms of storage everyting is inside a flat array.
            //so in this case [0] gives 2D vector, [1] gives w, and so on in the order
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        // to rebind a key first we disable the action map, then we get the action, use PerformInteractiveRebinding and use a callback, enable the action map and then use .Start() to start the rebinding.
        playerInputActions.Disable();
        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputActions.Player.AlternateInteraction;
                bindingIndex = 0;
                break;
            case Binding.GamePad_Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.GamePad_InteractAlternate:
                inputAction = playerInputActions.Player.AlternateInteraction;
                bindingIndex = 1;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;

        }


        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
        {
            // Debug.Log(callback.action.bindings[1].path);
            // Debug.Log(callback.action.bindings[1].overridePath);
            callback.Dispose();
            playerInputActions.Enable();
            onActionRebound();
            PlayerPrefs.SetString(PLAYER_PERFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
            OnBindingRebind?.Invoke(this, EventArgs.Empty);
        }).Start();

        // playerInputActions.SaveBindingOverridesAsJson();
    }
}
