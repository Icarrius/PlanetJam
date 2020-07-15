using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    public static FlashEffect singleton;

    private Animator animator;
    // Start is called before the first frame update

    private void Awake()
    {
        Screen.SetResolution(414, 736, false);
        Screen.fullScreen = false;
    }

    void Start()
    {
        singleton = this;
        animator = gameObject.GetComponent<Animator>();
    }

    public void FlashScreen()
    {
        animator.Play("Flash1");
    }

    public void FlashScreenWhite()
    {
        animator.Play("Flash");
    }
}
