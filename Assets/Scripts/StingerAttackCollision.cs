using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StingerAttackCollision : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.name);
        if (player.IsAttacking && collider.tag == "Slime")
        {
            SlimeController slime = collider.gameObject.GetComponent<SlimeController>();
            slime.Destroy();
        }
    }

}
