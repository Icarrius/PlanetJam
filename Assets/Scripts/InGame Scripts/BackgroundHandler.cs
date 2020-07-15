using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundHandler : MonoBehaviour
{
    private GameObject Stars, Details;

    private void Start()
    {
        var LevelHeight = GameObject.Find("Finish(Clone)").transform.position.y;

        //setting up starts
        Stars = GameObject.Find("Stars");
        Stars.GetComponent<SpriteRenderer>().size = new Vector2(LevelHeight / 10, LevelHeight);

        //
        Details = GameObject.Find("BackDetails");
        for (int i = 1; i < Mathf.FloorToInt(LevelHeight / 40); i++)
        {
            Vector2 Position = new Vector2(0, 58 * i);
            Instantiate(Details, Position, Quaternion.identity);
        }
    }
}
