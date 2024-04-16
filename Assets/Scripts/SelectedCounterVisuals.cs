using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// the approach is that the player fire off an event whenever selectedcounter chnages and all the counter visuals will listen to it
// if any of the counter gets the reference to the event it will change it's visual 

public class SelectedCounterVisuals : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedArgs e)
    {
        // comparing which counter this belongs to
        if (e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);

        }
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {

            visualGameObject.SetActive(false);
        }
    }
}
