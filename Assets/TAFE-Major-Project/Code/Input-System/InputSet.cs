using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class InputSetComponent
{
    public InputEnum input;
    public bool isPressed;
}

[System.Serializable]
public class InputSet
{
    [SerializeField] private InputSetComponent[] requiredInputs;
    [SerializeField, Min(0)] private float timePressed = 0;

    public bool IsValid(InputEnum[] _playerInputs, ref float timer)
    {
        //Check if the input matches
        foreach(InputSetComponent input in requiredInputs)
        {
            bool inputPressed = !input.isPressed;
            foreach (InputEnum _playerinput in _playerInputs)
            {

                if(_playerinput == input.input)
                {
                    if (input.isPressed)
                    {
                        inputPressed = true;
                    }
                    else
                    {
                        inputPressed = false;
                    }
                }
            }
            if (!inputPressed)
                return false;
        }

        //If it matches, check if it's been pressed for long enough
        if(timer >= timePressed)
        {
            return true;
        }
        else
        {
            timer += Time.deltaTime;
            return false;
        }
    }
}
