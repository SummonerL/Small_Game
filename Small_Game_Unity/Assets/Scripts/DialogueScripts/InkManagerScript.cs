using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkManagerScript : MonoBehaviour
{

    // Ink story files
    public TextAsset inkJSON;
    public Story story;

    // Ink Manager Singleton Ref
    private static InkManagerScript _instance;
    public static InkManagerScript Instance { get { return _instance; } }

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
        story = new Story(inkJSON.text);
    }
}
