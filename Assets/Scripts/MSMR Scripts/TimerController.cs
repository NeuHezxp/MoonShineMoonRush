using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UniversalTimer : GlobalTimer
{
    // actually counts our timer down
    [SerializeField] TextMeshProUGUI timerText;

    private void Update()
    {
        remainingTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
