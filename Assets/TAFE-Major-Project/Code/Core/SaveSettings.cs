using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSettings : MonoBehaviour
{

    private void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Settings.dat");
        Settings settings = new Settings();
    }

    private void Load()
    {

    }
}

[Serializable]
public class Settings
{
    public InputKeyDouble[] inputKeybindings;
}
