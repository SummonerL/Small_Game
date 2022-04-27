using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalendarScript : MonoBehaviour
{
    private static CalendarScript _instance;

    public static CalendarScript Instance { get { return _instance; } }

    [SerializeField]
    private TMP_Text _monthText;
    [SerializeField]
    private TMP_Text _dayText;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }


    public void ChangeMonthText(string month) {
        _monthText.text = month;
    }

    public void ChangeDayText(int day) {
        _dayText.text = day.ToString();
    }

}
