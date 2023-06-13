using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject ApplePrefab;
    public Vector2 center;
    public Vector2 size;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;

    void Start()
    {
        InvokeRepeating("SpawnApple", spawnTime, spawnDelay); //Invoke repeats, calls the spawn object with initial time it takes to spawn and time delay between each instantiation
        
    }

    public void SpawnApple()
    {
        Vector2 pos = center + new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2));
        Instantiate(ApplePrefab, pos, Quaternion.identity);
        if (stopSpawning)
        {
            CancelInvoke("SpawnApple");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0,0.5f);
        Gizmos.DrawCube(center, size);
    }
}
