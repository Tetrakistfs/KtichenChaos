using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    // we will invoke the event whenever a state changes
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;


    private State state;

    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        if (HasKitchenObject())
        {

            switch (state)
            {
                case State.Idle:
                    {
                        break;
                    }
                case State.Frying:
                    {
                        fryingTimer += Time.deltaTime;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
                        });
                        if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                        {
                            // the object is fried
                            GetKitchenObject().DestroySelf();
                            // spawn the fried object
                            KitchenObject.kitchenObjectSpawn(fryingRecipeSO.output, this);
                            Debug.Log("Object Fried");
                            burningTimer = 0f;
                            state = State.Fried;

                            burningRecipeSO = GetBurningRecepieWithInput(GetKitchenObject().GetKitchenObjectSO());

                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                            {
                                state = state
                            });

                        }
                        break;
                    }
                case State.Fried:
                    {
                        burningTimer += Time.deltaTime;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = (float)burningTimer / burningRecipeSO.burningTimerMax
                        });
                        if (burningTimer > burningRecipeSO.burningTimerMax)
                        {
                            // the object is fried
                            GetKitchenObject().DestroySelf();
                            // spawn the fried object
                            KitchenObject.kitchenObjectSpawn(burningRecipeSO.output, this);
                            Debug.Log("Object Burned");
                            state = State.Burned;

                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                            {
                                state = state
                            });

                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                            {
                                progressNormalized = 0f
                            });

                        }
                        break;
                    }
                case State.Burned:
                    {
                        break;
                    }
            }
            // Debug.Log(state);
        }


    }
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //There is no kitchen object here
            if (player.HasKitchenObject())
            {
                // Player has a kitchenObject that he will drop on to the counter
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // player is carrying something that can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecepieWithInput(GetKitchenObject().GetKitchenObjectSO());
                    fryingTimer = 0f;
                    state = State.Frying;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
            }
            else
            {
                // Player is not holding any kitchenObject
                Debug.Log("Player is Not holding any kitchenObject");
            }
        }
        else
        {
            // There is a kitchen object here
            if (player.HasKitchenObject())
            {
                // Player is already carrying something
                Debug.Log("Counter already has a kitchenObject on it");
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }

                    state = State.Idle;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = 0f
                    });
                }
            }
            else
            {
                // Player is not carrying anything so the counter will give the kitchenObject to the player
                GetKitchenObject().SetKitchenObjectParent(player);
                //reset the counter state to idle
                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }

        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecepieWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecepieWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
            return fryingRecipeSO.output;
        else
            return null;
    }

    private FryingRecipeSO GetFryingRecepieWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
                return fryingRecipeSO;
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecepieWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
                return burningRecipeSO;
        }
        return null;
    }

}
