using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class Player : MonoBehaviour
{
    private Rigidbody rigid;
    public Rigidbody Rigid => rigid;
    private Animator animator;
    public Animator Animator => animator;
    private AudioSource audioSrc;
    public AudioSource AudioSrc => audioSrc;
    [HideInInspector] public Health health;

    [SerializeField] private Transform camRotationTransform;
    [SerializeField] private Transform camHeightTransform;
    [SerializeField] private Transform camTargetTransform;
    [SerializeField] private float cameraSpeed = 10;
    [SerializeField] private float cameraCheckLength = 4;
    private Vector3 originalCameraPosition;

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

    public Vector3 exactMovementVector;
    public Vector3 movementVector;
    private float cameraX;
    private float cameraY;
    public float gravity = -9.8f;
    public float dynamicGravityMultiplier = 1;

    [SerializeField] private AbilityState currentState;
    private bool grounded = true;
    private readonly float coyoteTime = 0.1f;
    private float coyoteTimer = 0;
    [HideInInspector] public bool sliding = false;
    private bool canDoubleJump = true;
    [HideInInspector] public bool canDash = true;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float steepestWalkableAngle = 45;
    [HideInInspector] public bool shouldSprint = false;

    private float chargeLevel = 0;
    [SerializeField] private float maxCharge = 1;

    /// <summary>
    /// The amount of damage the player does with attacks
    /// This variable is only changed in animations
    /// </summary>
    [SerializeField] private float damage;
    [SerializeField] private AbilityState[] universalStates;

    private IEnumerator groundCheck;
    public Vector3 GroundNormal { get; private set; }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        audioSrc = GetComponent<AudioSource>();
        health = GetComponent<Health>();
        rigid.useGravity = false;
        originalCameraPosition = camTargetTransform.localPosition;
        groundCheck = GroundCheck();
    }

    private void OnEnable()
    {
        health.HealthIsEmpty += Die;
        health.HitIsTaken += GetHit;
        StartCoroutine(groundCheck);
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


        animator.SetFloat("Grounded", grounded ? 1 : 0);
        if (grounded)
        {
            AirReset();
        }

        movementVector = Vector3.MoveTowards(movementVector, exactMovementVector, 3 * Time.deltaTime);
        if (movementVector.magnitude < 0.01)
        {
            movementVector = Vector3.zero;
        }
    }

    private IEnumerator GroundCheck()
    {
        while (gameObject.activeSelf)
        {
            yield return null;
            Ray raycast = new Ray(transform.position + Vector3.up, Vector3.down);
            RaycastHit[] hits = Physics.SphereCastAll(raycast, 0.5f, 0.55f, groundMask);

            coyoteTimer += Time.deltaTime;
            if(coyoteTimer >= coyoteTime)
            {
                grounded = false;
            }
            sliding = false;

            if (hits.Length == 0)
            {
                continue;
            }
            else
            {
                GroundNormal = hits[0].normal;
                foreach(RaycastHit hit in hits)
                {
                    if(hit.distance == 0)
                    {
                        continue;
                    }
                    float angle = Vector3.Angle(hit.normal, Vector3.up);
                    if (angle < steepestWalkableAngle)
                    {
                        grounded = true;
                        coyoteTimer = 0;
                        sliding = false;
                        GroundNormal = hit.normal;
                        break;
                    }
                    else
                    {
                        sliding = true;
                    }
                }

            }
        }
    }

    public void SetCameraValues(float _cameraX, float _cameraY)
    {
        cameraX -= _cameraX;
        cameraY += _cameraY;
        cameraX = Mathf.Clamp(cameraX, -90, 90);
    }

    private void LateUpdate()
    {
        camRotationTransform.position = transform.position;
        camHeightTransform.localEulerAngles = new Vector3(cameraX, 0, 0);
        camRotationTransform.eulerAngles = new Vector3(0, cameraY, 0);

        //Vector3 camPosition;
        Vector3 playerPos = transform.position + Vector3.up * 2;

        //Vector3 localCamPosition = camTargetTransform.InverseTransformPoint(Camera.main.transform.position);

        if (Physics.Linecast(playerPos, camHeightTransform.TransformPoint(originalCameraPosition) - camRotationTransform.right * cameraCheckLength, out RaycastHit hitL, groundMask) 
            && Physics.Linecast(playerPos, camHeightTransform.TransformPoint(originalCameraPosition) + camRotationTransform.right * cameraCheckLength, out RaycastHit hitR, groundMask))
        {
            Vector3 hitPosition = (hitL.point + hitR.point) / 2 + Vector3.up * .5f;
            if (Vector3.Distance(camTargetTransform.position, hitPosition) < 0.1f)
            {
                camTargetTransform.position = hitPosition;
            }
            else
            {
                camTargetTransform.position = Vector3.Lerp(camTargetTransform.position, hitPosition, cameraSpeed * Time.deltaTime);
            }
            //Vector3 localHitPosition = camTargetTransform.InverseTransformPoint(hitPosition);
            ////float distance = Vector3.Distance(hitPosition, camPosition);
            //Vector3 lerp = Vector3.Lerp(localCamPosition, localHitPosition, cameraSpeed * Time.deltaTime);
            //camPosition = camTargetTransform.TransformPoint(new Vector3(0, 0, lerp.z));//camTargetTransform.TransformDirection(Vector3.back * distance);
        }
        else
        {
            if (Vector3.Distance(camTargetTransform.localPosition, originalCameraPosition) < 0.1f)
            {
                camTargetTransform.localPosition = originalCameraPosition;
            }
            else
            {
                camTargetTransform.localPosition = Vector3.Lerp(camTargetTransform.localPosition, originalCameraPosition, cameraSpeed * Time.deltaTime);
            }
            //Vector3 lerp = Vector3.Lerp(localCamPosition, camTargetTransform.localPosition, cameraSpeed * Time.deltaTime);
            //camPosition = camTargetTransform.TransformPoint(new Vector3(0, 0, lerp.z));//camTargetTransform.TransformDirection(Vector3.back * distance);
        }

        Camera.main.transform.position = camTargetTransform.position;
        Camera.main.transform.rotation = camTargetTransform.rotation;
    }

    private void FixedUpdate()
    {
        currentState.OnFixedUpdate(this);
    }

    private void SetUniversalState(Type _t)
    {
        foreach (AbilityState state in universalStates)
        {
            if (state.GetType() == _t)
            {
                ChangeState(state);
                return;
            }
        }
    }

    public void TurnInMovementDirection()
    {
        if(movementVector.magnitude > 0)
        {
            Vector3 newForward = Vector3.Slerp(transform.forward, movementVector.normalized, 5 * Time.deltaTime);
            animator.SetFloat("TurnSpeed", Vector3.SignedAngle(newForward, transform.forward, Vector3.up));
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
            case "Jump":
                currentState.OnJump(this);
                break;
            case "JumpRelease":
                currentState.OnJumpRelease(this);
                break;
            case "Dash":
                currentState.OnDash(this);
                break;
            case "DashRelease":
                currentState.OnDashRelease(this);
                break;
            case "Charge":
                currentState.OnChargeAttack(this);
                break;
            case "LightRelease":
                currentState.OnLightRelease(this);
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
                GameManager.Instance.Pause(true);
                break;
            default:
                break;
        }
    }

    private void SetStateToDefaultFromAnimation()
    {
        SetUniversalState(typeof(DefaultState));
    }

    private void GetHit()
    {
        SetUniversalState(typeof(StunState));
    }

    private void Die()
    {
        Debug.Log("uh oh");
        GameManager.Instance.Lose();
        enabled = false;
    }

    public void Jump(float _initialJumpForce, float _timeToReachPeak)
    {
        if (grounded)
        {
            rigid.velocity = new Vector3(rigid.velocity.x, _initialJumpForce, rigid.velocity.z);
            Invoke(nameof(ResetDynamicGravity), _timeToReachPeak);
        }
        else if (canDoubleJump)
        {
            rigid.velocity = new Vector3(rigid.velocity.x, _initialJumpForce, rigid.velocity.z);
            canDoubleJump = false;
            Invoke(nameof(ResetDynamicGravity), _timeToReachPeak);
        }
    }

    private void ResetDynamicGravity()
    {
        dynamicGravityMultiplier = 1;
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

    public void ChargeAttack(float _chargeRate)
    {
        chargeLevel += _chargeRate * Time.deltaTime * (grounded ? 1 : 2);
        if(chargeLevel > maxCharge)
        {
            chargeLevel = maxCharge;
        }
    }

    public void ReleaseCharge(bool _attack)
    {
        if(_attack)
            animator.SetFloat("ChargeLevel", chargeLevel / maxCharge);
        chargeLevel = 0;
    }

    public void StateTimer(float _duration)
    {
        IEnumerator coroutine = TimeCoroutine(_duration);
        StartCoroutine(coroutine);
    }

    private IEnumerator TimeCoroutine(float _duration)
    {
        yield return new WaitForSeconds(_duration);
        currentState.OnTimer(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            currentState.OnHitDealt(this);
            if (other.transform.root.TryGetComponent(out Health health))
            {
                health .UpdateHealth(-damage);
            }
            //if (other.transform.parent == null)
            //{
            //    health = other.GetComponent<Health>();
            //}
            //else
            //{
            //    health = other.GetComponentInParent<Health>();
            //}
        }
        else
        {
            currentState.OnHitTaken(this);
           health.UpdateHealth(-1);
        }
    }
}
