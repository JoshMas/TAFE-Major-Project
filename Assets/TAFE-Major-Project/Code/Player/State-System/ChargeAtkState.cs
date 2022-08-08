using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/Charge Attack")]
public class ChargeAtkState : AbilityState
{
    [SerializeField] private float chargeRate;

    public override void OnEnter(Player _player)
    {
        base.OnEnter(_player);
        _player.Animator.SetTrigger("Charge");
        _player.SetUpwardForce(0);
    }

    public override void OnUpdate(Player _player)
    {
        base.OnUpdate(_player);
        _player.ChargeAttack(chargeRate);
    }

    public override void OnChargeRelease(Player _player)
    {
        _player.ReleaseCharge();
        ChangeState(_player, typeof(AttackState));
    }
}
