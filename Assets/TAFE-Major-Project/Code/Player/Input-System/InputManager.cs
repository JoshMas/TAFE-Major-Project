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

    private Vector2 exactMovementAxis;
    private Vector2 lerpMovementAxis;

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
        foreach(MoveInput move in inputList)
        {
            if (move.Check(inputBuffer))
            {
                move.Activate(player);
                inputBuffer.Clear();
            }
        }

        player.exactMovementVector = player.CameraForward.TransformDirection(Vector3.ClampMagnitude(new Vector3(exactMovementAxis.x, 0, exactMovementAxis.y), 1));
        player.movementVector = player.CameraForward.TransformDirection(Vector3.ClampMagnitude(new Vector3(lerpMovementAxis.x, 0, lerpMovementAxis.y), 1));
        player.cameraX -= Input.GetAxis("Mouse Y") * mouseSensitivity.x;
        player.cameraY += Input.GetAxis("Mouse X") * mouseSensitivity.y;
        player.cameraX = Mathf.Clamp(player.cameraX, -90, 90);
    }

    private InputEnum[] RecordInputs()
    {
        List<InputEnum> currentInputs = new List<InputEnum>();
        exactMovementAxis = Vector2.zero;

        if (rightKey.KeyPressed())
        {
            currentInputs.Add(InputEnum.Right);
            exactMovementAxis += Vector2.right;
        }
        if (leftKey.KeyPressed())
        {
            currentInputs.Add(InputEnum.Left);
            exactMovementAxis += Vector2.left;
        }
        if (upKey.KeyPressed())
        {
            currentInputs.Add(InputEnum.Forward);
            exactMovementAxis += Vector2.up;
        }
        if (downKey.KeyPressed())
        {
            currentInputs.Add(InputEnum.Back);
            exactMovementAxis += Vector2.down;
        }

        lerpMovementAxis = Vector2.MoveTowards(lerpMovementAxis, exactMovementAxis, 3 * Time.deltaTime);
        if(lerpMovementAxis.magnitude < 0.01)
        {
            lerpMovementAxis = Vector2.zero;
        }

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
