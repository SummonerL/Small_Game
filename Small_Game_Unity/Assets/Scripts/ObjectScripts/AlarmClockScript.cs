using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlarmClockScript : MonoBehaviour
{
    private static AlarmClockScript _instance;

    public static AlarmClockScript Instance { get { return _instance; } }

    [SerializeField]
    private TMP_Text _clockText;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }


    public void ChangeClockText(string time) {
        _clockText.text = time;
    }

    public string GetClockText() {
        return _clockText.text;
    }
}
