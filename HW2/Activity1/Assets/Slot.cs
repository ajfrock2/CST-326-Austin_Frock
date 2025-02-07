using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    public int slotNumber;
    

    void OnTriggerEnter(Collider other)
    {
           Debug.Log($"{other.name} entered slot {slotNumber}. You won 5.00$"); 
    }
}
