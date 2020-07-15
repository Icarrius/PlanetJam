using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private GameObject TutorialText;
    public float Timer = 5;
    private Manager manager;

    private void Start()
    {
        manager = GameObject.Find("Game Manager").GetComponent<Manager>();

        TutorialText = gameObject;
        TutorialText.SetActive(false);

        if (manager.level == 0)
        {
            TutorialText.SetActive(true);
        }
        else
        {
            TutorialText.SetActive(false);
        }
    }
    private void Update()
    {
        if (TutorialText)
        {
            if (Timer >= 0)
            {
                Timer -= Time.deltaTime;
            }
            else
            {
                TutorialText.SetActive(false);
            }
        }
    }
}
