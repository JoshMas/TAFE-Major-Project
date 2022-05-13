using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(AudioSource))]
public class Player : MonoBehaviour
{
    private Rigidbody rigid;
    public Rigidbody Rigid => rigid;
    private Animator animator;
    public Animator Animator => animator;
    private AudioSource audioSrc;
    public AudioSource AudioSrc => audioSrc;

    [SerializeField] private AbilityState currentState;
    [SerializeField] private Transform camRotationTransform;
    [SerializeField] private Transform camHeightTransform;
    [SerializeField] private Transform cameraTransform;

    public Transform CameraForward
    {
        get
        {
            return camRotationTransform;
        }
    }

    public Vector3 movementVector;
    public float cameraX;
    public float cameraY;
    public float gravity = -9.8f;

    private bool grounded = true;
    private bool canDoubleJump = true;
    public bool canDash = true;
    [SerializeField] private LayerMask groundMask;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();

        rigid.useGravity = false;

    }

    private void Start()
    {
        camRotationTransform.parent = null;

        cameraY = camRotationTransform.eulerAngles.y;
        cameraX = camHeightTransform.eulerAngles.x;

        camHeightTransform.localEulerAngles = new Vector3(cameraX, 0, 0);
        camRotationTransform.eulerAngles = new Vector3(0, cameraY, 0);
        
    }

    public void ChangeState(AbilityState _newState)
    {
        currentState.OnExit(this);
        currentState = _newState;
        currentState.OnEnter(this);
    }

    private void Update()
    {
        currentState.OnUpdate(this);

        camRotationTransform.position = transform.position;
        camHeightTransform.localEulerAngles = new Vector3(cameraX, 0, 0);
        camRotationTransform.eulerAngles = new Vector3(0, cameraY, 0);
        
        grounded = Physics.CheckBox(transform.position, new Vector3(.5f, .1f, .5f), transform.rotation, groundMask);
        if (grounded)
        {
            AirReset();
        }
    }

    private void FixedUpdate()
    {
        currentState.OnFixedUpdate(this);
    }

    public void TurnInMovementDirection()
    {
        if(movementVector.magnitude > 0)
        {
            transform.forward = Vector3.Slerp(transform.forward, movementVector, .5f);
        }
    }

    public void PerformMove(string _move)
    {
        switch (_move)
        {
            case "Test":
                Debug.Log("test successful");
                break;
            case "Jump":
                currentState.OnJump(this);
                break;
            case "Dash":
                currentState.OnDash(this);
                break;
            case "Light":
                currentState.OnLightAttack(this);
                break;
            case "Heavy":
                currentState.OnHeavyAttack(this);
                break;
            default:
                break;
        }
    }

    public void Jump(float _jumpForce)
    {
        if (grounded)
        {
            rigid.velocity = new Vector3(rigid.velocity.x, _jumpForce, rigid.velocity.z);
        }
        else if (canDoubleJump)
        {
            rigid.velocity = new Vector3(rigid.velocity.x, _jumpForce, rigid.velocity.z);
            canDoubleJump = false;
        }
    }

    public void AirReset()
    {
        canDoubleJump = true;
        canDash = true;
    }

    public void SetUpwardForce(float _force)
    {
        rigid.velocity = new Vector3(rigid.velocity.x, _force, rigid.velocity.z);
    }

    public void TimeState(float _duration, Type _t)
    {
        IEnumerator coroutine = TimeCoroutine(_duration, _t);
        StartCoroutine(coroutine);
    }

    private IEnumerator TimeCoroutine(float _duration, Type _t)
    {
        yield return new WaitForSeconds(_duration);
        currentState.ChangeState(this, _t);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            currentState.OnHitDealt(this);
        }
        else
        {
            currentState.OnHitTaken(this);
        }
    }
}
