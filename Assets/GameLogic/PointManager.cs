using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class PointManager : MonoBehaviour
{

    public int myPoints;
    public TextMeshProUGUI PointsText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PointsText.text = myPoints.ToString();
    }

    public void AddPoints()
    {

    }
}
