using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

    public Slider slider;
    public float maxHealth;


    public void SetMaxHealth (int health)
    {
        slider.maxValue = health;
        slider.minValue = health;
    }
    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void DamagePlayer(int playerIndex, int damageAmount) {
        float newValue = slider.value - damageAmount;
        if (newValue <= 0) {
            GameStateManager.Instance.OnPlayerDeath(playerIndex);
            newValue = 0;
        }
        slider.value = newValue;
    }
}
