using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject foodPrefab; // Prefab de la comida
    public Transform[] spawnPoints; // Puntos donde se puede spawnear la comida
    //public int numberOfFoodItems = 5; // Cantidad de comida a spawnear
    public float spawnInterval = 2f; // Tiempo entre spawns

    private void Start()
    {
        StartCoroutine(SpawnFood());
    }

    private IEnumerator SpawnFood()
    {


        while (true)
        {
            SpawnRandomFood();
            yield return new WaitForSeconds(spawnInterval);
        }

    }

    private void SpawnRandomFood()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];
        Instantiate(foodPrefab, spawnPoint.position, spawnPoint.rotation);
    }

}
