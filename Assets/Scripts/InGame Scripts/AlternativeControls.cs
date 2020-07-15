using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativeControls : MonoBehaviour
{
    private GameObject[] Planets;
    private LineRenderer Slingshot;
    private GameObject CurrentPlanet;
    private Camera MainCamera;
    public bool Pressing;
    public float Speed = 0;

    private Vector3 PlayerPos;

    private void Start()
    {
        Planets = GameObject.FindGameObjectsWithTag("Planet");
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Slingshot = gameObject.GetComponent<LineRenderer>();
        //Pressing = true;
    }
    private void Update()
    {
        Controls();
        Target();
        LinePosition();
    }
    private void Controls()
    {
        if (Input.GetMouseButton(0))
        {
            Attached();
            //Pressing = true;
        }
        else
        {
            NotAttached();
            //Pressing = false;
        }
    }
    private void Target()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(MainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Planet")
            {
                CurrentPlanet = hit.collider.gameObject;
            }
        }
    }
    private void Attached()
    {
        if (CurrentPlanet)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, CurrentPlanet.transform.position - transform.position);
        }
        Speed = Mathf.Lerp(Speed, 5, .2f);
        transform.Translate(transform.up * Speed * Time.deltaTime, Space.World);
    }
    private void NotAttached()
    {
        Speed = Mathf.Lerp(Speed, 0, .025f);
        transform.Translate(transform.up * Speed * Time.deltaTime, Space.World);
    }
    private void LinePosition()
    {
        if (CurrentPlanet)
        {
            //set line renderer positions
            Slingshot.SetPosition(0, gameObject.transform.position);
            Slingshot.SetPosition(1, CurrentPlanet.transform.position);
        }
    }
}
