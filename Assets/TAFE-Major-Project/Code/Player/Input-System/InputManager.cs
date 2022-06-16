using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Queue<InputEnum[]> inputBuffer;
    [SerializeField] private float inputTime = 1;

    [Space]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode lightKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode heavyKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

    private Player player;
    private bool inputEnabled;

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
        player = GetComponent<Player>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputEnabled = true;
    }

    private void OnEnable()
    {
        GameManager.Instance.IsPaused += EnableInput;
    }

    private void OnDisable()
    {
        GameManager.Instance.IsPaused -= EnableInput;
    }

    private void Update()
    {
        if (!inputEnabled)
            return;

        player.movementVector = player.CameraForward.TransformDirection(Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), 1));
        player.cameraX -= Input.GetAxis("Mouse Y") * mouseSensitivity.x;
        player.cameraY += Input.GetAxis("Mouse X") * mouseSensitivity.y;
        player.cameraX = Mathf.Clamp(player.cameraX, -90, 90);

        inputBuffer.Enqueue(RecordInputs());
        if (inputBuffer.Count > InputBufferLength)
            inputBuffer.Dequeue();
        foreach(MoveInput move in inputList)
        {
            if (move.Check(inputBuffer))
            {
                move.Activate(player);
                inputBuffer.Clear();
            }
        }
    }

    private void EnableInput(bool _isInputPaused)
    {
        inputEnabled = !_isInputPaused;
    }

    private InputEnum[] RecordInputs()
    {
        List<InputEnum> currentInputs = new List<InputEnum>();
        float xVal = player.movementVector.x;
        float yVal = player.movementVector.y;
        if (xVal > 0)
            currentInputs.Add(InputEnum.Forward);
        else if (xVal < 0)
            currentInputs.Add(InputEnum.Backward);

        if (yVal > 0)
            currentInputs.Add(InputEnum.Up);
        else if (yVal < 0)
            currentInputs.Add(InputEnum.Down);

        if (Input.GetKey(jumpKey))
            currentInputs.Add(InputEnum.Jump);
        if (Input.GetKey(dashKey))
            currentInputs.Add(InputEnum.Dash);
        if (Input.GetKey(lightKey))
            currentInputs.Add(InputEnum.Light);
        if (Input.GetKey(heavyKey))
            currentInputs.Add(InputEnum.Heavy);
        if (Input.GetKey(pauseKey))
            currentInputs.Add(InputEnum.Pause);

        return currentInputs.ToArray();
    }
}
