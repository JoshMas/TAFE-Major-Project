using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Checkpoint set");
        GameManager.Instance.SetSpawnpoint(transform.position);
        gameObject.SetActive(false);
        LevelManager.savedTimer = PlayerUI.timer;
    }
}
