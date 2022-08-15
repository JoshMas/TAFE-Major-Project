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
            keyUI.Initialise(key, this);
            uiList.Add(keyUI);

        }
    }

    public void CheckForDuplicates(KeyCode _previous, KeyCode _next)
    {
        foreach(KeybindRemapUI ui in uiList)
        {
            ui.DuplicateCheck(_previous, _next);
        }
    }

    public void SaveKeybinds()
    {

        for(int i = 0; i < keybinds.keybinds.Length; ++i)
        {
            uiList[i].SetKeybind(ref keybinds.keybinds[i]);
        }

        GameManager.Instance.SetKeybinds(keybinds);
    }

    public void ResetKeybinds()
    {
        keybinds = GameManager.Instance.DefaultKeybinds;
        for (int i = 0; i < keybinds.keybinds.Length; ++i)
        {
            uiList[i].Initialise(keybinds.keybinds[i], this);
        }
    }
}
