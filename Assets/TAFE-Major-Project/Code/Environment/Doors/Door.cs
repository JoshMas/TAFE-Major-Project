using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Lever[] levers;

    private void Start()
    {
        foreach(Lever lever in levers)
        {
            lever.SetDoor(this);
        }
    }

    public void CheckIfOpen()
    {
        bool isOpen = true;
        foreach(Lever lever in levers)
        {
            if (!lever.isOpen)
                isOpen = false;
        }
        if (isOpen)
            Open();
    }

    private void Open()
    {
        Destroy(gameObject);
    }
}
