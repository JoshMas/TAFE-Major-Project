using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "ScriptableObjects/MoveInput", order = 1)]
public class MoveInput : ScriptableObject
{
    public InputSet[] inputSet;
    private int inputMarker = 0;
    private float timer = 0;

    public bool Check(Queue<InputEnum[]> _inputs)
    {
        inputMarker = 0;
        timer = 0;

        foreach(InputEnum[] _playerInput in _inputs)
        {
            if (inputSet[inputMarker].IsValid(_playerInput, ref timer))
            {
                timer = 0;
                inputMarker++;
                if(inputMarker >= inputSet.Length)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void Activate(Player _player)
    {
        _player.PerformMove(name);
    }
}
