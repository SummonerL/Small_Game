using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
*   This class maintains a list of interactive objects, and provides handlers for
*   objects entering/leaving the player's proximity
***/
public class InteractiveObjectsScript : MonoBehaviour
{

    // keep a singleton reference to this script
    private static InteractiveObjectsScript _instance;

    public static InteractiveObjectsScript Instance { get { return _instance; } }

    private static GameObject[] _interactiveObjects; // keep track of all child gameObjects 
    public static GameObject[] InteractiveObjects { get { return _interactiveObjects; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    // start is called before the first frame update
    void Start()
    {
        _interactiveObjects = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++) {
            _interactiveObjects[i] = transform.GetChild(i).gameObject;
        }
    }
}
