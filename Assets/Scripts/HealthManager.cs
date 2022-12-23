using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    [SerializeField] private int fullHealth;
    public int Health { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Health = fullHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Damage(int damage)
    {
        Health -= damage;
    }

    void Die()
    {
        GameEvents.Lose();
    }
}
