using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEndZone : MonoBehaviour
{
    void OnTriggerEnter(Collider otherObject){
        if(otherObject.gameObject.CompareTag("EnemyShip")){
            Destroy(otherObject.gameObject);
        }
    }

}
