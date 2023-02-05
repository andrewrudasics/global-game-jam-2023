using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityControllerCarrot : AbilityControllerBase
{
    // Dodge Roll
    public override void UseAbility1() {
        // TODO: Implement Me
    } 
    // Carrot Rain
    public override void UseAbility2() {
        PlayerController player = GetPlayerController();
        Vector2 mousePosProjected = player.GetProjectedMousePosition();        
        AbilityManager.Instance.PerformCircularAttack(player.PlayerIndex, mousePosProjected, 1.0f);
    }
    // ???
    public override void UseAbility3() {
        // TODO: Implement Me
    }
    // ???
    public override void UseAbility4() {
        // TODO: Implement Me
    }

    public override float GetAbilityCooldown(int abilityIndex) {
        return 0;
    }
}
