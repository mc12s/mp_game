using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSetup : MonoBehaviour {

	public GameObject planet;
	public GameObject player;
	public GameObject planet_text;

	public int planet_count=8;
	public int planet_size_max=10;
	public int planet_size_min=2;


	// Use this for initialization
	void Awake () {
		Canvas myCanvas = FindObjectOfType<Canvas>();
		for(int i=0; i<planet_count; i++){
			float planet_size = Random.Range(planet_size_min, planet_size_max);

			Vector2 planet_loc = planetPositioner(planet_size/2); //recursive function for positioning nonoverlapping planets

			GameObject newPlanet = Instantiate(planet, planet_loc, Quaternion.identity);	//random position
			newPlanet.transform.localScale = new Vector3(planet_size, planet_size, 0f);		//random size
			newPlanet.GetComponent<SpriteRenderer>().color = new Color(Random.value,Random.value,Random.value); //random color

			//attach text to each planet
			GameObject newPlanetText = Instantiate(planet_text, planet_loc, Quaternion.identity);
			newPlanetText.transform.SetParent(myCanvas.transform);
			newPlanetText.GetComponent<Text>().text = planet_size.ToString();


			if(planet_size>=9){
				//for(int j=0; j<Random.Range(1,3);j++){	//set for random number of moons
					Vector2 moonOffset = Random.insideUnitCircle;
					Vector2 moonOrbit = moonOffset + (moonOffset.normalized * planet_size/2);
					GameObject newMoon = Instantiate(planet, planet_loc+moonOrbit, Quaternion.identity);
					newMoon.transform.localScale = new Vector3(1.4f, 1.4f, 0f);
					newMoon.GetComponent<SpriteRenderer>().color = new Color(Random.value,Random.value,Random.value);
					newMoon.GetComponent<SpriteRenderer>().sortingOrder = 2;
					newMoon.layer = 18;
					newMoon.tag = "moon";
				//}
			}
		}
	}

	void Start(){	//Spawn in players and set their colors
		Vector2 spawn = new Vector2(-20f,-10f); //init position offset

		//P1, instantiate, color, and rename
		GameObject p1 = Instantiate(player, Vector2.Scale(spawn,new Vector2(1,1)), Quaternion.identity); 
		GameObject p1model = p1.transform.Find("player").gameObject;
		p1model.GetComponent<SpriteRenderer>().color = new Color(0,1,1,1);
		p1model.layer = 8;	//taken from layer setup
		p1model.name = "p1";

		//P2, instantiate, color, and rename
		GameObject p2 = Instantiate(player, Vector2.Scale(spawn,new Vector2(-1,1)), Quaternion.identity); 
		GameObject p2model = p2.transform.Find("player").gameObject;
		p2model.GetComponent<SpriteRenderer>().color = new Color(0,1,0,1);
		p2model.layer = 10;
		p2model.name = "p2";

		//P3, instantiate, color, and rename
		GameObject p3 = Instantiate(player, Vector2.Scale(spawn,new Vector2(1,-1)), Quaternion.identity); 
		GameObject p3model = p3.transform.Find("player").gameObject;
		p3model.GetComponent<SpriteRenderer>().color = new Color(1,0,1,1);
		p3model.layer = 12;
		p3model.name = "p3";

		//P4, instantiate, color, and rename
		GameObject p4 = Instantiate(player, Vector2.Scale(spawn,new Vector2(-1,-1)), Quaternion.identity); 
		GameObject p4model = p4.transform.Find("player").gameObject;
		p4model.GetComponent<SpriteRenderer>().color = new Color(1,0,0,1);
		p4model.layer = 14;
		p4model.name = "p4";

	}

	Vector3 planetPositioner(float radius){	//recursive function, what happens if planets randomly spawn ontop of eachother?
		Vector2 planet_loc = new Vector2(Random.Range(-35,35),Random.Range(-20,20));
		//if planet loc+scale overlaps with any other planet recurse
		if(Physics2D.OverlapCircle(planet_loc, radius+1)){
			return planetPositioner(radius);
		}
		return planet_loc;
	}

}
