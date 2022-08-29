using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [HideInInspector] public bool isOpen = false;
    [SerializeField] private Collider triggerCollider;
    private Door door;
    public void SetDoor(Door _door)
    {
        door = _door;
    }

    private void OnTriggerEnter(Collider other)
    {
        isOpen = true;
        triggerCollider.enabled = false;
        door.CheckIfOpen();
    }
}
