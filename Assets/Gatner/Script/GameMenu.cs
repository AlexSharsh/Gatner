using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private Button _buttonStart;
    [SerializeField] private Button _buttonSetting;
    [SerializeField] private Button _buttonQuit;

    private void Awake()
    {
        _buttonStart.onClick.AddListener(StartGame);
        _buttonQuit.onClick.AddListener(() => { Application.Quit(); });
    }


    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
