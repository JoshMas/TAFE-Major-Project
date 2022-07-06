using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputKeyDouble
{
    [SerializeField] private KeyCode intended;
    [SerializeField] private KeyCode alternative;

    public bool KeyPressed()
    {
        return Input.GetKey(intended) || Input.GetKey(alternative);
    }
}
