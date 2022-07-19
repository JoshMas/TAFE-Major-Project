using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Vector3 startingSpawnpoint = Vector3.zero;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        GameManager.Instance.UIMode(false);
        GameManager.Instance.StartLevel(startingSpawnpoint);
        GameManager.Instance.PlaceAtSpawnpoint(player);
    }
}
