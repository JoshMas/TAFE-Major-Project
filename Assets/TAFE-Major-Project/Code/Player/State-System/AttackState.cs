using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/Attack")]
public class AttackState : AbilityState
{
    [SerializeField] private string attackType;
    [SerializeField] private AnimationClip attackClip;
    [SerializeField] private float jump = 0;
    [SerializeField] private float pogoStrength;

    [Space]

    [SerializeField] private bool heavy;
    [SerializeField] private float heavyPogo;

    public override void OnEnter(Player _player)
    {
        _player.Animator.SetTrigger(attackType);
        _player.SetUpwardForce(jump);
        _player.TimeState(attackClip.length, typeof(DefaultState));
    }

    public override void OnUpdate(Player _player) { }

    public override void OnHitDealt(Player _player)
    {
        _player.SetUpwardForce(pogoStrength);
        _player.timingWindowValid = true;
        _player.AirReset();

        if (!heavy)
            return;
        if (_player.timingWindowValid && _player.timingWindowValid2)
        {
            _player.SetUpwardForce(heavyPogo);
        }
    }

    public override void OnLightAttack(Player _player)
    {
        if (heavy)
            return;
        if(_player.timingWindowAnim)
        {
            _player.StopTimeState();
            ChangeState(_player, typeof(AttackState));
        }
    }

    public override void OnHeavyRelease(Player _player)
    {
        if (!heavy)
            return;

        if (_player.timingWindowAnim)
        {
            _player.timingWindowValid2 = true;
        }
        if(_player.timingWindowValid && _player.timingWindowValid2)
        {
            _player.SetUpwardForce(heavyPogo);
        }
    }

    public override void OnExit(Player _player)
    {
        _player.timingWindowValid = false;
        _player.timingWindowValid2 = false;
    }
}
