using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class MenuHandler : MonoBehaviour
{
    private Text Level;
    private Manager manager;
    [SerializeField] private TextMeshProUGUI Levelcounter, Coins;

    private void Start()
    {
        Levelcounter.text = "LEVEL " + PlayerPrefs.GetInt("level").ToString();
        Coins.text = PlayerPrefs.GetInt("coins").ToString() + "x";
    }
}
