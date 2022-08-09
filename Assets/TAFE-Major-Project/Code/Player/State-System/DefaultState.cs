using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PlayerStates/Default")]
public class DefaultState : AbilityState
{
    [SerializeField] private float jumpHeight;
    [SerializeField] private bool sprint;

    public override void OnEnter(Player _player) { }

    public override void OnJump(Player _player)
    {
        _player.dynamicGravityMultiplier = 0.5f;
        float gravity = _player.gravity * _player.dynamicGravityMultiplier * gravityScale;
        _player.Jump(Mathf.Sqrt(4 * -gravity * jumpHeight), Mathf.Sqrt(-jumpHeight/gravity));
    }

    public override void OnJumpRelease(Player _player)
    {
        _player.dynamicGravityMultiplier = 1;
    }

    public override void OnUpdate(Player _player)
    {
        base.OnUpdate(_player);
        _player.Animator.SetFloat("MoveSpeed", _player.movementVector.magnitude);
        if(_player.shouldSprint != sprint)
        {
            ChangeState(_player, typeof(DefaultState));
        }
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

    public override void OnChargeAttack(Player _player)
    {
        ChangeState(_player, typeof(ChargeAtkState));
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
