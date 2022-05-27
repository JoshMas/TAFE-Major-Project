using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/Stunned")]
public class StunState : AbilityState
{
    [SerializeField] private float duration = .5f;

    public override void OnEnter(Player _player)
    {
        _player.TimeState(duration, typeof(DefaultState));
        _player.Animator.SetTrigger("Stunned");
    }

    public override void OnUpdate(Player _player) { }

    public override void OnFixedUpdate(Player _player) { }
}
