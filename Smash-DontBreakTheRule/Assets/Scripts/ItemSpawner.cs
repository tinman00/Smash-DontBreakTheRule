using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Vector3[] Spawn;
    public GameObject[] GO;
    public float firstSpawnTime = 30f;
    private float lastSpawnTime = 0f;
    public float intervalTime = 60f;

    void Start()
    {
        lastSpawnTime = Time.time + firstSpawnTime - intervalTime;
    }

    void Update()
    {
        if (Time.time >=  lastSpawnTime + intervalTime) {
            SpawnItem();
            lastSpawnTime = Time.time;
        }
    }

    private void SpawnItem() {
        var obj = GO[Random.Range(0, GO.Length)];
        var i = Random.Range(0, Spawn.Length);
        var pos = Spawn[i];
        Instantiate(obj, pos, new Quaternion());
        var tmp = Spawn[i];
        Spawn[i] = Spawn[Spawn.Length - 1];
        Spawn[Spawn.Length - 1] = tmp;
        obj = GO[Random.Range(0, GO.Length)];
        pos = Spawn[Random.Range(0, Spawn.Length - 1)];
        Instantiate(obj, pos, new Quaternion());
    }
}
