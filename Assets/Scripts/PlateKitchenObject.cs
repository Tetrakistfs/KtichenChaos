using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientsAddedEventArgs> OnIngredientsAdded;
    public class OnIngredientsAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    }
    [SerializeField] List<KitchenObjectSO> validKitchenObjectList;
    private List<KitchenObjectSO> kitchenObjectSOList;
    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectList.Contains(kitchenObjectSO))
        {
            // not a valid ingredient to put on a plate
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //already has the ingredient
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientsAdded?.Invoke(this, new OnIngredientsAddedEventArgs
            {
                KitchenObjectSO = kitchenObjectSO
            });
            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
