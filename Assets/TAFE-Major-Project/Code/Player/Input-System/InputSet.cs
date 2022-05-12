using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputSet
{
    [SerializeField] private InputEnum[] requiredInputs;
    [SerializeField] private InputEnum[] disallowedInputs;

    public InputSet(List<InputEnum> _inputs)
    {
        requiredInputs = new InputEnum[_inputs.Count];
        _inputs.CopyTo(requiredInputs, 0);
    }

    public bool IsValid(InputEnum[] _playerInputs)
    {
        foreach(InputEnum _playerInput in _playerInputs)
        {
            foreach(InputEnum disallowedInput in disallowedInputs)
            {
                if (_playerInput == disallowedInput)
                    return false;
            }
        }

        foreach (InputEnum requiredInput in requiredInputs)
        {
            bool isContained = false;
            foreach (InputEnum _playerInput in _playerInputs)
            {
                if (_playerInput == requiredInput)
                    isContained = true;
            }
            if (!isContained)
            {
                return false;
            }
        }
        return true;
    }
}
