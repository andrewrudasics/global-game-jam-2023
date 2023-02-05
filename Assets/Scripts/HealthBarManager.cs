using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{

    public GameObject player1HealthObject;
    public GameObject player2HealthObject;
    public float maxHealth;

    public int currentHealth;

    private static HealthBarManager _instance;
    public static HealthBarManager Instance { get { 
        return _instance; 
    } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    //Collision function
    /*void OnTriggerEnter(CollisionDetector col){
        if (col.GetComponent<Collider>().name == "Carrot")
        {
            Debug.Log("They touched");
            //Slider -= damageAmount;
        }


    }*/


    // public void SetMaxHealth (int health)
    // {
    //     slider.maxValue = health;
    //     slider.minValue = health;
    // }
    // public void SetHealth(int health)
    // {
    //     slider.value = health;
    // }

    // Start is called before the first frame update
    void Start()
    {
        // currentHealth = health;
        HealthBarScript player1Health = player1HealthObject.GetComponent<HealthBarScript>();
        player1Health.SetHealth(100);
    }
    
    public void DamagePlayer(int playerIndex, int damageAmount) {
        // player1Health.damagePlayer
        
    }
    
}
