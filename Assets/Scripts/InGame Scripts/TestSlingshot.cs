using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSlingshot : MonoBehaviour
{
    private GameObject[] Planets;
    private LineRenderer Slingshot;
    private GameObject NearestPlanet, CurrentPlanet;
    private bool Direction, Released = false;
    private float Atmosphere = 420;
    private Vector2 currentDirection;
    [SerializeField] private float Speed;

   private void Start()
    {
        //setting up objects
        Planets = GameObject.FindGameObjectsWithTag("Planet");
        Slingshot = gameObject.GetComponent<LineRenderer>();
        //player is attached to a planet when the game starts
        ChangePlanet();
    }
    private void Update()
    {
        Controls();
    }
    private void Controls()
    {
        switch (Released)
        {
            //player is attached to a planet
            case false:
                LineRendererPosition();
                PlayerRotation();
                break;
            //player isn't attached to a planet
            case true:
                PlayerReleased();
                break;
        }
        //evey mouse click
        if (Input.GetMouseButtonDown(0))
        {
            //change player attached/not attached
            Released = !Released;
            switch (Released)
            {
                //line positions
                case false:
                    Slingshot.positionCount = 2;
                    ChangePlanet();
                    break;
                case true:
                    Slingshot.positionCount = 0;
                    break;
            }
        }
    }
    private void ChangePlanet()
    {
        float NearestDistance = 1000;
        foreach (GameObject planet in Planets)
        {
            //if (planet != CurrentPlanet)
            //the player will be attached only with higher planets
            if (planet != CurrentPlanet && transform.position.y <= planet.transform.position.y)
            {
                //calculate the distance between the player and each planet exept the planet the player is attached to
                var distance = Mathf.Abs(Vector2.Distance(gameObject.transform.position, planet.transform.position));
                //
                if (distance <= NearestDistance && distance < 25)
                {
                    NearestDistance = distance;
                    NearestPlanet = planet;
                }
            }
        }
        //
        
        //return the nearest planet
        CurrentPlanet = NearestPlanet;
        DirectionHandler(CurrentPlanet);
        //atmpsphere set
        Atmosphere = CurrentPlanet.GetComponent<Renderer>().bounds.size.x/1.25f;
        //rotate the player
        transform.rotation = Quaternion.LookRotation(Vector3.forward, CurrentPlanet.transform.position - transform.position);
    }
    private void LineRendererPosition()
    {
        if (CurrentPlanet)
        {
            //set line renderer positions
            Slingshot.SetPosition(0, gameObject.transform.position);
            Slingshot.SetPosition(1, NearestPlanet.transform.position);
        }
    }
    private void PlayerRotation()
    {
        //player rotation around planets
        //
        // bool direction = true = anticlockwise
        //
        // bool direction = false = clockwise
        //

        //player rotates around the planet
        if (CurrentPlanet)
        {
            //var radius = Vector2.Distance(transform.position, CurrentPlanet.transform.position);
            //Speed = GeneralSpeed / Mathf.Sqrt(radius / GetComponent<CircleCollider2D>().radius);

            //var dis = Vector2.Distance(transform.position, CurrentPlanet.transform.position);
            //Speed = GeneralSpeed / Mathf.Sqrt(GetComponent<CircleCollider2D>().radius / dis);

            //var rad = GetComponent<CircleCollider2D>().radius;
            //var dis = Vector2.Distance(transform.position, CurrentPlanet.transform.position);
            //Speed = (rad + dis) * 90;

            var radius = Vector2.Distance(transform.position, CurrentPlanet.transform.position);
            float angularVelocity = (Speed / radius) * (180f / Mathf.PI);

            switch (Direction)
            {
                case true:
                    transform.RotateAround(NearestPlanet.transform.position, Vector3.forward, angularVelocity * Time.deltaTime);
                    break;
                case false:
                    transform.RotateAround(NearestPlanet.transform.position, Vector3.forward, -angularVelocity * Time.deltaTime);
                    break;
            }

            /*
            //gravity vars set up
            var distance = Vector2.Distance(transform.position, CurrentPlanet.transform.position);
            var GravitySpeed = ((Speed * 15) / Mathf.Abs((Atmosphere - 10) * 9));
            //if the player is above the atmosphere
            if (distance > Atmosphere)
            {
                //the player slows down if it's too near to the atmosphere
                if (distance - Atmosphere < .45f)
                {
                    GravitySpeed = ((Speed * 15)/ Mathf.Abs((Atmosphere - 10) * 17));
                }
                //the player moves towards the atmosphere
                transform.Translate(transform.up * GravitySpeed * Time.deltaTime, Space.World);
            }
            */

            var distance = Vector2.Distance(transform.position, CurrentPlanet.transform.position);
            var distancePlayerAtmosphere = distance - Atmosphere;
            var GravitySpeed = distancePlayerAtmosphere * 1.25f;
            if (distance > Atmosphere)
            {
                transform.Translate(transform.up * GravitySpeed * Time.deltaTime, Space.World);
            }
        }
    }
    private void PlayerReleased()
    {
        var velocity = Speed;
        //player moves in the space / NOT attached to a planet
        switch (Direction)
        {
            case true:
                currentDirection = transform.right;
                transform.Translate(currentDirection * velocity * Time.deltaTime, Space.World);
                break;
            case false:
                currentDirection = -transform.right;
                transform.Translate(currentDirection * velocity * Time.deltaTime, Space.World);
                break;
        }
    }
    private void DirectionHandler(GameObject planet)
    {
        //when attached to a new planet dedides if anticlockwise or clockwise
        Vector2 planetToPlayer = transform.position - planet.transform.position;
        Vector2 vectorPerpendicular = Vector2.Perpendicular(planetToPlayer).normalized;
        //Debug.Log("perpendicular vector: (" + vectorPerpendicular.x.ToString() + "; " + vectorPerpendicular.y.ToString() + ")");
        Vector3 projectedVector3 = Vector3.Project(currentDirection, vectorPerpendicular).normalized;
        //Debug.Log("vector planet to player: (" + planetToPlayer.x.ToString() + "; " + planetToPlayer.y.ToString() + ")");
        //Debug.Log("projected vector: (" + projectedVector3.x.ToString() + "; " + projectedVector3.y.ToString() + ")");

        if (planetToPlayer.x >= 0f)
        {
            if (planetToPlayer.y >= 0f)
            {
                if (projectedVector3.x >= 0f)
                {
                    Direction = false;
                }
                else if (projectedVector3.x < 0f)
                {
                    Direction = true;
                }
            }
            else if (planetToPlayer.y < 0f)
            {
                if (projectedVector3.x >= 0f)
                {
                    Direction = true;
                }
                else if (projectedVector3.x < 0f)
                {
                    Direction = false;
                }
            }
        }
        else if (planetToPlayer.x < 0f)
        {
            if (planetToPlayer.y >= 0f)
            {
                if (projectedVector3.x >= 0f)
                {
                    Direction = false;
                }
                else if (projectedVector3.x < 0f)
                {
                    Direction = true;
                }
            }
            else if (planetToPlayer.y < 0f)
            {
                if (projectedVector3.x >= 0f)
                {
                    Direction = true;
                }
                else if (projectedVector3.x < 0f)
                {
                    Direction = false;
                }
            }
        }
    }
}
