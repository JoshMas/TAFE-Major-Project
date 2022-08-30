using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/Dash")]
public class DashState : AbilityState
{
    [SerializeField] private float distance;
    [SerializeField] private float duration;

    private void OnValidate()
    {
        if (duration <= 0)
            return;
        moveSpeedModifier = distance / duration;
    }

    public override void OnEnter(Player _player)
    {
        _player.shouldSprint = true;
        Vector3 direction = _player.exactMovementVector;
        if(direction.magnitude > 0)
        {
            _player.Rigid.velocity = direction * moveSpeedModifier;
        }
        else
        {
            _player.Rigid.velocity = _player.transform.forward * moveSpeedModifier;
        }
        _player.transform.forward = _player.Rigid.velocity;
        _player.StateTimer(duration);

        _player.Animator.SetBool("Dash", true);
    }

    public override void OnJump(Player _player)
    {
        ChangeState(_player, typeof(DefaultState));
        _player.JumpFromDash();
    }

    public override void OnFixedUpdate(Player _player) { }

    public override void OnTimer(Player _player)
    {
        if (_player.shouldSprint)
        {
            ChangeState(_player, typeof(DefaultState));
        }
        else
        {
            ChangeState(_player, typeof(DefaultState), 1);
        }
    }
    public override void OnExit(Player _player)
    {
        _player.Animator.SetBool("Dash", false);
    }
}
