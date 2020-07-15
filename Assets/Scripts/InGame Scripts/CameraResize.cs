using UnityEngine;

public class CameraResize : MonoBehaviour
{
    void Start()
    {
        if(Screen.height > Screen.width)
        {
            Camera.main.orthographicSize = 16;
        }
        else
        {
            Camera.main.orthographicSize = 5;
        }
    }
}
