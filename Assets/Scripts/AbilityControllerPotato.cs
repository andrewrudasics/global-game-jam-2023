using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityControllerPotato : AbilityControllerBase
{
    private float BlockTimeS = 1.0f;
    private float blockTimeLeftS;

    // Block
    public override void UseAbility1() {
        Debug.Log(abilityRecoveryTimes[0]);
        if (abilityRecoveryTimes[0] > 0) {
            return;
        }
        abilityRecoveryTimes[0] = abilityCooldownsS[0];

        GetAnimator().SetBool("blocking", true);
        GetPlayerController().IsBlocking = true;
        blockTimeLeftS = BlockTimeS;
    } 
    // Lunge
    public override void UseAbility2() {
        if (abilityRecoveryTimes[1] > 0) {
            return;
        }
        abilityRecoveryTimes[1] = abilityCooldownsS[1];

        PlayerController player = GetPlayerController();

        GetAnimator().SetBool("lunging", true);
        player.IsRooted = true;

        // Basically send a projectile where the potato itself is the projectile
        Vector2 cursorPosProjected = player.GetProjectedCursorPosition();
        Vector2 playerPosition2D = new Vector2(transform.position.x, transform.position.z);
        Vector2 aimDirection = (cursorPosProjected - playerPosition2D).normalized; 
        Transform parent = transform.parent;
        // GameObject potatoSprite = GetAnimator().gameObject;
        GameObject attackObject = AbilityManager.Instance.PerformProjectileAttack(player.PlayerIndex, playerPosition2D, aimDirection, 0.4f, 15, 0.5f, 0.01f, (GameObject hitPlayer) => {
            if (hitPlayer) {
                hitPlayer.GetComponent<PlayerController>().SetSlowStatus(1);
            }
            parent.transform.position = transform.position;
            transform.SetParent(parent, false);
            GetAnimator().SetBool("lunging", false);
            player.IsRooted = false;
        });
        transform.SetParent(attackObject.transform, false);
    }
    // Leap
    public override void UseAbility3() {
        if (abilityRecoveryTimes[2] > 0) {
            return;
        }
        abilityRecoveryTimes[2] = abilityCooldownsS[2];

        // TODO: Implement Me
    }
    // Pound
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
        
        if (blockTimeLeftS > 0) {
            blockTimeLeftS -= Time.deltaTime;
            if (blockTimeLeftS < 0) {
                GetAnimator().SetBool("blocking", false);
                GetPlayerController().IsBlocking = false;
            }
        }
    }
}
