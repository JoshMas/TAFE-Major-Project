using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PlayerStates/Default")]
public class DefaultState : AbilityState
{
    [SerializeField] private float initialJumpForce = 5;
    [SerializeField] private float continuousJumpForce = 5;
    [SerializeField] private float jumpDuration = 1;

    public override void OnJump(Player _player)
    {
        _player.Jump(initialJumpForce, continuousJumpForce, jumpDuration);
    }

    public override void OnDash(Player _player)
    {
        if (_player.canDash)
        {
            _player.canDash = false;
            ChangeState(_player, typeof(DashState));
        }
    }

    public override void OnLightAttack(Player _player)
    {
        ChangeState(_player, typeof(AttackState));
    }

    public override void OnHeavyAttack(Player _player)
    {
        ChangeState(_player, typeof(AttackState), 1);
    }
}
