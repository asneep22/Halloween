using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderExtended : SceneLoader
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!UIManager.Instance.GameMenu.activeInHierarchy)
            {
                UIManager.Instance.GameMenu.SetActive(true);
            } else
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        if (!UIManager.Instance.IsLose)
        {
            Time.timeScale = 1;
            UIManager.Instance.GameMenu.SetActive(false);
        }
    }
}
