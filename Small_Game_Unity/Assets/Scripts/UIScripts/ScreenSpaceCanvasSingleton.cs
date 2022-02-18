using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceCanvasSingleton : MonoBehaviour
{
    private static ScreenSpaceCanvasSingleton _instance;

    public static ScreenSpaceCanvasSingleton Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
}
