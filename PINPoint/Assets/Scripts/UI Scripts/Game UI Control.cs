using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIControl : MonoBehaviour
{
    //Variables
    [SerializeField]
    private TextMeshProUGUI payDisplay, timeDisplay;

    private float min, sec, mSec;
    Text timer;
    private float elapsedTime = 0;

    private void Update()
    {
        elapsedTime += Time.deltaTime *5;
        min = Mathf.FloorToInt(elapsedTime/60);
        sec = Mathf.FloorToInt(elapsedTime%60);
        mSec = Mathf.FloorToInt((elapsedTime%1f) * 60);

        //min.ToString("00"), sec.ToString("00"), mSec.ToString("00");

        //timeDisplay.text = string.Format("{00:00:0}:{01:00:2}", min, sec, mSec);
        timeDisplay.text = min.ToString("00") + ":" + sec.ToString("00") + ":" + mSec.ToString("00");

    }
}
