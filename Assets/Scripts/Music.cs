using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music GameMusic;
    private void Awake()
    {
        if (GameMusic == null)
        {
            GameMusic = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);


    }
}
