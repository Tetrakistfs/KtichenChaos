using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpwan;
    public event EventHandler OnRecipeComplete;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 2f;
    private int waitingRecipeMax = 4;
    private int successfulDeliveryAmount = 0;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (GameHandler.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipeMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                // Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpwan?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // has the same number of ingredients
                bool plateContentMathcesRecipe = true;
                // we need to cycle through each ingredient in the recipe to match it
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    // we need to cycle through each ingredient in the plate to match it
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            // ingredients matches
                            ingredientFound = true;
                            break;

                        }
                    }
                    if (!ingredientFound)
                    {
                        // this recipe ingredient was not found on the plate
                        plateContentMathcesRecipe = false;
                    }
                }

                if (plateContentMathcesRecipe)
                {
                    // player delivered the correct recipe
                    // Debug.Log("Player Delivered the correct recipe");
                    successfulDeliveryAmount++;
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeComplete?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        // no mathces found
        // player did not delivered the correct recipe
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        Debug.Log("player did not delivered the correct recipe");
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulDeliveryAmount()
    {
        return successfulDeliveryAmount;
    }
}
