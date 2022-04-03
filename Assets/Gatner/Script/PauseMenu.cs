using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button _buttonContinue;
    [SerializeField] private Button _buttonMainMenu;
    [SerializeField] private Canvas _pauseMenu;

    private void Awake()
    {
        _buttonContinue.onClick.AddListener(Continue);
        _buttonMainMenu.onClick.AddListener(MainMenu);
        _pauseMenu = FindObjectOfType<Canvas>();
        _pauseMenu.enabled = false;
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Time.timeScale = 0;
            _pauseMenu.enabled = true;
        }

        if (Input.GetKey(KeyCode.N))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    public void Continue()
    {
        Time.timeScale = 1;
        _pauseMenu.enabled = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
