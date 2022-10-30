using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private FadeChanger _fadeChanger;

    public void Load(int index)
    {
        _fadeChanger.StartFadeInAndChangeScene(index);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
