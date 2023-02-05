using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityManager : MonoBehaviour
{
    private static AbilityManager _instance;
    public GameObject DebugEffectContainer;
    public GameObject DebugRectangularAttackVisualObjectPrefab;
    private GameObject debugRectangularAttackVisualObject;

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

        debugRectangularAttackVisualObject = Object.Instantiate(DebugRectangularAttackVisualObjectPrefab, DebugEffectContainer.transform);
    }

    public void PerformRectangularAttack(int owningPlayer, Vector2 position2D, float width, float length, Vector2 direction) {
        // Rectangular Attack
        // Attack Width: 
        // Attack Length:
        // Attack Direction:
        Vector3 position = new Vector3(position2D.x, 0, position2D.y);
        Vector2 attackCenter2D = position2D + (length / 2) * direction;
        Vector3 attackCenter = new Vector3(attackCenter2D.x, 0, attackCenter2D.y);
        float angle = Mathf.Atan2(direction.x, direction.y) + Mathf.PI / 2.0f;
        Quaternion rotation = Quaternion.AngleAxis(angle / Mathf.PI * 180.0f, new Vector3(0, 1, 0));

        // Debug Visualization
        DebugEffectContainer.transform.position = position;
        debugRectangularAttackVisualObject.transform.localPosition = attackCenter - position;
        debugRectangularAttackVisualObject.transform.rotation = rotation;
        ParticleSystem debugParticleSystem = debugRectangularAttackVisualObject.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = debugParticleSystem.main;
        main.startSizeXMultiplier = length;
        main.startSizeYMultiplier = width;
        debugParticleSystem.Play();

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
            }
        }
    }
}
