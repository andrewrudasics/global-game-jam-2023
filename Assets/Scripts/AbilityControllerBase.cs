using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityControllerBase : MonoBehaviour
{
    protected int MaxProjectiles = 5;
    [HideInInspector]
    public int ProjectileCount = 5;
    protected float ProjectileCooldownS = 1.0f;
    protected float projectileRecoveryTime; // Time until next projectile is ready
    protected float[] abilityCooldownsS = new float[4];
    protected float[] abilityRecoveryTimes = new float[4]; // 0 means it's ready to use

    protected virtual PlayerController GetPlayerController() {
        return this.GetComponentInParent<PlayerController>();
    }

    protected virtual Animator GetAnimator() {
        return this.GetComponent<Animator>();
    }

    public abstract void UseAbility1();
    public abstract void UseAbility2();
    public abstract void UseAbility3();
    public abstract void UseAbility4();
    // Returns a number [0-1] where 0 means it just went on cooldown, and 1 means it's off of cooldown
    public virtual float GetAbilityCooledDownPercent(int abilityIndex) {
        return (1 - abilityRecoveryTimes[abilityIndex] / abilityCooldownsS[abilityIndex]);
    }
    public virtual void AttackMelee() { 
        PlayerController player = GetPlayerController();
        Vector2 cursorPosProjected = player.GetProjectedCursorPosition();
        Vector2 playerPosition2D = new Vector2(transform.position.x, transform.position.z);
        Vector2 aimDirection = (cursorPosProjected - playerPosition2D).normalized;
        AbilityManager.Instance.PerformRectangularAttack(player.PlayerIndex, playerPosition2D, 1.0f, 2.0f, aimDirection);
    }
    public virtual void AttackRanged() {
        if (ProjectileCount <= 0) {
            return;
        }
        
        ProjectileCount -= 1;

        PlayerController player = GetPlayerController();
        Vector2 cursorPosProjected = player.GetProjectedCursorPosition();
        Vector2 playerPosition2D = new Vector2(transform.position.x, transform.position.z);
        Vector2 aimDirection = (cursorPosProjected - playerPosition2D).normalized;
        AbilityManager.Instance.PerformRangedAttack(player.PlayerIndex, playerPosition2D, aimDirection);
    }

    protected virtual void Update() {
        // Recover abilities
        for (int i = 0; i < abilityRecoveryTimes.Length; i++) {
            if (abilityRecoveryTimes[i] > 0) {
                abilityRecoveryTimes[i] -= Time.deltaTime;
                if (abilityRecoveryTimes[i] < 0) {
                    abilityRecoveryTimes[i] = 0;
                }
            }
        }

        // Recover projectiles
        if (ProjectileCount < MaxProjectiles) {
            projectileRecoveryTime -= Time.deltaTime;
            if (projectileRecoveryTime <= 0) {
                ProjectileCount += 1;
                projectileRecoveryTime = ProjectileCooldownS;
            }
        }
    }
}
