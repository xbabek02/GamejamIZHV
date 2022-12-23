using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] private bool active = true;
    [SerializeField] private Material OffColor;
    [SerializeField] private Material OnColor;

    [SerializeField] private SkinnedMeshRenderer ButtonRenderer;

    private Animator animator;
    private bool clicked;

    private float spawnIntervalMin = 35.0f;
    private float spawnIntervalMax = 120.0f;
    private bool canSpawn = true;

    [SerializeField] private GameObject slimePrefab;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        Material[] mats = ButtonRenderer.materials;
        mats[1] = OnColor;

        ButtonRenderer.materials = mats;

        GameEvents.IncrementTeleporters();
    }

    public void Switch()
    {
        if (!active || clicked)
        {
            return;
        }

        StartCoroutine(Click());
    }

    private IEnumerator Click()
    {
        animator.SetTrigger("Pressed");
        
        yield return new WaitForSeconds(1);

        clicked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (clicked && active)
        {
            Debug.Log("vypnout");
            Material[] mats = ButtonRenderer.materials;
            mats[1] = OffColor;
            ButtonRenderer.materials = mats;
            active = !active;

            clicked = false;
            GameEvents.DecrementTeleporters();
        }
        
        //spawning
        if (canSpawn && active)
        {
            canSpawn = false;
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(spawnIntervalMin, spawnIntervalMax));
        if (active)
        {
            canSpawn = true;
            GameObject newSlime = Instantiate(slimePrefab, transform, false);
        }


    }
}
