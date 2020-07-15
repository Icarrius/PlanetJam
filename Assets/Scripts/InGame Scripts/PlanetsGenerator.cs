using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetsGenerator : MonoBehaviour
{
    public enum Difficult
    {
        dummy, easy, medium, hard, hardest
    }

    private Difficult difficult;

    [SerializeField]
    private GameObject finishPrefab, coinPrefab, asteroidPrefab;

    [SerializeField]
    private GameObject asteroidPrefab1, asteroidPrefab2, asteroidPrefab3;

    [SerializeField]
    private GameObject[] bluePlanets, violetPlanets, pinkPlanets, greenPlanets;

    [SerializeField]
    private int totalPlanets, minCoinsAroundPlanet, maxCoinsAroundPlanet;

    [SerializeField]
    private float coinDistanceFromPlanet;

    private float minVerticalDistance, maxVerticalDistance, minHorizontalOffset, maxHorizontalOffset, leftSpawnPos, rightSpawnPos;

    private Dictionary<GameObject, int> planetsWithCoins = new Dictionary<GameObject, int>();
    private List<Vector2> asteroidPositions = new List<Vector2>();

    private bool asteroidsOn = false;
    private const int colorsNumber = 4;

    public void SetupGenerator(int level)
    {
        if (level >= 0) difficult = Difficult.dummy;
        if (level >= 5) difficult = Difficult.easy;
        if (level >= 10) difficult = Difficult.medium;
        if (level >= 15) difficult = Difficult.hard;
        if (level >= 20) difficult = Difficult.hardest;

        totalPlanets += level * 2;

        switch (difficult)
        {
            case Difficult.dummy:
                SetOffsets(2.25f, 2.75f, -0.5f, 0.5f);
                leftSpawnPos = -3.75f;
                rightSpawnPos = 3.75f;
                break;
            case Difficult.easy:
                leftSpawnPos = -3.5f;
                rightSpawnPos = 3.5f;
                SetOffsets(1.75f, 2.25f, -1f, 1f);
                break;
            case Difficult.medium:
                leftSpawnPos = -3.25f;
                rightSpawnPos = 3.25f;
                SetOffsets(1.5f, 1.75f, -1.5f, 1.5f);
                break;
            case Difficult.hard:
                SetOffsets(1.25f, 1.5f, -1.5f, 1.5f);
                break;
            case Difficult.hardest:
                asteroidsOn = true;
                SetOffsets(0.75f, 1.25f, -2f, 2f);
                break;          
        }

        GenerateLevel(totalPlanets);
    }

    private void SetOffsets(float minVert, float maxVert, float minHoriz, float maxHoriz)
    {
        minVerticalDistance = minVert;
        maxVerticalDistance = maxVert;
        minHorizontalOffset = minHoriz;
        maxHorizontalOffset = maxHoriz;
    }
    private void GenerateLevel(int planets)
    {
        // For start - basic 1-st planet position (*possibly need to change later)
        Vector2 prevPlanetPosition = new Vector2(-2, 0);
        List<int> avaliableColors = new List<int>();
        List<int> usedColors = new List<int>();

        for (int i = 0; i < colorsNumber; i++)
            avaliableColors.Add(i);

        for(int planet = 0; planet < totalPlanets; planet++)
        {
            // Chosing color of planet
            GameObject[] planetsToSpawn;
            int chosenColor = avaliableColors[Random.Range(0, avaliableColors.Count)];
            avaliableColors.Remove(chosenColor);
            usedColors.Add(chosenColor);

            if(avaliableColors.Count < 2)
            {
                avaliableColors.Add(usedColors[0]);
                usedColors.RemoveAt(0);
            }

            switch (chosenColor)
            {
                case 0:
                    planetsToSpawn = bluePlanets;
                    break;
                case 1:
                    planetsToSpawn = pinkPlanets;
                    break;
                case 2:
                    planetsToSpawn = greenPlanets;
                    break;
                case 3:
                    planetsToSpawn = violetPlanets;
                    break;
                default:
                    Debug.LogError("List of avaliable planets to spawn is null");
                    planetsToSpawn = null;
                    break;
            }


            // Set planet size
            GameObject planetPrefab = planetsToSpawn[Random.Range(0, planetsToSpawn.Length)];

            float planetRadius = planetPrefab.GetComponent<CircleCollider2D>().radius;
            float planetSize = (planetRadius * 2);

            float verticalDistance, horizontalOffset;

            verticalDistance = Random.Range(planetSize + minVerticalDistance, planetSize + maxVerticalDistance);
            horizontalOffset = Random.Range(minHorizontalOffset, maxHorizontalOffset);

            Vector2 planetPosition;

            if (difficult <= Difficult.medium)
            {
                if (planet % 2 == 0)
                {
                    planetPosition = new Vector2(rightSpawnPos + horizontalOffset, prevPlanetPosition.y + verticalDistance);
                }
                else
                {
                    planetPosition = new Vector2(leftSpawnPos + horizontalOffset, prevPlanetPosition.y + verticalDistance);
                    
                }
            }
            else
            {
                if (planet % 2 == 0)
                {
                    planetPosition = new Vector2(planetSize / 1.25f - horizontalOffset, prevPlanetPosition.y + verticalDistance);
                }
                else
                {
                    planetPosition = new Vector2(-planetSize / 1.25f + horizontalOffset, prevPlanetPosition.y + verticalDistance);
                }
            }

            if(planet == 0)
                planetPosition = new Vector2(3, 2);
            if(planet == 1)
                planetPosition = new Vector2(-3, 5);

            GameObject newPlanet = Instantiate(planetPrefab, planetPosition, planetPrefab.transform.rotation);

            prevPlanetPosition = newPlanet.transform.position;

            // Mirroring planet
            if (newPlanet.transform.position.x < 0)
            {
                newPlanet.transform.Rotate(new Vector3(0, 180, 0), Space.World);
            }

            // Adding some random rotation
            newPlanet.transform.Rotate(new Vector3(0, 0, Random.Range(-15, 15)));

            if (asteroidsOn && planet > 2)
            {
                GameObject asteroid = GenerateAsteroidsAroundPlanet(newPlanet.transform);
                if (asteroid != null) asteroidPositions.Add(asteroid.transform.position);
            }
            // Just for start, then we going to change it
            if (planet % 5 == 0 && planet != 0)
            {          
                int coinsNumber = Random.Range(minCoinsAroundPlanet, maxCoinsAroundPlanet);
                planetsWithCoins.Add(newPlanet, coinsNumber);
            }
        }

        GenerateCoins();

        Vector2 finishPosition = new Vector2(0, prevPlanetPosition.y + 8);
        Instantiate(finishPrefab, finishPosition, Quaternion.identity);
    }

    public void GenerateCoins()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");

        foreach(GameObject coin in coins)
        {
            Destroy(coin);
        }

        foreach(KeyValuePair<GameObject, int> entry in planetsWithCoins)
        {
            GenerateCoinsAroundPlanet(entry.Key.transform, entry.Value);
        }
    }

    private void GenerateCoinsAroundPlanet(Transform planet, int coinsNumber)
    {
        float degree = 360 / coinsNumber;
        float spawnRadius = planet.gameObject.GetComponent<CircleCollider2D>().radius + coinDistanceFromPlanet;

        for (int coin = 0; coin < coinsNumber; coin++)
        {
            float angle = coin * degree;
            Vector2 coinPosition = new Vector2(planet.position.x + spawnRadius * Mathf.Sin(angle * Mathf.Deg2Rad), planet.position.y + spawnRadius * Mathf.Cos(angle * Mathf.Deg2Rad));
            Instantiate(coinPrefab, coinPosition, Quaternion.identity);
        }
    }

    public void GenerateAsteroids()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        foreach (GameObject asteroid in asteroids)
        {
            Destroy(asteroid);
        }

        foreach (Vector2 pos in asteroidPositions)
        {
            GameObject Asteroid = asteroidPrefab;
            switch (Mathf.CeilToInt(Random.Range(1, 4)))
            {
                case 1:
                    Asteroid = asteroidPrefab1;
                    break;
                case 2:
                    Asteroid = asteroidPrefab2;
                    break;
                case 3:
                    Asteroid = asteroidPrefab3;

                    break;
            }
            Instantiate(Asteroid, pos, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
    }

    private GameObject GenerateAsteroidsAroundPlanet(Transform planet)
    {
        float spawnRadius = planet.gameObject.GetComponent<CircleCollider2D>().radius + Random.Range(2f, 3f);
        float angle = Random.Range(0, 360);
        Vector2 spawnPosition = new Vector2(planet.position.x + spawnRadius * Mathf.Sin(angle * Mathf.Deg2Rad), planet.position.y + spawnRadius * Mathf.Cos(angle * Mathf.Deg2Rad));
        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, 1.75f);
        if(colliders.Length == 0)
        {
            GameObject Asteroid = asteroidPrefab;
            switch (Mathf.CeilToInt(Random.Range(1, 4)))
            {
                case 1:
                    Asteroid = asteroidPrefab1;
                    break;
                case 2:
                    Asteroid = asteroidPrefab2;
                    break;
                case 3:
                    Asteroid = asteroidPrefab3;
                    break;
            }
            return Instantiate(Asteroid, spawnPosition, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }

        return null;
    }
}
