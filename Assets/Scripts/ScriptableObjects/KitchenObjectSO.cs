using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class KitchenObjectSO : ScriptableObject
{
    // this is a scriptable object file which will spwan multiple kitchen objects 
    public Transform prefab;
    public Sprite sprite;
    public string objectName;
}
