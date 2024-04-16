using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //There is no kitchen object here
            if (player.HasKitchenObject())
            {
                // Player has a kitchenObject that he will drop on to the counter
                player.GetKitchenObject().SetKitchenObjectParent(this);
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
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // player is not carrying a plate but something else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // counter is holding a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
                // Debug.Log("Counter already has a kitchenObject on it");
            }
            else
            {
                // Player is not carrying anything so the counter will give the kitchenObject to the player
                GetKitchenObject().SetKitchenObjectParent(player);
            }

        }
    }


}
// if there is something on top of clearcounter the SO will not spawn
// if (!HasKitchenObject())
// {
//     Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
//     kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
// }
// else
// {
//     // give the object to the player
//     kitchenObject.SetKitchenObjectParent(player);
//     // Debug.Log(kitchenObject.GetClearCounter());
// }
// Debug.Log("Interact");
// Debug.Log(kitchenObjectTransform.GetComponent<KitchenObject>().GetKitchenObjectSO().objectName);
