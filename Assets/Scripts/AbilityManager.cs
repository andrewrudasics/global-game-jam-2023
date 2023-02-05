using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityManager : MonoBehaviour
{
    private static AbilityManager _instance;
    public GameObject DebugRectangularAttackEffectPrefab;
    public GameObject DebugCircularAttackEffectPrefab;

    // Define attack:
    // - Collider Shape
    // - Collider Radius?
    // - Maybe this should be just a 3D Collider
    // - Origination Position (either at the player, or another area)
    // - Direction

    public static AbilityManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public void PerformRectangularAttack(int owningPlayer, Vector2 position2D, float width, float length, Vector2 direction) {
        // Rectangular Attack
        Vector3 position = new Vector3(position2D.x, 0, position2D.y);
        Vector2 attackCenter2D = position2D + (length / 2) * direction;
        Vector3 attackCenter = new Vector3(attackCenter2D.x, 0, attackCenter2D.y);
        float angle = Mathf.Atan2(direction.x, direction.y) + Mathf.PI / 2.0f;
        Quaternion rotation = Quaternion.AngleAxis(angle / Mathf.PI * 180.0f, new Vector3(0, 1, 0));

        // Debug Visualization
        GameObject effectContainer = Object.Instantiate(DebugRectangularAttackEffectPrefab);
        effectContainer.transform.position = position;
        ParticleSystem effect = effectContainer.GetComponentInChildren<ParticleSystem>();
        effect.gameObject.transform.localPosition = attackCenter - position;
        effect.gameObject.transform.rotation = rotation;
        ParticleSystem.MainModule main = effect.main;
        main.startSizeXMultiplier = length;
        main.startSizeYMultiplier = width;
        effect.Play();

        // Perform hit detection
        Collider[] hitColliders = Physics.OverlapBox(attackCenter, new Vector3(length / 2, 0.1f, width / 2), rotation, Physics.AllLayers);
        foreach (Collider collider in hitColliders) {
            PlayerController playerController = collider.gameObject.GetComponent<PlayerController>();
            if (playerController == null) {
                continue;
            }
            int playerIndex = playerController.PlayerIndex;
            if (playerIndex != owningPlayer) {
                Debug.Log("Player Hit: " + playerIndex);
                HealthBarManager.Instance.DamagePlayer(playerIndex, 10);
            }
        }
    }

    public void PerformCircularAttack(int owningPlayer, Vector2 position2D, float radius) {
        Vector3 attackCenter = new Vector3(position2D.x, 0, position2D.y);

        // Debug Visualization
        GameObject effectContainer = Object.Instantiate(DebugCircularAttackEffectPrefab);
        effectContainer.transform.position = attackCenter;
        ParticleSystem effect = effectContainer.GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule main = effect.main;
        main.startSizeXMultiplier = radius * 2;
        main.startSizeYMultiplier = radius * 2;
        effect.Play();

        // Perform hit detection
        Collider[] hitColliders = Physics.OverlapSphere(attackCenter, radius, Physics.AllLayers);
        foreach (Collider collider in hitColliders) {
            PlayerController playerController = collider.gameObject.GetComponent<PlayerController>();
            if (playerController == null) {
                continue;
            }
            int playerIndex = playerController.PlayerIndex;
            if (playerIndex != owningPlayer) {
                Debug.Log("Player Hit: " + playerIndex);
                HealthBarManager.Instance.DamagePlayer(playerIndex, 10);
            }
        }
    }
}
