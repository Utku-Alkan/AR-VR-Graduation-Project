using UnityEngine;

public class SpawnTimestamp : MonoBehaviour
{
    public float spawnTime;
    public float lifeTime = 0f;

    void Start()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        lifeTime = Time.time - spawnTime;
    }
}