using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputKeyDouble
{
    public InputEnum input;
    [SerializeField] private KeyCode intended;
    [SerializeField] private KeyCode alternative;

    public bool KeyPressed()
    {
        return Input.GetKey(intended) || Input.GetKey(alternative);
    }

    public void SetKeybinds(KeyCode _intended, KeyCode _alternative)
    {
        intended = _intended;
        alternative = _alternative;
    }
}