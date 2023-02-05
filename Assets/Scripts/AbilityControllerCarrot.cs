using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityControllerCarrot : AbilityControllerBase
{
    // Dodge Roll
    public override void UseAbility1() {
        if (abilityRecoveryTimes[0] > 0) {
            return;
        }
        abilityRecoveryTimes[0] = abilityCooldownsS[0];

        // TODO: Implement Me
    } 
    // Carrot Rain
    public override void UseAbility2() {
       if (abilityRecoveryTimes[1] > 0) {
            return;
        }
        abilityRecoveryTimes[1] = abilityCooldownsS[1];

        PlayerController player = GetPlayerController();
        Vector2 cursorPos = player.GetProjectedCursorPosition();        
        AbilityManager.Instance.PerformCircularAttack(player.PlayerIndex, cursorPos, 1.0f);
    }
    // ???
    public override void UseAbility3() {
        if (abilityRecoveryTimes[2] > 0) {
            return;
        }
        abilityRecoveryTimes[2] = abilityCooldownsS[2];

        // TODO: Implement Me
    }
    // ???
    public override void UseAbility4() {
        if (abilityRecoveryTimes[3] > 0) {
            return;
        }
        abilityRecoveryTimes[3] = abilityCooldownsS[3];

        // TODO: Implement Me
    }

    void Start() {
        abilityCooldownsS[0] = 2.0f;
        abilityCooldownsS[1] = 2.0f;
        abilityCooldownsS[2] = 2.0f;
        abilityCooldownsS[3] = 2.0f;
    }

    protected override void Update() {
        base.Update();
    }
}
