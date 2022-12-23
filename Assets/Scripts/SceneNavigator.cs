using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneNavigator : MonoBehaviour
{
    public SceneNavigator instance;
    public GameObject controls;

    private void Start()
    {
        instance = this;
    }

    // Start is called before the first frame update
    public void Play()
    {
        SceneManager.LoadScene("MainScene");
        Cursor.visible = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Cursor.visible = true;
    }

    public void Controls()
    {
        controls.SetActive(true);
    }

    public void ControlsHide()
    {
        controls.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
