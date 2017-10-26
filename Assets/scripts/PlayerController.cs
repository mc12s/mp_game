using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject player_aim;
	public GameObject thruster_obj;
	public GameObject bullet;

	public float shotspeed = 40f;
	public float boost = 500f;
	public int health = 100;

	public AudioClip fire_sound;
	public AudioClip die_sound;
	public GameObject explosion;

	private GameObject[] objs;
	private Rigidbody2D rb;
	private ParticleSystem thruster;
	private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		objs = GameObject.FindGameObjectsWithTag("planet");
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		thruster = thruster_obj.GetComponent<ParticleSystem>();
		thruster.Stop();	//stop particle system
	}

	void Update(){
		if(Input.GetButtonUp(gameObject.name +"Jump")){
			thruster.Stop();	//stop the particle system when you let go of boost
		}
		player_aim.transform.position = transform.position;
	}

	void FixedUpdate () {
		//Gravity Calculations
		foreach(GameObject planet in objs){	//for every planet
			float planet_mass=Mathf.Pow((planet.transform.localScale.x*Mathf.PI),2f)*50f; //increased mass to counter closeness of planets
			Vector3 heading = planet.transform.position - transform.position; //vector from planet to player
			float distance = heading.magnitude; //distance from player to planet
			float planet_gravity = planet_mass/(Mathf.Pow(distance*5f, 2f));    // GMm/d^2, artificially increased distance
			Vector3 direction = heading/distance;		//a vector divided by its magnitude is a unit vector
			rb.AddForce(direction * planet_gravity);	//pull player towards planet with calculated gravity

		} //end of gravity loop




		//Get Inputs
		float moveH = Input.GetAxis(gameObject.name + "Horizontal");
		float moveV = Input.GetAxis(gameObject.name + "Vertical");

		Vector2 movement = new Vector2(moveH,moveV);
		rb.AddForce(movement * 50f * Time.fixedDeltaTime); //move player without boost

		if(Input.GetButton(gameObject.name + "Jump")){ //Move with boost
			float angle = Mathf.Atan2(moveV,moveH) * (180f/Mathf.PI);	//convert input axes into degrees
			player_aim.transform.eulerAngles = new Vector3(0f,0f,angle-180f);
			thruster.Play(); //starts the particle effect
			rb.AddForce(movement * boost * Time.fixedDeltaTime);
		}




		if(Input.GetButtonDown(gameObject.name + "Fire1")){
			if(movement.magnitude != 0){								//if we're holding a direction and pressing fire
				GameObject laser = Instantiate(bullet,transform.position,Quaternion.identity); 	//create a bullet
				laser.layer = gameObject.layer + 1;												//put it on the correct layer
				laser.GetComponent<Rigidbody2D>().velocity = movement * shotspeed;				//fire it off in the right direction
				AudioSource.PlayClipAtPoint(fire_sound, transform.position);					//play a sound

			}
		}

	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("bullet")){			//if you get shot
			print("hit");
			health-=40;
			Destroy(other.gameObject);
		}
		else if(other.CompareTag("shredder")){	//if you stray too far from the playing field
			health-=100;
		}
		if(health<=0){
			Die();
		}
	}

	void Die(){
		AudioSource.PlayClipAtPoint(die_sound,transform.position);	//play explosion sound
		GameObject explode = Instantiate(explosion, transform.position, Quaternion.identity);	//load explosion
		ParticleSystem.MainModule p_main= explode.GetComponent<ParticleSystem>().main; 
		p_main.startColor = sr.color;	//color the explosion the same as the player
		Destroy(transform.parent.gameObject);	//destroy the player
	}
}
