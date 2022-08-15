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
        isRebinding = true;
        StartCoroutine(nameof(GetInput));
    }

    private void RebindInput(KeyCode _key)
    {
        keybind.intended = _key;
        keybindText.text = keybind.intended.ToString();
        isRebinding = false;
    }

    public void RebindAltInput()
    {
        isAltRebinding = true;
        StartCoroutine(nameof(GetInput));
    }

    public void RebindAltInput(KeyCode _key)
    {
        keybind.alternative = _key;
        altKeybindtext.text = keybind.alternative.ToString();
        isAltRebinding = false;
    }

    public IEnumerator GetInput()
    {
        bool keyPressed = false;
        KeyCode key = KeyCode.None;
        while (!keyPressed)
        {
            yield return null;
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(vKey))
                {
                    key = vKey;
                    keyPressed = true;
                    break;
                }
            }
        }

        if (isRebinding)
        {
            RebindInput(key);
        }
        else if (isAltRebinding)
        {
            RebindAltInput(key);
        }
    }
}
