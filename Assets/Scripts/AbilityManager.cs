using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityManager : MonoBehaviour
{
    private static AbilityManager _instance;
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

        debugRectangularAttackVisualObject = Object.Instantiate(DebugRectangularAttackVisualObjectPrefab);
    }

    public void PerformRectangularAttack(Vector2 position, float width, float length, Vector2 direction) {
        // Rectangular Attack
        // Attack Width: 
        // Attack Length:
        // Attack Direction:
        Vector2 attackCenter2D = position + width * direction;
        Vector3 attackCenter = new Vector3(attackCenter2D.x, 0, attackCenter2D.y);
        float angle = Mathf.Atan2(direction.y, direction.x);
        Debug.Log(angle);
        Quaternion rotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));

        // Debug Visualization
        debugRectangularAttackVisualObject.transform.position = attackCenter;
        debugRectangularAttackVisualObject.transform.rotation = rotation;
        ParticleSystem debugParticleSystem = debugRectangularAttackVisualObject.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = debugParticleSystem.main;
        main.startSizeXMultiplier = length;
        main.startSizeYMultiplier = width;
        debugParticleSystem.Play();

        // Perform hit detection
        Collider[] hitColliders = Physics.OverlapBox(attackCenter, new Vector3(length / 2, 0.1f, width / 2), rotation, Physics.AllLayers);
        foreach (Collider collider in hitColliders) {
            // TODO: Replace with actual player controller
            DummyPlayerController playerController = collider.gameObject.GetComponent<DummyPlayerController>();
            if (playerController == null) {
                continue;
            }
            int playerIndex = playerController.PlayerIndex;
            Debug.Log("Player Hit: " + playerIndex);
        }
    }
}
