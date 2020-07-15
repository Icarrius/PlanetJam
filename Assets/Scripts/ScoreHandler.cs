using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    public static ScoreHandler singleton;
    public float startPoints;

    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
