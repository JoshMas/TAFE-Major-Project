using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/Attack")]
public class AttackState : AbilityState
{
    [SerializeField] private float jump = 0;
    [SerializeField] private float pogoStrength;
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashDuration;
    private float dashSpeed;

    private void OnValidate()
    {
        dashSpeed = dashDistance / dashDuration;
    }

    public override void OnEnter(Player _player)
    {
        base.OnEnter(_player);
        _player.Animator.SetTrigger("Light");
        _player.SetUpwardForce(jump);
    }

    public override void OnUpdate(Player _player)
    {
        if (_player.timingWindowAnim && _player.timingWindowValid)
        {
            ChangeState(_player, typeof(AttackState));
        }
        _player.attackTimer += Time.deltaTime;
    }

    public override void OnFixedUpdate(Player _player)
    {
        if (_player.attackTimer < dashDuration)
            return;
        base.OnFixedUpdate(_player);
    }

    public override void OnHitDealt(Player _player)
    {
        _player.SetUpwardForce(pogoStrength);
        _player.AirReset();
    }

    public override void OnLightAttack(Player _player)
    {
        _player.timingWindowValid = true;
    }

    public override void OnExit(Player _player)
    {
        _player.timingWindowValid = false;
        _player.attackTimer = 0;
    }
}
