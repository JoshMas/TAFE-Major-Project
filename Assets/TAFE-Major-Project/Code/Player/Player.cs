using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        health.healthEmpty += Die;
        health.hitTaken += GetHit;
    }

    private void OnDisable()
    {
        health.healthEmpty -= Die;
        health.hitTaken -= GetHit;
    }

    private void Start()
    {
        camRotationTransform.parent = null;
        Camera.main.transform.parent = null;

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

        Vector3 camPosition = camTargetTransform.position;
        Vector3 playerPos = transform.position + Vector3.up;
        if (Physics.Linecast(playerPos, camTargetTransform.position, out RaycastHit hit, groundMask))
        {
            camPosition = hit.point;
        }


        Camera.main.transform.position = camPosition;
        Camera.main.transform.rotation = camTargetTransform.rotation;
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
        StartCoroutine(nameof(Died));
    }

    private IEnumerator Died()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
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
