using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityControllerPotato : AbilityControllerBase
{
    // Block
    public override void UseAbility1() {
        // TODO: Implement Me
    } 
    // ???
    public override void UseAbility2() {
        // TODO: Implement Me
        PlayerController player = GetPlayerController();
        Vector2 cursorPos = player.GetProjectedCursorPosition();        
        AbilityManager.Instance.PerformCircularAttack(player.PlayerIndex, cursorPos, 1.0f);
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
