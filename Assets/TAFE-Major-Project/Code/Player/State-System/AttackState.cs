using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/Attack")]
public class AttackState : AbilityState
{
    [SerializeField] private string attackType;
    [SerializeField] private AnimationClip attackClip;
    [SerializeField] private float pogoStrength;
    public override void OnEnter(Player _player)
    {
        _player.Animator.SetTrigger(attackType);
        _player.TimeState(attackClip.length, typeof(DefaultState));
    }

    public override void OnHitDealt(Player _player)
    {
        _player.SetUpwardForce(pogoStrength);
        _player.AirReset();
    }
}
