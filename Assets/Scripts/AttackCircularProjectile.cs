using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCircularProjectile : MonoBehaviour
{
    public int OwningPlayer = -1;
    public GameObject CleanupObject;
    public float MaximumLifetimeS = 5.0f;
    public Vector2 StartingPosition2D;
    public Vector2 Direction;
    public float Speed = 5.0f;
    public float Radius = 0.3f;
    public float Damage = 5;

    private int frameCounter = 0;
    private int framesBeforeCollisionCheck = 30;
    private float lifeCountdown;
    private Vector2 position;

    public void Cleanup() {
        if (CleanupObject) {
            Destroy(CleanupObject);
        } else {
            Destroy(this);
        }
    }

    public void Start()
    {
        lifeCountdown = MaximumLifetimeS;
        position = StartingPosition2D;
    }

    void Update()
    {
        if (lifeCountdown <= 0) {
            Cleanup();
        }
        lifeCountdown -= Time.deltaTime;

        position += Direction * Speed;
        transform.position = new Vector3(position.x, 0.2f, position.y);

        frameCounter += 1;
        if (frameCounter % framesBeforeCollisionCheck == 0) {
            if (AbilityManager.Instance.PerformCircularAttack(OwningPlayer, position, Radius, Damage)) {
                Cleanup();
            }
        }
    }
}
