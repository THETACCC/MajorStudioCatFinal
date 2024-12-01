using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetmyPoints : MonoBehaviour
{
    public int myPointsEnd;
    public TextMeshProUGUI PointsTextEnd;
    // Start is called before the first frame update
    void Start()
    {
        myPointsEnd = StaticData.Score;
        PointsTextEnd.text = myPointsEnd.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
