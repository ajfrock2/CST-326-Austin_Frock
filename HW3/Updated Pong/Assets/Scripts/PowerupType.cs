using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

public class PowerupType : MonoBehaviour
{
    
    private int index;
    
    public void SetIndex(int index)
    {
        this.index = index;
    }

    public void OnTriggerEnter(Collider other)
    {
        
        Abilities ability = null;
        
        //Finds which player got a powerup
        if (other.gameObject.tag == "Left Paddle")
        {
            Debug.Log("Left Paddle got powerup" + this.index);
            Destroy(this.gameObject);
            
            GameObject leftPlayer = GameObject.FindGameObjectWithTag("Left Player");
            ability = leftPlayer.GetComponent<Abilities>();
        }
        if (other.gameObject.tag == "Right Paddle")
        {
            Destroy(this.gameObject);

            GameObject rightPlayer = GameObject.FindGameObjectWithTag("Right Player");
            ability = rightPlayer.GetComponent<Abilities>();
        }

        //Assigns player objects powerups
        if (ability != null)
        {
            switch (index)
            {
                case 0:
                    ability.SetHasJump();
                    break;
                case 1:
                    ability.SetHasDuplicate();
                    break;
                default:
                    break;
            }
            
        }
    }
    
}
