using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/Attack")]
public class AttackState : AbilityState
{
    [SerializeField] private float jump = 0;
    [SerializeField] private float pogoStrength;

    [Space]

    [SerializeField] private bool heavy;
    [SerializeField] private float heavyPogo;

    public override void OnEnter(Player _player)
    {
        _player.Animator.SetTrigger("Light");
        _player.SetUpwardForce(jump);
        //_player.TimeState(attackClip.length, typeof(DefaultState));
    }

    public override void OnUpdate(Player _player) { }

    public override void OnHitDealt(Player _player)
    {
        _player.SetUpwardForce(pogoStrength);
        _player.timingWindowValid = true;
        _player.AirReset();
    }

    public override void OnLightAttack(Player _player)
    {
        if(_player.timingWindowAnim)
        {
            ChangeState(_player, typeof(AttackState));
        }
    }

    public override void OnExit(Player _player)
    {
        _player.timingWindowValid = false;
    }
}
