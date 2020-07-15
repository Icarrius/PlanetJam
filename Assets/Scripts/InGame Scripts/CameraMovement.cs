using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float smoothing; //How much the camera is behind the player (between 0 and 1, 1 being right on the player)

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerObject(GameObject.FindGameObjectWithTag("Player").transform);
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
            HandleMovement();
    }

    public void SetPlayerObject(Transform _player)
    {
        player = _player;
    }

    private void HandleMovement()
    {
        Vector2 playerPos = player.position;
        transform.position = Vector3.Lerp(transform.position, playerPos, smoothing);
        transform.position = new Vector3(0f, transform.position.y, -10f);
    }
}
