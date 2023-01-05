using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSpawn : MonoBehaviour
{
    public GameObject[] spawn_points;
    public GameObject Enemy;
    public Text spawn_time;
    public float spawn_interval = 5f;
    public int spawn_number = 5;

    private float time_count;
    private Transform[] spawn_points_transform;

    private void Start()
    {
        time_count = spawn_interval;
        spawn_points_transform = new Transform[spawn_points.Length];
        for (int i = 0; i < spawn_points.Length; i++)
        {
            spawn_points_transform[i] = spawn_points[i].transform;
        }
    }
    private void Update()
    {
        time_count -= Time.deltaTime;
        if (time_count <= 0)
        {
            time_count = spawn_interval;
            for (int i = 0; i < spawn_points.Length; i++)
            {
                for (int k = 0; k < spawn_number; k++)
                {
                    GameObject enemy = Instantiate(Enemy, spawn_points_transform[i].position, new Quaternion(0, 0, 0, 0));
                    enemy.SetActive(true);
                }
            }
        }
        spawn_time.text = "Next wave: " + (time_count).ToString();
    }
}
