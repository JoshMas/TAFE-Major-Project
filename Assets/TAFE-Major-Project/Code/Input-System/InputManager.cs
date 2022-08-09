using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Queue<InputEnum[]> inputBuffer;
    [SerializeField] private float inputTime = 1;
    
    private KeybindObject keybinds;

    private Player player;

    private Vector2 exactMovementAxis;

    [Space]
    [SerializeField] private Vector2 mouseSensitivity = Vector2.one;

    public int InputBufferLength 
    { 
        get
        {
            return (int)(inputTime / Time.deltaTime);
        }
    }

    [SerializeField] private MoveInput[] inputList;

    private void Awake()
    {
        inputBuffer = new Queue<InputEnum[]>();
        //inputObject.SaveKeybinds();
        //inputObject.LoadKeybinds();
    }

    private void Start()
    {
        GetKeybindObject();
    }

    public void GetKeybindObject()
    {
        keybinds = GameManager.Instance.ActualKeybinds;
    }

    private void OnEnable()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        inputBuffer.Enqueue(RecordInputs());
        if (inputBuffer.Count > InputBufferLength)
            inputBuffer.Dequeue();

        bool moveActivated = false;
        foreach(MoveInput move in inputList)
        {
            if (move.Check(inputBuffer))
            {
                move.Activate(player);
                moveActivated = true;
            }
        }
        if (moveActivated)
        {
            inputBuffer.Clear();
            inputBuffer.Enqueue(RecordInputs());
        }

        player.exactMovementVector = player.CameraForward.TransformDirection(Vector3.ClampMagnitude(new Vector3(exactMovementAxis.x, 0, exactMovementAxis.y), 1));

        player.SetCameraValues(Input.GetAxis("Mouse Y") * mouseSensitivity.x, Input.GetAxis("Mouse X") * mouseSensitivity.y);
    }

    private InputEnum[] RecordInputs()
    {
        InputEnum[] currentInputs = keybinds.RecordInputs(ref exactMovementAxis);

        return currentInputs;
    }
}
