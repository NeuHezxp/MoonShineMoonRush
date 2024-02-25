using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversalTimer : GlobalTimer
{
    // actually counts our timer down
    // i changed these from serialized to private so i can use the GameManager (CHANGE BACK IF BROKE)
    [SerializeField] protected TextMeshProUGUI timerText;

    private void Update()
    {
            remainingTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
