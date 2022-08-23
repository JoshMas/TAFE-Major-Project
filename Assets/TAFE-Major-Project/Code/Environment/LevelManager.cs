using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Transform player;
    public static float savedTimer = 0;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        GameManager.Instance.UIMode(false);
        GameManager.Instance.StartLevel(transform.position);
        GameManager.Instance.PlaceAtSpawnpoint(player);
    }
}
