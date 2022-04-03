using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private Button _buttonSave;


    private void Awake()
    {
        _buttonSave.onClick.AddListener(Save);
    }


    public void Save()
    {
        SceneManager.LoadScene(0);
    }
}
