using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class KeybindRemapUI : MonoBehaviour
{
    public delegate void KeyRebound(KeyCode oldKey, KeyCode newKey);
    public static event KeyRebound OnKeyRebind;

    private InputKeyDouble keybind;
    private bool isRebinding = false;
    private bool isAltRebinding = false;
    [SerializeField] private Text nameText;
    [SerializeField] private Text keybindText;
    [SerializeField] private Text altKeybindtext;

    public void Initialise(InputKeyDouble _keybind)
    {
        keybind = _keybind;
        nameText.text = keybind.input.ToString();
        keybindText.text = keybind.intended.ToString();
        altKeybindtext.text = keybind.alternative.ToString();
    }

    public void SetKeybind(ref InputKeyDouble _keybind)
    {
        _keybind = keybind;
    }

    public void RebindInput()
    {

    }

    public void RebindAltInput()
    {

    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
        {
            Debug.Log(e.keyCode);
        }
    }
}
