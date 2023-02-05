using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityControllerBase : MonoBehaviour
{
    public GameObject ThrowingKnifeP1Prefab;
    public GameObject ThrowingKnifeP2Prefab;
    public int MaxProjectiles = 3;
    public int ProjectileCount = 3;
    protected float ProjectileCooldownS = 1.0f;
    protected float projectileRecoveryTime; // Time until next projectile is ready
    public GameObject MeleeKnifePrefab;
    public float MeleeSweepAngle;
    protected float MeleeCooldownS = 1.0f;
    protected float meleeRecoveryTime;
    public float MeleeAnimationDuration = 0.5f;
    protected float[] abilityCooldownsS = new float[4];
    protected float[] abilityRecoveryTimes = new float[4]; // 0 means it's ready to use

    protected virtual PlayerController GetPlayerController() {
        return this.GetComponentInParent<PlayerController>();
    }

    protected virtual Animator GetAnimator() {
        return this.GetComponentInChildren<Animator>();
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
        if (meleeRecoveryTime > 0)
        {
            return;
        }
        meleeRecoveryTime = MeleeCooldownS;

        PlayerController player = GetPlayerController();
        Vector2 cursorPosProjected = player.GetProjectedCursorPosition();
        Vector2 playerPosition2D = new Vector2(transform.position.x, transform.position.z);
        Vector2 aimDirection = (cursorPosProjected - playerPosition2D).normalized;

        // Spawn Melee Knife
        GameObject meleeObject = Instantiate(MeleeKnifePrefab, gameObject.transform);
        float currentRotation = -1.0f * Vector2.SignedAngle(Vector2.right, aimDirection);
        float startRotation = currentRotation - (MeleeSweepAngle / 2);
        float endRotation = currentRotation + (MeleeSweepAngle / 2);
        StartCoroutine(RotateKnife(meleeObject, startRotation, endRotation, MeleeAnimationDuration));


        AbilityManager.Instance.PerformRectangularAttack(player.PlayerIndex, playerPosition2D, 1.0f, 2.0f, aimDirection, 10);
    }
    public virtual void AttackRanged() {
        if (ProjectileCount <= 0) {
            return;
        }
        
        ProjectileCount -= 1;

        // Fire a projectile
        PlayerController player = GetPlayerController();
        Vector2 cursorPosProjected = player.GetProjectedCursorPosition();
        Vector2 playerPosition2D = new Vector2(transform.position.x, transform.position.z);
        Vector2 aimDirection = (cursorPosProjected - playerPosition2D).normalized;
        GameObject attackObject = AbilityManager.Instance.PerformProjectileAttack(player.PlayerIndex, playerPosition2D, aimDirection, 0.3f, 5, 5, 0.02f);

        // Attach Knife onto projectile
        Vector3 direction = new Vector3(aimDirection.x, 0, aimDirection.y);
        Quaternion shootRotation = Quaternion.FromToRotation(Vector3.right, direction);
        if (player.PlayerIndex == 0) {
            GameObject knife = Instantiate(ThrowingKnifeP1Prefab, attackObject.transform);
        } else if (player.PlayerIndex == 1) {
            GameObject knife = Instantiate(ThrowingKnifeP2Prefab, attackObject.transform);
        }
        
        attackObject.transform.rotation = shootRotation;
    }

    protected virtual void Update() {
        // Recover Melee
        if (meleeRecoveryTime > 0)
        {
            meleeRecoveryTime -= Time.deltaTime;
            if (meleeRecoveryTime < 0)
            {
                meleeRecoveryTime = 0;
            }
        }

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

    IEnumerator RotateKnife(GameObject knifeObject, float startRotation, float endRotation, float duration)
    {
        float t = 0.0f;
        while(t < duration)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / duration); // % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation,
            transform.eulerAngles.z);
            yield return null;
        }

        Destroy(knifeObject);
    }
}
