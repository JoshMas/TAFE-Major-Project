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
    public Health health;

    [SerializeField] private AbilityState currentState;
    [SerializeField] private Transform camRotationTransform;
    [SerializeField] private Transform camHeightTransform;
    [SerializeField] private Transform camTargetTransform;
    [SerializeField] private float cameraSpeed = 10;
    [SerializeField] private float cameraCheckLength = 4;

    public bool timingWindowAnim = false;
    [HideInInspector] public bool timingWindowValid = false;
    [HideInInspector] public bool timingWindowValid2 = false;
    [HideInInspector] public bool timingWindowInvalid = false;

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

    private IEnumerator jumpCoroutine;

    private bool grounded = true;
    private bool canDoubleJump = true;
    public bool canDash = true;
    [SerializeField] private LayerMask groundMask;

    /// <summary>
    /// The amount of damage the player does with attacks
    /// This variable is only changed in animations
    /// </summary>
    [SerializeField] private float damage;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        health = GetComponent<Health>();
        rigid.useGravity = false;
        jumpCoroutine = JumpCoroutine(0, 0);
    }

    private void OnEnable()
    {
        health.HealthIsEmpty += Die;
        health.HitIsTaken += GetHit;
    }

    private void OnDisable()
    {
        health.HealthIsEmpty -= Die;
        health.HitIsTaken -= GetHit;
    }

    private void Start()
    {
        camRotationTransform.parent = null;
        Camera.main.transform.parent = null;
        Camera.main.transform.position = camTargetTransform.position;

        cameraY = camRotationTransform.eulerAngles.y;
        cameraX = camHeightTransform.eulerAngles.x;

        camHeightTransform.localEulerAngles = new Vector3(cameraX, 0, 0);
        camRotationTransform.eulerAngles = new Vector3(0, cameraY, 0);

        GameManager.Instance.PlaceAtSpawnpoint(transform);
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
        
        grounded = Physics.CheckBox(transform.position, new Vector3(.5f, .1f, .5f), transform.rotation, groundMask);
        animator.SetFloat("Grounded", grounded ? 1 : 0);
        if (grounded)
        {
            AirReset();
        }
    }

    private void LateUpdate()
    {
        camRotationTransform.position = transform.position;
        camHeightTransform.localEulerAngles = new Vector3(cameraX, 0, 0);
        camRotationTransform.eulerAngles = new Vector3(0, cameraY, 0);

        Vector3 camPosition;
        Vector3 playerPos = transform.position + Vector3.up;

        Vector3 localCamPosition = camTargetTransform.InverseTransformPoint(Camera.main.transform.position);

        if(Physics.Linecast(playerPos - transform.right * cameraCheckLength, camTargetTransform.position - camTargetTransform.right * cameraCheckLength, out RaycastHit hitL, groundMask) 
            && Physics.Linecast(playerPos + transform.right * cameraCheckLength, camTargetTransform.position + camTargetTransform.right * cameraCheckLength, out RaycastHit hitR, groundMask))
        {
            Vector3 hitPosition = (hitL.point + hitR.point) / 2 + Vector3.up * .5f;
            Vector3 localHitPosition = camTargetTransform.InverseTransformPoint(hitPosition);
            //float distance = Vector3.Distance(hitPosition, camPosition);
            Vector3 lerp = Vector3.Lerp(localCamPosition, localHitPosition, cameraSpeed * Time.deltaTime);
            camPosition = camTargetTransform.TransformPoint(new Vector3(0, 0, lerp.z));//camTargetTransform.TransformDirection(Vector3.back * distance);
        }
        else
        {
            Vector3 lerp = Vector3.Lerp(localCamPosition, camTargetTransform.localPosition, cameraSpeed * Time.deltaTime);
            camPosition = camTargetTransform.TransformPoint(new Vector3(0, 0, lerp.z));//camTargetTransform.TransformDirection(Vector3.back * distance);
        }

        Camera.main.transform.position = camPosition;
        Camera.main.transform.rotation = camTargetTransform.rotation;

        //Vector3 camPosition = camTargetTransform.position;
        //Vector3 playerPosL = transform.position + Vector3.up + Vector3.left;
        //Vector3 playerPosR = transform.position + Vector3.up + Vector3.right;
        //if (Physics.Linecast(playerPosL, camTargetTransform.position, out RaycastHit hitL, groundMask) 
        //     && Physics.Linecast(playerPosR, camTargetTransform.position, out RaycastHit hitR, groundMask))
        //{
        //    camPosition = (hitL.point + hitR.point)/2;
        //}



        //Camera.main.transform.rotation = camTargetTransform.rotation;
        //Camera.main.transform.position = camPosition;
    }

    private void FixedUpdate()
    {
        currentState.OnFixedUpdate(this);
    }

    public void TurnInMovementDirection()
    {
        if(movementVector.magnitude > 0)
        {
            Vector3 newForward = Vector3.Slerp(transform.forward, movementVector.normalized, 5 * Time.deltaTime);
            animator.SetFloat("TurnSpeed", Vector3.Angle(newForward, transform.forward));
            transform.forward = new Vector3(newForward.x, 0, newForward.z);
        }
        else
        {
            animator.SetFloat("TurnSpeed", 0);
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
            case "Dash1":
            case "Dash2":
            case "Dash3":
            case "Dash4":
                currentState.OnDash(this);
                break;
            case "Light":
                currentState.OnLightAttack(this);
                break;
            case "Heavy":
                currentState.OnHeavyAttack(this);
                break;
            case "UnHeavy":
                currentState.OnHeavyRelease(this);
                break;
            case "Pause":
                GameManager.Instance.Pause();
                break;
            default:
                break;
        }
    }

    private void GetHit()
    {
        Debug.Log("ow");
    }

    private void Die()
    {
        Debug.Log("uh oh");
        GameManager.Instance.Lose();
    }

    public void Jump(float _initialJumpForce, float _continuousJumpForce, float _jumpDuration)
    {
        StopCoroutine(jumpCoroutine);
        jumpCoroutine = JumpCoroutine(_continuousJumpForce, _jumpDuration);
        if (grounded)
        {
            rigid.velocity = new Vector3(rigid.velocity.x, _initialJumpForce, rigid.velocity.z);
            StartCoroutine(jumpCoroutine);
        }
        else if (canDoubleJump)
        {
            rigid.velocity = new Vector3(rigid.velocity.x, _initialJumpForce, rigid.velocity.z);
            StartCoroutine(jumpCoroutine);
            canDoubleJump = false;
        }
    }

    private IEnumerator JumpCoroutine(float _force, float _duration)
    {
        float timer = 0;
        while (timer < _duration)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            rigid.AddForce(Vector3.up * _force, ForceMode.Acceleration);
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

    public void StopTimeState()
    {
        StopCoroutine(nameof(TimeCoroutine));
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
            other.GetComponent<Health>()?.UpdateHealth(-damage);
        }
        else
        {
            currentState.OnHitTaken(this);
            health.UpdateHealth(-1);
        }
    }
}
