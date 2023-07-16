using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedMenuManager : MonoBehaviour
{
    public GameObject winLooseMenu;
    public GameObject pauseCanvas;


    // Start is called before the first frame update
    void Start()
    {
        pauseCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseCanvas.activeInHierarchy)
            {
                paused();
            }
        }
    }

    public void pauseStop()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void paused()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 0;
    }
}
