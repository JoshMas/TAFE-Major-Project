using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityState : ScriptableObject
{
    [SerializeField] protected AbilityState[] transitions;
    [SerializeField] protected float moveSpeedModifier = 1;
    [SerializeField] protected float gravityScale = 1;

    public virtual void OnEnter(Player _player)
    {
        //Debug.Log(this);
        if(_player.exactMovementVector.magnitude > 0)
        {
            _player.transform.forward = _player.exactMovementVector;
        }
    }

    public virtual void OnUpdate(Player _player)
    {
        _player.TurnInMovementDirection();
    }
    public virtual void OnFixedUpdate(Player _player)
    {
        float gravity = _player.gravity * gravityScale * _player.dynamicGravityMultiplier;
        if (_player.sliding)
        {
            Vector3 slopeForward = _player.GroundNormal;
            slopeForward.y = 0;
            float angle = Vector3.Angle(_player.GroundNormal, Vector3.up);
            if (Vector3.Angle(_player.movementVector, slopeForward) > 90)
            {
                _player.movementVector = Vector3.ProjectOnPlane(_player.movementVector, slopeForward);
            }
            gravity /= Mathf.Sin(angle * Mathf.Deg2Rad);
            gravity = Mathf.Clamp(gravity, float.MinValue, 0);
        }
        //Debug.Log(_player.GroundNormal + ", " + gravity);
        Vector3 xzPlaneVector = _player.movementVector;
        xzPlaneVector *= moveSpeedModifier;


        xzPlaneVector.y = _player.Rigid.velocity.y;
        _player.Rigid.velocity = xzPlaneVector;
        _player.Rigid.AddForce(gravity * Vector3.up, ForceMode.Acceleration);

    }
    public virtual void OnExit(Player _player) { }

    public virtual void OnMove(Player _player) { }
    public virtual void OnJump(Player _player) { }
    public virtual void OnJumpRelease(Player _player) { }
    public virtual void OnDash(Player _player)
    {
        _player.shouldSprint = true;
    }
    public virtual void OnDashRelease(Player _player)
    {
        _player.shouldSprint = false;
    }
    public virtual void OnLightAttack(Player _player) { }
    public virtual void OnLightRelease(Player _player) { }
    public virtual void OnHeavyAttack(Player _player) { }
    public virtual void OnHeavyRelease(Player _player) { }
    public virtual void OnChargeAttack(Player _player) { }
    public virtual void OnHitDealt(Player _player) { }
    //public virtual void OnHitTaken(Player _player) { }
    public virtual void OnTimer(Player _player) { }
    public void ChangeState(Player _player, Type _t)
    {
        foreach(AbilityState transition in transitions)
        {
            if(transition.GetType() == _t)
            {
                _player.ChangeState(transition);
                return;
            }
        }
    }
    public void ChangeState(Player _player, Type _t, int _num)
    {
        int counter = _num;
        foreach(AbilityState transition in transitions)
        {
            if(transition.GetType() == _t)
            {
                if(counter <= 0)
                {
                    _player.ChangeState(transition);
                    return;
                }
                else
                {
                    counter--;
                }
            }
        }
    }

}
