using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timeBeforeStartTranslating;
    private float timer;
    private bool canTranslate;
    private Vector2 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        timer = timeBeforeStartTranslating;
        canTranslate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!canTranslate)
        {
            if(timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                canTranslate = true;
            }
        }
        else
        {
            transform.Translate(Vector3.up.normalized * speed * Time.deltaTime);
        }
    }

    public void ResetPosition()
    {
        timer = timeBeforeStartTranslating;
        canTranslate = false;
        transform.position = startPosition;
    }
}
