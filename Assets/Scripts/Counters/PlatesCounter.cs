using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// at the time of spawn only a visual will be spawned but when the player interacts with the object it will spwan a kitchenObject
// this is due to the kitchenObject set to only one kitchen parent i.e. one counter can spawn one object at a time not multiple.

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 2f;
    private int plateSpawnAmount;
    private int plateSpawnAmountMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;
            if (GameHandler.Instance.IsGamePlaying() && plateSpawnAmount < plateSpawnAmountMax)
            {
                plateSpawnAmount++;
                // KitchenObject.kitchenObjectSpawn(plateKitchenObjectSO, this);
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }


    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // player is empty handed
            if (plateSpawnAmount > 0)
            {
                // plate counter has atleat 1 plate to give
                plateSpawnAmount--;
                KitchenObject.kitchenObjectSpawn(plateKitchenObjectSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
