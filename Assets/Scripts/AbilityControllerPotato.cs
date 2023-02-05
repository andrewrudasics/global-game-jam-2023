using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityControllerPotato : AbilityControllerBase
{
    private float BlockTimeS = 1.0f;
    private float blockTimeLeftS;

    // Block
    public override void UseAbility1() {
        PlayerController player = GetPlayerController();
        if (abilityRecoveryTimes[0] > 0 || player.IsUsingAbility) {
            return;
        }
        abilityRecoveryTimes[0] = abilityCooldownsS[0];

        GetAnimator().SetBool("blocking", true);
        player.IsBlocking = true;
        blockTimeLeftS = BlockTimeS;
    } 
    // Lunge
    public override void UseAbility2() {
        PlayerController player = GetPlayerController();
        if (abilityRecoveryTimes[1] > 0 || player.IsUsingAbility) {
            return;
        }
        abilityRecoveryTimes[1] = abilityCooldownsS[1];

        GetAnimator().SetBool("lunging", true);
        player.IsRooted = true;
        player.IsUsingAbility = true;

        // Basically send a projectile where the potato itself is the projectile
        Vector2 cursorPosProjected = player.GetProjectedCursorPosition();
        Vector2 playerPosition2D = new Vector2(transform.position.x, transform.position.z);
        Vector2 aimDirection = (cursorPosProjected - playerPosition2D).normalized; 
        Transform playerTransform = transform.parent; // Player object
        GameObject potatoSprite = GetAnimator().gameObject;
        GameObject attackObject = AbilityManager.Instance.PerformProjectileAttack(player.PlayerIndex, playerPosition2D, aimDirection, 0.4f, 15, 0.5f, 0.015f, (GameObject hitPlayer) => {
            if (hitPlayer) {
                hitPlayer.GetComponent<PlayerController>().SetSlowStatus(1);
            }
            playerTransform.position = potatoSprite.transform.parent.position;
            potatoSprite.transform.SetParent(transform, false);
            GetAnimator().SetBool("lunging", false);
            player.IsRooted = false;
            player.IsUsingAbility = false;
        });
        potatoSprite.transform.SetParent(attackObject.transform, false);
    }
    // Leap
    public override void UseAbility3() {
        PlayerController player = GetPlayerController();
        if (abilityRecoveryTimes[2] > 0 || player.IsUsingAbility) {
            return;
        }
        abilityRecoveryTimes[2] = abilityCooldownsS[2];

        // TODO: Implement Me
    }
    // Shockwave
    public override void UseAbility4() {
        PlayerController player = GetPlayerController();
        if (abilityRecoveryTimes[3] > 0 || player.IsUsingAbility) {
            return;
        }
        abilityRecoveryTimes[3] = abilityCooldownsS[3];

        // TODO: Implement Me
    }

    void Start() {
        abilityCooldownsS[0] = 4.0f;
        abilityCooldownsS[1] = 6.0f;
        abilityCooldownsS[2] = 2.0f;
        abilityCooldownsS[3] = 2.0f;
    }

    protected override void Update() {
        base.Update();
        
        if (blockTimeLeftS > 0) {
            blockTimeLeftS -= Time.deltaTime;
            if (blockTimeLeftS < 0) {
                GetAnimator().SetBool("blocking", false);
                GetPlayerController().IsBlocking = false;
            }
        }
    }
}
