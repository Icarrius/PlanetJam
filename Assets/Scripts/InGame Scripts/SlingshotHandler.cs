using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotHandler : MonoBehaviour
{
    private GameObject[] Planets;
    private LineRenderer Slingshot;
    private GameObject NearestPlanet, CurrentPlanet;
    private bool Direction, Released = true;
    private float PlayerSpeed, Speed = 12, Atmosphere, PlanetDistance;
    private Vector2 currentDirection;
    public int Progression;

    private void Start()
    {
        //setting up objects
        Planets = GameObject.FindGameObjectsWithTag("Planet");
        Slingshot = gameObject.GetComponent<LineRenderer>();
        //player is attached to a planet when the game starts
        //ChangePlanet();
        PlayerSpeed = Speed;
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
        if (NearestPlanet && CurrentPlanet)
        {
            //gravity streaches
            PlanetDistance = Vector2.Distance(NearestPlanet.transform.position, CurrentPlanet.transform.position);
        }
        //HandleDirection(CurrentPlanet, NearestPlanet);
        //return the nearest planet
        CurrentPlanet = NearestPlanet;
        //handle direction
        DirectionHandler(CurrentPlanet);
        //atmpsphere set
        if (CurrentPlanet.name.Contains("Small"))
        {
            //Atmosphere = CurrentPlanet.GetComponent<SpriteRenderer>().bounds.size.y / 1.55f;
            Atmosphere = 2.45f;
        }
        else if (CurrentPlanet.name.Contains("Medium"))
        {
            //Atmosphere = CurrentPlanet.GetComponent<SpriteRenderer>().bounds.size.y / 1.85f;
            Atmosphere = 3.25f;
        }
        else if (CurrentPlanet.name.Contains("Big"))
        {
            //Atmosphere = CurrentPlanet.GetComponent<SpriteRenderer>().bounds.size.y / 1.85f;
            Atmosphere = 3.35f;
        }
        //rotate the player
        //transform.rotation = Quaternion.LookRotation(Vector3.forward, CurrentPlanet.transform.position - transform.position);
        //play attach sound
        SoundHandler.singleton.PlayPlanetAttach();
    }
    private void LineRendererPosition()
    {
        if (CurrentPlanet)
        {
            //set line renderer positions
            Slingshot.SetPosition(0, transform.position);
            Slingshot.SetPosition(1, NearestPlanet.transform.position);
        }
    }
    private void PlayerRotation()
    {
        //rotate player on the z axis
        Quaternion PlanetRotation = new Quaternion(0, 0, 0, 0);
        switch (Direction)
        {
            case true:
                PlanetRotation = Quaternion.LookRotation(Vector3.forward, CurrentPlanet.transform.position - transform.position);
                break;
            case false:
                PlanetRotation = Quaternion.LookRotation(Vector3.forward, -(CurrentPlanet.transform.position - transform.position));
                break;
        }
        float RotationRatio = Speed / (135 * Mathf.Sqrt(Vector2.Distance(transform.position, CurrentPlanet.transform.position)));
        transform.rotation = Quaternion.Lerp(transform.rotation, PlanetRotation, RotationRatio);

        //player rotation around planets
        //
        // bool direction = true = anticlockwise
        //
        // bool direction = false = clockwise
        //

        PlayerSpeed += (Speed / 4.5f) * Time.deltaTime;
        PlayerSpeed = Mathf.Clamp(PlayerSpeed, 0, Speed);

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
            float angularVelocity = (PlayerSpeed / radius) * (180f / Mathf.PI);
            switch (Direction)
            {
                case true:
                    transform.RotateAround(CurrentPlanet.transform.position, Vector3.forward, angularVelocity * Time.deltaTime);
                    break;
                case false:
                    transform.RotateAround(CurrentPlanet.transform.position, Vector3.forward, -angularVelocity * Time.deltaTime);
                    break;
            }
            transform.Translate(transform.right * (Speed / 5) * Time.deltaTime, Space.World);
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
            GravityHandler();
        }
    }
    private void GravityHandler()
    {
        /*
        var distance = Vector2.Distance(transform.position, CurrentPlanet.transform.position);
        var distancePlayerAtmosphere = distance - Atmosphere;
        var GravitySpeed = distancePlayerAtmosphere * 1.25f;
        GravitySpeed = Mathf.Clamp(GravitySpeed, 0f, 7f);
        if (distance > Atmosphere)
        {
            transform.Translate(transform.up * GravitySpeed * Time.deltaTime, Space.World);
        }
        */
        /*
        var distance = Vector2.Distance(transform.position, CurrentPlanet.transform.position);
        var distancePlayerAtmosphere = Mathf.Abs(distance - Atmosphere);
        var GravitySpeed = (distancePlayerAtmosphere * 1.35f) - (PlanetDistance / PlayerSpeed);
        PlanetDistance -= (PlayerSpeed / 3.5f) * Time.deltaTime;
        PlanetDistance = Mathf.Clamp(PlanetDistance, 0, 8);
        GravitySpeed = Mathf.Clamp(GravitySpeed, 0, 15);

        if (distancePlayerAtmosphere > Atmosphere * 1.35f) 
        {
            switch (Direction)
            {
                case true:
                    transform.Translate(transform.up * (GravitySpeed * Time.deltaTime), Space.World);
                    break;
                case false:
                    transform.Translate(transform.up * -(GravitySpeed * Time.deltaTime), Space.World);
                    break;
            }
        }
        */
        var distance = Vector2.Distance(transform.position, CurrentPlanet.transform.position);
        var distancePlayerAtmosphere = Mathf.Abs(distance - Atmosphere);
        var GravitySpeed = (distancePlayerAtmosphere * 1.35f) - (PlanetDistance / PlayerSpeed) + (Speed / 5);
        PlanetDistance -= (PlayerSpeed / 3.5f) * Time.deltaTime;
        PlanetDistance = Mathf.Clamp(PlanetDistance, 0, 10);
        GravitySpeed = Mathf.Clamp(GravitySpeed, 0, 20);

        if (distance > Atmosphere) 
        {
            switch (Direction)
            {
                case true:
                    transform.Translate(transform.up * (GravitySpeed * Time.deltaTime), Space.World);
                    break;
                case false:
                    transform.Translate(transform.up * -(GravitySpeed * Time.deltaTime), Space.World);
                    break;
            }
        }
        else
        {
            switch (Direction)
            {
                case true:
                    transform.Translate(transform.up * -(GravitySpeed * Time.deltaTime), Space.World);
                    break;
                case false:
                    transform.Translate(transform.up * (GravitySpeed * Time.deltaTime), Space.World);
                    break;
            }

        }
    }
    private void PlayerReleased()
    {
        //loosing momentum
        PlayerSpeed -= (Speed / 3.35f) * Time.deltaTime;
        PlayerSpeed = Mathf.Clamp(PlayerSpeed, 0, Speed);
        var velocity = PlayerSpeed + (Speed / 5);
        //player moves in the space / NOT attached to a planet
        /*
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
        */
        currentDirection = transform.right;
        transform.Translate(currentDirection * velocity * Time.deltaTime, Space.World);
    }
    public int CheckProgession()
    {
        var LevelLenght = (GameObject.Find("Finish(Clone)").transform.position.y - 1f);// - (GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize) / 2;
        Progression = Mathf.FloorToInt((100 * transform.position.y) / LevelLenght);
        Progression = Mathf.Clamp(Progression, 0, 100);
        return Progression;
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
    //
    private bool isLeft(Vector2 a, Vector2 b, Vector2 c)
    {
        return ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x)) > 0;
    }
    private void HandleDirection(GameObject lastplanet, GameObject currentplanet)
    {
        if (lastplanet && NearestPlanet)
        {
            //setting up vars
            var velocity = Speed;
            var distance = Vector2.Distance(lastplanet.transform.position, currentplanet.transform.position);
            Vector2 PlayerProjection;
            if (Direction)
            {
                //PlayerProjection = new Vector2(transform.position.x + ((transform.right.x * Speed / 20 * Time.deltaTime) * distance),
                //  transform.position.y + ((transform.right.y * Speed / 20 * Time.deltaTime) * distance));
                PlayerProjection = new Vector3(transform.position.x, transform.position.y) + ((transform.right * velocity * Time.deltaTime) * distance);
            }
            else
            {
                //PlayerProjection = new Vector2(transform.position.x - ((transform.right.x * Speed / 20 * Time.deltaTime) * distance), 
                //  transform.position.y - ((transform.right.y * Speed / 20 * Time.deltaTime) * distance));
                PlayerProjection = new Vector3(transform.position.x, transform.position.y) + ((-transform.right * velocity * Time.deltaTime) * distance);
            }
            switch (isLeft(lastplanet.transform.position, currentplanet.transform.position, PlayerProjection))
            {
                case true:
                    Direction = false;
                    break;
                case false:
                    Direction = true;
                    break;
            }
        }
    }
}