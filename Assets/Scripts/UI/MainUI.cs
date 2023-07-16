using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    public void LoadLevel()
    {
        SceneManager.LoadScene("LevelMain");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
