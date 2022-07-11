using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/Dash")]
public class DashState : AbilityState
{
    [SerializeField] private float duration;

    public override void OnEnter(Player _player)
    {
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
        _player.TimeState(duration, typeof(DefaultState));

        _player.Animator.SetBool("Dash", true);
    }

    public override void OnFixedUpdate(Player _player) { }

    public override void OnExit(Player _player)
    {
        _player.Animator.SetBool("Dash", false);
    }
}
