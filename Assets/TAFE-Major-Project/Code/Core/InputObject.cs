using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu(menuName = "ScriptableObjects/InputObject")]
public class InputObject : ScriptableObject
{
    public InputKeyDouble[] keybinds;

    public InputEnum[] RecordInputs(ref Vector2 _inputAxis)
    {
        List<InputEnum> currentInputs = new List<InputEnum>();
        _inputAxis = Vector2.zero;

        foreach(InputKeyDouble input in keybinds)
        {
            if (input.KeyPressed())
            {
                currentInputs.Add(input.input);
                switch (input.input)
                {
                    case InputEnum.Right:
                        _inputAxis += Vector2.right;
                        break;
                    case InputEnum.Left:
                        _inputAxis += Vector2.left;
                        break;
                    case InputEnum.Forward:
                        _inputAxis += Vector2.up;
                        break;
                    case InputEnum.Back:
                        _inputAxis += Vector2.down;
                        break;
                }
            }
        }

        return currentInputs.ToArray();
    }

    public void SaveKeybinds()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Settings.dat");
        SavedKeybinds savedKeybinds = new SavedKeybinds(keybinds);
        bf.Serialize(file, savedKeybinds);
        file.Close();
        Debug.Log("file saved");
    }

    public void LoadKeybinds()
    {
        if(File.Exists(Application.persistentDataPath + "/Settings.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Settings.dat", FileMode.Open);
            SavedKeybinds savedKeybinds = (SavedKeybinds)bf.Deserialize(file);
            file.Close();
            keybinds = savedKeybinds.inputKeybinds;
            Debug.Log("file loaded");
        }
    }
}

[Serializable]
public class SavedKeybinds
{
    public InputKeyDouble[] inputKeybinds;

    public SavedKeybinds(InputKeyDouble[] _keybinds)
    {
        inputKeybinds = _keybinds;
    }
}