using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{

    public GameManager gameManger;
    public PointManager pointManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManger.TimeRemain <= 0)
        {
            int pointsToKeep = pointManager.myPoints;
            StaticData.Score = pointsToKeep;
            SceneManager.LoadScene(1);
        }
    }
}
