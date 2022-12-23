using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    [SerializeField] private static int teleportersCount;
    [SerializeField] private static int enemiesCount;
    
    [SerializeField] private TextMeshProUGUI teleportersText;
    [SerializeField] private TextMeshProUGUI cakeHealthText;
    [SerializeField] private GameObject winscene;
    [SerializeField] private GameObject loserscene;
    [SerializeField] private GameObject pausedscene;
    private bool pauseShowed = false;

    [SerializeField] HealthManager cakeHealth;

    public static GameEvents instance;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (teleportersCount > 0)
        {
            teleportersText.text = $"Teleporters left: {teleportersCount}";
        }
        else
        {
            teleportersText.text = $"Enemies left: {enemiesCount}";
        }

        cakeHealthText.text = $"Cake Health: {cakeHealth.Health}";

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchPauseScreen();
        }
    }

    public void SwitchPauseScreen()
    {
        if (pauseShowed)
        {
            Cursor.visible = false;
            pausedscene.SetActive(false);
        }
        else
        {
            Cursor.visible = true;
            pausedscene.SetActive(true);
        }
        pauseShowed = !pauseShowed;
    }

    public static void DecrementTeleporters()
    {
        teleportersCount--;
        if ((teleportersCount == 0) && (enemiesCount == 0))
        {
            Win();
        }
    }

    public static void IncrementTeleporters()
    {
        teleportersCount++;
    }

    public static void IncrementEnemies()
    {
        enemiesCount++;
    }

    public static void DecrementEnemies()
    {
        enemiesCount--;
        if ((teleportersCount == 0) && (enemiesCount == 0))
        {
            Win();
        }
    }

    public static void Win()
    {
        Cursor.visible = true;
        instance.winscene.SetActive(true);
    }

    public static void Lose()
    {
        Cursor.visible = true;
        instance.loserscene.SetActive(true);
    }
}
