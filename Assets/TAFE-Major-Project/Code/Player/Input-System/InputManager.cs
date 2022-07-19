using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Queue<InputEnum[]> inputBuffer;
    [SerializeField] private float inputTime = 1;

    [Space]
    [SerializeField] private InputObject inputObject;

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
        //inputObject.SaveKeybinds();
        //inputObject.LoadKeybinds();
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
        InputEnum[] currentInputs = inputObject.RecordInputs(ref exactMovementAxis);

        lerpMovementAxis = Vector2.MoveTowards(lerpMovementAxis, exactMovementAxis, 3 * Time.deltaTime);
        if(lerpMovementAxis.magnitude < 0.01)
        {
            lerpMovementAxis = Vector2.zero;
        }

        return currentInputs;
    }
}
