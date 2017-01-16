using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour {

	//These are the enemy's health stats
	public float engagement;
	public float maxEngagement;
	public float energy;
	public float maxEnergy;
	public float esteem;
	public float maxEsteem;

	//These numers keep track of the player response and change accordingly
	public int previousResponseNumber = 5;
	public float effectMultiplier = 1f;

	//These numbers keep track of how many times the player repeated before switching
	public float brownNoseMultiplyer = 1f;
	public float blackMailMultiplyer = 1f;
	public float bitchOutMultiplyer = 1f;

	//These numbers change how tolerant each enemy type is to different conversation types
	public float brownNoseFalloffRate = 1f;
	public float blackMailFalloffRate = 1f;
	public float bitchOutFalloffRate = 1f; 

	//These are the enemy's miscellaneous attributes
	public string name;
	public string className;
	//0 = female; 1 = male; 2 = other
	public int gender;

	//This int keeps track of dialogue responses
	public int dialoguerResponseID;
	public int itemRecieved;



	void Start () {
		//sets default values for enemy attributes in case
		name = "Anonymous";
		className = "Co-Worker";
		gender = 2;

		//sets default values for health attributes
		  engagement = 10;
		  maxEngagement = 100;
		  energy = 10;
		  maxEnergy = 100;
		  esteem = 10;
		  maxEsteem = 100;

		dialoguerResponseID = 0;


	}

	void Update(){
		//Ensures the health values cant go over max
		if (engagement > maxEngagement) {
			engagement = maxEngagement;
		}
		if (energy > maxEnergy) {
			energy = maxEnergy;
		}
		if (esteem > maxEsteem) {
			esteem = maxEsteem;
		}
	}

	//Returns health values as a percentage, so that it can be used with the health bars
	public float getEngagement(){
		return (engagement / maxEngagement);
	}
	public float getEnergy(){
		return (energy / maxEnergy);
	}
	public float getEsteem(){
		return (esteem / maxEsteem);
	}

	//Used by Subclasses to track responses
	public void setPreviousResponseTrackers(){

		//Sets multiplyer trakers to keep track of how many turns have
		//passed since the player switched options
		if (previousResponseNumber == 1) {brownNoseMultiplyer = effectMultiplier;}
		if (previousResponseNumber == 2) {bitchOutMultiplyer = effectMultiplier;}
		if (previousResponseNumber == 4) {blackMailMultiplyer = effectMultiplier;}
	}

	public float setNewEffectMultiplyer(int x){

		float value = 1.0f;

		//Returns the value of old response tracker
		if(x == 1){value = brownNoseMultiplyer; }
		if(x == 2){value = bitchOutMultiplyer; }
		if(x == 4){value = blackMailMultiplyer; }

		return value;

	}

	public void resetPreviousResponseTrackers(int x){

		//slowly resets repeated options over time
		//Amon forgets insults more easily that Brown Nosing or Blackmail
		if (x != 1 && brownNoseMultiplyer < 1) {
			brownNoseMultiplyer += brownNoseFalloffRate;
		}
		if (x != 2 && bitchOutMultiplyer < 1) {
			bitchOutMultiplyer += bitchOutFalloffRate;
		}
		if (x != 4 && blackMailMultiplyer < 1) {
			blackMailMultiplyer += blackMailFalloffRate;
		}
	}

	//Abstract method; different for every subclass
	public abstract string getResponse (int x); 

}

