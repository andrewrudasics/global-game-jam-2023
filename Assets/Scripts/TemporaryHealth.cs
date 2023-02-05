using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryHealth : MonoBehaviour
{
    GameObject defenseCall;
    public int tempHealth = 20;


    public GameObject defenseBarier;
    

    bool defense = false;
    // In the event an action of defense is called, the current health increases by a factor of the temprary health
    // currentHealth + tempHealth = newHealth;

    // if(defense == true)
    // {
    //     int newHealth;
    //     //newHealth = currentHealth + tempHealth;
    // }
    //create a function called defense for the charaacter

    public void defenseCalling(){
        //It checks the index of the respective player requesting defense
        //adds the value of the newHealth to the player.
    }
}
