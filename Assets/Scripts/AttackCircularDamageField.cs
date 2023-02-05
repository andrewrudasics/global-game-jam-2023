using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCircularDamageField : MonoBehaviour
{
    public int OwningPlayer = -1;
    public GameObject CleanupObject;
    public float AttackDurationS = 5.0f;
    public float TickPeriodS = 0.25f;
    public Vector2 Position2D;
    public float Radius = 1.0f;
    public float TickDamage = 5;

    private float tickCountdown = 0;
    private float attackDurationCountdown;

    void Start() {
        attackDurationCountdown = AttackDurationS;
    }

    void Update()
    {
        if (attackDurationCountdown <= 0) {
            if (CleanupObject) {
                Destroy(CleanupObject);
            } else {
                Destroy(this);
            }
        }

        attackDurationCountdown -= Time.deltaTime;

        if (tickCountdown <= 0) {
            AbilityManager.Instance.PerformCircularAttack(OwningPlayer, Position2D, Radius, TickDamage);
            tickCountdown = TickPeriodS;
        } else {
            tickCountdown -= Time.deltaTime;
        }
    }
}
