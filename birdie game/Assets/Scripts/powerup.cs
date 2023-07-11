using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerup : MonoBehaviour
{


    public GameObject pickupEffect;

    void OnTriggerEnter(Collider other)
    {
        // This if compares the tag that is contained withing other, to the string "Player"
        if (other.CompareTag("Player"))
        {
            // Perform pickup animation and clearup object
            Pickup();

            // These two lines will not work:
            // PlayerMovement stats = GetComponent<PlayerMovement>();
            // stats.Speed *= multiplier;

            // TODO: Modify Player.speed using the "other" thing, 
            //print("OnTriggerEnter other = Player; name, tag & speed = " + other.gameObject.name + " " + other.gameObject.tag + " " + other.gameObject.speed);
            //other.gameObject.Speak();

        }
    }
    void Pickup()
    {
        Instantiate(pickupEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
