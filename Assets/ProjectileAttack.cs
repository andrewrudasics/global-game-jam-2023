using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public int owningPlayer = -1;
    public float lifespan = 10.0f;

    private float creationTime;

    public void Start()
    {
        creationTime = Time.time;
    }

    public void Update()
    {
        if (Time.time - creationTime > lifespan)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        PlayerController playerController = collider.gameObject.GetComponent<PlayerController>();
        if (playerController == null)
        {
            return;
        }
        int playerIndex = playerController.PlayerIndex;
        if (playerIndex != owningPlayer)
        {
            Debug.Log("Player Hit: " + playerIndex);
            HealthBarManager.Instance.DamagePlayer(playerIndex, 10);
            Destroy(gameObject);
        }
        
    }
}
