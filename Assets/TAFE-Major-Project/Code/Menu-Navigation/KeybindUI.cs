using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindUI : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform prefabHolder;
    private List<KeybindRemapUI> uiList;
    private KeybindObject keybinds;

    private void Awake()
    {
        uiList = new List<KeybindRemapUI>();
    }

    private void Start()
    {
        keybinds = GameManager.Instance.ActualKeybinds;

        foreach(InputKeyDouble key in keybinds.keybinds)
        {
            KeybindRemapUI keyUI = Instantiate(prefab, prefabHolder).GetComponent<KeybindRemapUI>();
            Debug.Log(key.alternative);
            keyUI.Initialise(key);
            uiList.Add(keyUI);

        }
    }

    public void CheckForDuplicates()
    {

    }
}
