using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/Stunned")]
public class StunState : AbilityState
{
    [SerializeField] private float duration = .5f;

    public override void OnEnter(Player _player)
    {
        _player.StateTimer(duration);
        _player.Animator.SetTrigger("Stunned");
        _player.Rigid.velocity = Vector3.zero;
        _player.Rigid.AddExplosionForce(10, _player.transform.position + _player.transform.forward, 2, 1, ForceMode.VelocityChange);
    }

    public override void OnUpdate(Player _player) { }

    public override void OnFixedUpdate(Player _player) { }
    public override void OnTimer(Player _player)
    {
        ChangeState(_player, typeof(DefaultState));
    }
}