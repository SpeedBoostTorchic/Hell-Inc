using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrackerBehavior : MonoBehaviour {

	//These are the images the trackers will use
	public Sprite plus;
	public Sprite minus;

	//These variables will help determine how
	//the health bars are affected
	public float previousHealthPercentage;
	public bool enabled;

	void Start () {
		enabled = false;
	}

	void Update(){
		//This turns the tracker images on/off based on whether
		//or not the player has just selected a combat option
		if (enabled == true) {
			this.GetComponent <Image> ().enabled = true;
		} else if (enabled == false) {
			this.GetComponent <Image> ().enabled = false;
		}
	}

	//This method is called by BattleManager to swap the
	//Plus-Minus images next to the enemy health bar
	public void checkTrackerImage(float currentHealth){

		//Checks to see if the health bar has risen
		//or fallen, and changes the image respectively
		if (currentHealth > previousHealthPercentage) {
			this.GetComponent<Image> ().sprite = plus;
	
		} 
		else if (currentHealth < previousHealthPercentage) {
			this.GetComponent <Image> ().sprite = minus;

		}


		previousHealthPercentage = currentHealth;
		enabled = true;
	}

}
