using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateScript : MonoBehaviour
{
     public float deltaTime;

     [SerializeField]
     public string frameRate;

    // update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        frameRate = Mathf.Ceil (fps).ToString ();
    }
}
