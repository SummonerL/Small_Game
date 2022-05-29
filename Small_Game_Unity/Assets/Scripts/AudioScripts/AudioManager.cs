using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    // keep a singleton reference to this script
    private static AudioManager _instance;

    public static AudioManager Instance { get { return _instance; } }

    // various audio sources
    [SerializeField]
    private GameObject _smartphone;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public void PhoneAudio(bool stop = false)
    {
        AudioSource audioData = _smartphone.GetComponent<AudioSource>();

        if (stop) {
            audioData.Stop();
        } else {
            audioData.Play(0);
        }
    }
}
