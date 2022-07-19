using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/Heavy Attack")]
public class HeavyAttackState : AbilityState
{
    [SerializeField] private float jump = 4;
    [SerializeField] private float pogoStrength = 15;

    public override void OnEnter(Player _player)
    {
        base.OnEnter(_player);
        _player.Animator.SetTrigger("Heavy");
        _player.SetUpwardForce(jump);
    }

    public override void OnUpdate(Player _player) { }

    public override void OnHitDealt(Player _player)
    {
        _player.timingWindowValid = true;
        _player.AirReset();
        Bounce(_player);
    }

    public override void OnHeavyAttack(Player _player)
    {
        _player.timingWindowInvalid = true;
    }

    public override void OnHeavyRelease(Player _player)
    {
        if (_player.timingWindowAnim)
        {
            _player.timingWindowValid2 = true;
        }
        Bounce(_player);
    }

    private void Bounce(Player _player)
    {
        if (_player.timingWindowValid && _player.timingWindowValid2 && !_player.timingWindowInvalid)
        {
            _player.SetUpwardForce(pogoStrength);
            _player.timingWindowInvalid = true;
        }
    }

    public override void OnExit(Player _player)
    {
        _player.timingWindowValid = false;
        _player.timingWindowValid2 = false;
        _player.timingWindowInvalid = false;
    }
}
