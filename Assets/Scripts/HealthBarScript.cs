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
}
