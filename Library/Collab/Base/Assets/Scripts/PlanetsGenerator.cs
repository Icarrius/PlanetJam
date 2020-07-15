using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetsGenerator : MonoBehaviour
{

    [SerializeField]
    private GameObject planetPrefab, finishPrefab, coinPrefab;

    [SerializeField]
    private int totalPlanets, minCoinsAroundPlanet, maxCoinsAroundPlanet;

    [SerializeField]
    private float minSize, maxSize;

    [SerializeField]
    private float minVerticalDistance, maxVerticalDistance, minHorizontalOffset, maxHorizontalOffset;

    private void Awake()
    {
        GenerateLevel(totalPlanets);
    }

    private void GenerateLevel(int planets)
    {
        // For start - basic 1-st planet position (possibly need to change later)
        Vector2 prevPlanetPosition = new Vector2(-2, 0);

        for(int planet = 0; planet < totalPlanets; planet++)
        {
            // Set distance from previous planet
            float verticalDistance = Random.Range(minVerticalDistance, maxVerticalDistance);
            float horizontalOffset = Random.Range(minHorizontalOffset, maxHorizontalOffset);

            Vector2 planetPosition;
            if (planet % 2 == 0)
                planetPosition = new Vector2(1.5f + horizontalOffset, prevPlanetPosition.y + verticalDistance);
            else
                planetPosition = new Vector2(-1.5f + horizontalOffset, prevPlanetPosition.y + verticalDistance);

            // Set planet size
            float planetSize = Random.Range(minSize, maxSize);
            Vector2 planetScale = new Vector2(planetSize, planetSize);

            GameObject newPlanet = Instantiate(planetPrefab, planetPosition, Quaternion.identity);
            newPlanet.transform.localScale = planetScale;
            prevPlanetPosition = newPlanet.transform.position;

            // Just for start, then we going to change it
            if(planet % 5 == 0)
            {
                int coinsNumber = Random.Range(minCoinsAroundPlanet, maxCoinsAroundPlanet);
                GenerateCoinsAroundPlanet(newPlanet.transform, coinsNumber);
            }
        }

        Vector2 finishPosition = new Vector2(0, prevPlanetPosition.y + 4);
        Instantiate(finishPrefab, finishPosition, Quaternion.identity);
    }

    private void GenerateCoinsAroundPlanet(Transform planet, int coinsNumber)
    {
        float degree = 360 / coinsNumber;
        float radius = planet.localScale.x / 6;

        for (int coin = 0; coin < coinsNumber; coin++)
        {
            float angle = coin * degree;
            Vector2 coinPosition = new Vector2(planet.position.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad), planet.position.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad));
            Instantiate(coinPrefab, coinPosition, Quaternion.identity);
        }
    }
}
