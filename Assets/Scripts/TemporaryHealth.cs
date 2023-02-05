using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryHealth : MonoBehaviour
{

    public float temporaryHelthAmount= 20.0f;
    public float temporaryHealthDuration = 10.0f;

    private float health;
    private float originalHealth;
    private float startTime;

    private void Start(){
        health = GetComponent<HealthBarScript>().health;
        originalHealth = health;
    }

    private void Update()
    {
        health = GetComponent<HealthBarScript>().health;
        float timeElapsed = Time.time - startTime;

        if(health < originalHealth && timeElapsed < temporaryHealthDuration)
        {
            health += temporaryHelthAmount * Time.deltaTime;
            GetComponent<Health>().health = health;
        }
        else{
            startTime = Time.time;
            originalHealth = health;
        }
    }
}
