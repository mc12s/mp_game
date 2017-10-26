using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour {

	void OnTriggerEnter2D( Collider2D other){
		if(other.CompareTag("bullet")){
			Destroy(other.gameObject);
		}
	}
}
