using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PlayerStates/Default")]
public class DefaultState : AbilityState
{
    [SerializeField] private float jumpHeight;


    public override void OnEnter(Player _player) { }

    public override void OnJump(Player _player)
    {
        _player.dynamicGravityMultiplier = 0.5f;
        float gravity = _player.gravity * _player.dynamicGravityMultiplier * gravityScale;
        _player.Jump(2 * Mathf.Sqrt(-gravity * jumpHeight));
    }

    public override void OnJumpRelease(Player _player)
    {
        _player.dynamicGravityMultiplier = 1;
    }

    public override void OnFixedUpdate(Player _player)
    {
        if(_player.Rigid.velocity.y < 0)
        {
            _player.dynamicGravityMultiplier = 1;
        }

        base.OnFixedUpdate(_player);
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
        ChangeState(_player, typeof(HeavyAttackState));
    }
}
