using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Queue<InputEnum[]> inputBuffer;
    [SerializeField] private float inputTime = 1;

    [Space]
    [SerializeField] private InputKeyDouble jumpKey;
    [SerializeField] private InputKeyDouble dashKey;
    [SerializeField] private InputKeyDouble lightKey;
    [SerializeField] private InputKeyDouble heavyKey;
    [SerializeField] private InputKeyDouble pauseKey;

    [Space]
    [SerializeField] private InputKeyDouble leftKey;
    [SerializeField] private InputKeyDouble rightKey;
    [SerializeField] private InputKeyDouble upKey;
    [SerializeField] private InputKeyDouble downKey;

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
        GameManager.Instance.HasLost += DisableInput;
        GameManager.Instance.HasWon += DisableInput;
    }

    private void OnDisable()
    {
        GameManager.Instance.IsPaused -= EnableInput;
        GameManager.Instance.HasLost -= DisableInput;
        GameManager.Instance.HasWon -= DisableInput;
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

    private void DisableInput()
    {
        inputEnabled = false;
    }

    private InputEnum[] RecordInputs()
    {
        List<InputEnum> currentInputs = new List<InputEnum>();
        float xVal = Input.GetAxis("Horizontal");
        float yVal = Input.GetAxis("Vertical");
        if (xVal > 0)
            currentInputs.Add(InputEnum.Right);
        else if (xVal < 0)
            currentInputs.Add(InputEnum.Left);

        if (yVal > 0)
            currentInputs.Add(InputEnum.Forward);
        else if (yVal < 0)
            currentInputs.Add(InputEnum.Back);

        if (jumpKey.KeyPressed())
            currentInputs.Add(InputEnum.Jump);
        if (dashKey.KeyPressed())
            currentInputs.Add(InputEnum.Dash);
        if (lightKey.KeyPressed())
            currentInputs.Add(InputEnum.Light);
        if (heavyKey.KeyPressed())
            currentInputs.Add(InputEnum.Heavy);
        if (pauseKey.KeyPressed())
            currentInputs.Add(InputEnum.Pause);

        return currentInputs.ToArray();
    }
}
