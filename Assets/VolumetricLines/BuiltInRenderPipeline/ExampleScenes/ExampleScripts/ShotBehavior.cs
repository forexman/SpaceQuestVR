using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour {

	[SerializeField] float projectileSpeed = 10f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * Time.deltaTime * projectileSpeed;
		Destroy(gameObject, 1);
	}

	void OnTriggerEnter(Collider otherObject){
        Destroy(gameObject);
    }
}
