using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/HeavyAttack")]
public class HeavyAttack : AbilityState
{
    [SerializeField] private float pogoStrength;

    public override void OnUpdate(Player _player) { }
}
