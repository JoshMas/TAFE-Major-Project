using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        GameManager.Instance.UIMode(false);
        GameManager.Instance.StartLevel(transform.position);
        GameManager.Instance.PlaceAtSpawnpoint(player);
    }
}
