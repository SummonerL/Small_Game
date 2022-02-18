using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceCanvasSingleton : MonoBehaviour
{
    private static WorldSpaceCanvasSingleton _instance;

    public static WorldSpaceCanvasSingleton Instance { get { return _instance; } }


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
