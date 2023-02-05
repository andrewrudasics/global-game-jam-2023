using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityControllerBase : MonoBehaviour
{
    private int MaxProjectiles = 5;
    [HideInInspector]
    public int ProjectileCount = 5;
    private float ProjectileCooldownS = 1.0f;
    private float projectileRecoveryTime;

    protected virtual PlayerController GetPlayerController() {
        return this.GetComponentInParent<PlayerController>();
    }

    public abstract void UseAbility1();
    public abstract void UseAbility2();
    public abstract void UseAbility3();
    public abstract void UseAbility4();
    public abstract float GetAbilityCooldown(int abilityIndex);
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

    public void Update() {
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
