using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public float TimeRemain;

    public float TimeTotal = 30f;
    //Visual Indicate
    public SKSlider DashSliderLeft;
    public SKSlider DashSliderRight;

    public int smallObjectTimeAdd = 2;
    public int mediumObjectTimeAdd = 4;
    public int bigObjectTimeAdd = 8;

    // Start is called before the first frame update
    void Start()
    {
        TimeRemain = TimeTotal;
    }

    // Update is called once per frame
    void Update()
    {
        TimeRemain = Mathf.Clamp(TimeRemain, 0,TimeTotal);
        TimeRemain -= Time.deltaTime;
        UpdateTimerVisual();
    }

    private void UpdateTimerVisual()
    {
        // Calculate the decimal percentage of bloodCount relative to maxBlood
        float DashCoolDownRef = (float)TimeRemain / TimeTotal;
        DashSliderLeft.SetValue(DashCoolDownRef);
        DashSliderRight.SetValue(DashCoolDownRef);
    }

    public void AddTimeSmallObject()
    {
        TimeRemain += smallObjectTimeAdd;
    }

}
