using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class FactoryFishSpawner : MonoBehaviour
{
    // TODO: Not used at this state, but can be implemented down the line
    [HideInInspector]
    enum FishState
    {
        Alive,
        Stunned,
        Dead
    }

    [SerializeField]
    [Tooltip("The gameobject prefab to spawn")]
    private GameObject fishPrefab;

    [SerializeField]
    [Tooltip("The maximum amount of fish that can be spawned")]
    private int maxAmountOfFish;

    // Counts the amount of child gameobjects in the spawner
    private int currentAmountOfFish;

    [SerializeField]
    [Range(0.1f, 10)]
    [Tooltip("The rate at which the fish will spawn in seconds")]
    private float spawnRate;

    [SerializeField]
    [Range(0, 10)]
    [Tooltip("The maximum spawn delay variation in seconds. 0 means no variation.")]
    private float varationInSpawnrate;

    [SerializeField]
    [Tooltip("If toggled, the fish will spawn with the different sizes")]
    private bool fishSizeVariation;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFish());
    }

    void Update()
    {
        // Checks the amount of spawned gameobjects in the simulation
        currentAmountOfFish = gameObject.transform.childCount;
    }

    private IEnumerator SpawnFish()
    {
        // Waits for number of seconds equal to the spawnrate + variation
        yield return new WaitForSeconds(spawnRate + RandomizeSpawnRateVariation());

        if (currentAmountOfFish < maxAmountOfFish)
        {
            // spawn object as a child of the spawner object
            GameObject childGameObject = Instantiate(
                fishPrefab,
                transform.position,
                Random.rotation,
                transform
            );
            childGameObject.name = "FactoryFish" + gameObject.transform.childCount.ToString();

            if (fishSizeVariation)
            {
                float randomSize = RandomizeObjectSize();
                childGameObject.transform.localScale = new Vector3(
                    randomSize,
                    randomSize,
                    randomSize
                );
            }
        }

        StartCoroutine(SpawnFish());
    }

    private float RandomizeObjectSize()
    {
        // the size variation of the fish relative to the parent spawner
        return Random.Range(9, 15);
    }

    private float RandomizeSpawnRateVariation()
    {
        return Random.Range(0.5f, varationInSpawnrate);
    }
}
