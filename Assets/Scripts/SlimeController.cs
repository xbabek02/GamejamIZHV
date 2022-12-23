using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class SlimeController : MonoBehaviour, IDestroyable
{
    [SerializeField] private GameObject Cake;
    private NavMeshAgent nav;
    [SerializeField] private SlimeStates state;
    [SerializeField] private GameObject slimePrefab;
    private Animator animator;
    private HealthManager cakeHealth;

    private float replicationMidTime = 50f;
    private float replicationDelta = 10f;
    private bool canReplicate = true;

    private bool CanAttack = true;
    private float attackDelay = 5f;

    private enum SlimeStates
    {
        Spawning,
        Walking,
        Attacking
    };

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.IncrementEnemies();
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Cake = GameObject.FindGameObjectWithTag("Cake");
        state = SlimeStates.Spawning;
        StartCoroutine(WalkToCake());
        cakeHealth = Cake.GetComponent<HealthManager>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Cake")
        {
            nav.isStopped = true;
            state = SlimeStates.Attacking;
            animator.SetBool("Moving", false);
        }
    }
       
    private IEnumerator WalkToCake()
    {
        yield return new WaitForSeconds(5);
        
        animator.SetBool("Moving", true);
        state = SlimeStates.Walking;
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        CanAttack = true;
    }

    IEnumerator Replicate()
    {
        canReplicate = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(replicationMidTime-replicationDelta, replicationMidTime+replicationDelta));

        //GameObject newSlime = Instantiate(slimePrefab);
        //newSlime.transform.position = transform.position;
        GameObject newSlime = Instantiate(slimePrefab, transform.position, transform.rotation);

        animator.SetTrigger("Replicate");
        canReplicate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canReplicate)
        {
            StartCoroutine(Replicate());
        }
        if (state == SlimeStates.Walking)
        {
            nav.SetDestination(Cake.transform.position);
        }
        else if (state == SlimeStates.Attacking && CanAttack)
        {
            CanAttack = false;
            cakeHealth.Damage(5);
            animator.SetTrigger("Attack");
            StartCoroutine(DelayAttack());
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
        GameEvents.DecrementEnemies();
    }
}
