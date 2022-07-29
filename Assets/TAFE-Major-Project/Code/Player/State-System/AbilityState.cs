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
        if (_player.sliding)
        {

        }

        Vector3 xzPlaneForce = new Vector3(_player.movementVector.x, 0, _player.movementVector.z) * moveSpeedModifier;
        _player.Rigid.velocity = xzPlaneForce + Vector3.up * _player.Rigid.velocity.y;
        _player.Rigid.AddForce(_player.gravity * gravityScale * _player.dynamicGravityMultiplier * Vector3.up, ForceMode.Acceleration);
    }
    public virtual void OnExit(Player _player) { }

    public virtual void OnMove(Player _player) { }
    public virtual void OnJump(Player _player) { }
    public virtual void OnJumpRelease(Player _player) { }
    public virtual void OnDash(Player _player) { }
    public virtual void OnLightAttack(Player _player) { }
    public virtual void OnHeavyAttack(Player _player) { }

    public virtual void OnHeavyRelease(Player _player) { }

    public virtual void OnHitDealt(Player _player) { }
    public virtual void OnHitTaken(Player _player) { }

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
