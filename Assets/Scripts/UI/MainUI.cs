using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] GameObject instructionPanel;
    public void LoadLevel()
    {
        SceneManager.LoadScene("LevelMain");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToggleInstructionPanel()
    {
        if (instructionPanel.activeInHierarchy)
        {
            instructionPanel.SetActive(false);
        }
        else
        {
            instructionPanel.SetActive(true);
        }
    }
}
