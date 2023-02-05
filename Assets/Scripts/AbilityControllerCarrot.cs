using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityControllerCarrot : AbilityControllerBase
{
    private float dodgeSpeed = 10.0f;
    public GameObject CarrotRainEffectPrefab;
    // Dodge Roll
    public override void UseAbility1() {
        if (abilityRecoveryTimes[0] > 0) {
            return;
        }
        abilityRecoveryTimes[0] = abilityCooldownsS[0];

        // TODO: Implement Me
        PlayerController player = GetPlayerController();
        Vector2 cursorPos = player.GetProjectedCursorPosition();
        Vector2 playerPos2D = new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.z);
        Vector3 dodgeDir = new Vector3();
        dodgeDir.x = playerPos2D.x - cursorPos.x;
        dodgeDir.z = playerPos2D.y - cursorPos.y;

        player.gameObject.GetComponent<Rigidbody>().AddForce(dodgeSpeed * dodgeDir, ForceMode.Impulse);
    } 
    // Carrot Rain
    public override void UseAbility2() {
       if (abilityRecoveryTimes[1] > 0) {
            return;
        }
        abilityRecoveryTimes[1] = abilityCooldownsS[1];

        PlayerController player = GetPlayerController();
        Vector2 cursorPos = player.GetProjectedCursorPosition();        
        AbilityManager.Instance.PerformCircularDamageFieldAttack(player.PlayerIndex, cursorPos, 1.0f, 5);
        GameObject effectObj = Object.Instantiate(CarrotRainEffectPrefab, new Vector3(cursorPos.x, 0, cursorPos.y), Quaternion.identity);
        ParticleSystem effect = effectObj.GetComponentInChildren<ParticleSystem>();
        effect.Play();
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
        abilityCooldownsS[0] = 4.0f;
        abilityCooldownsS[1] = 2.0f;
        abilityCooldownsS[2] = 2.0f;
        abilityCooldownsS[3] = 2.0f;
    }

    protected override void Update() {
        base.Update();
    }
}
