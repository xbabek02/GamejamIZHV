using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonCollision : MonoBehaviour
{
    public UnityEvent pressButtonEvent;
    public PlayerMovement player;
    bool deactivated = false;
    
    private void OnTriggerStay(Collider other)
    {
        if (!player.inTheAir && !deactivated)
        {
            pressButtonEvent.Invoke();
            deactivated = true;
        }

    }
}
