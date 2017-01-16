using UnityEngine;
using System.Collections;

public class Amon : Enemy {

	private bool firstBlackmail = true;

	void Start () {
		//alter stats from the superclass
		engagement = 25;
		maxEngagement = 54;
		esteem = 25;
		maxEsteem = 54;
		energy = 15;
		maxEnergy = 54;

		//Sets identity variables
		name = "Amon";
		className = "Professional Slacker";
		gender = 1;

		//Sets enemy conversation falloff markers
		//IE: Amon forgets insults easily, but not blackmail
		brownNoseFalloffRate = 0.25f;
		blackMailFalloffRate = 0.1f;
		bitchOutFalloffRate = 0.5f;

	}

	//dialogue response and effect based on player option
	public override string getResponse(int x){
		string reply = "...";

			setPreviousResponseTrackers ();

		//If the player's response was different, this resets accordingly
		 if (x != previousResponseNumber) {
			effectMultiplier = setNewEffectMultiplyer (x);
			previousResponseNumber = x;
		}

		resetPreviousResponseTrackers (x);

		if (x == 0) {

			//The enemy's reply is decided here, based on effect multiplier
			if (effectMultiplier >= 1f) {
				reply = "Amon is awaiting your response, man."; dialoguerResponseID = 10;
			} else if (effectMultiplier < 1f && effectMultiplier >= 0f) {
				reply = "Amon is getting kinda creeped out, dude."; dialoguerResponseID = 11;
			} else if (effectMultiplier < 0f && effectMultiplier >= -1f) {
				reply = "Amon can feel you staring into his soul, man."; dialoguerResponseID = 12;
			} else if (effectMultiplier < -1f) {
				reply = "Amon is ligitimatley worried you just had a stroke."; dialoguerResponseID = 13;
			}

			//Effect is listed here
			engagement += (2f * (effectMultiplier - 0.5f));
			energy += (2f * (effectMultiplier -  0.5f));
			esteem += (2f * (effectMultiplier -  0.5f));
		} else if (x == 1) {
			
			//The enemy's reply is decided here, based on effect multiplier
			if (effectMultiplier >= 1f) {
				reply = "Amon is flattered by your flattery, dude.";
				dialoguerResponseID = 1;
			} else if (effectMultiplier < 1f && effectMultiplier >= 0f) {
				reply = "Amon gets the point, thank you very much.";
				dialoguerResponseID = 1;
			} else if (effectMultiplier < 0f && effectMultiplier >= -1f) {
				reply = "Amon is starting to get annoyed now, dude";
				dialoguerResponseID = 1;
			} else if (effectMultiplier < -1f) {
				reply = "Amon's getting irritated by your disingenuous compliments.";
				dialoguerResponseID = 1;
			}

			//Effect is listed here; Engagement gets lowered moderately,
			//energy and esteem are both raised slightly
			engagement += (-6f * Mathf.Abs(effectMultiplier));
			energy += (4f * (effectMultiplier));
			esteem += (4f * (effectMultiplier));

		} else if (x == 2) {
			
			//The enemy's reply is decided here, based on effect multiplier
			if (effectMultiplier >= 1f) {reply = "Amon laughs good-naturedly at your ribbing."; dialoguerResponseID = 2;}
			else if (effectMultiplier < 1f && effectMultiplier >= 0f) {reply = "Amon's starting to get the impression that you're not joking."; dialoguerResponseID = 3;}
			else if (effectMultiplier < 0f && effectMultiplier >= -1f) {reply = "Amon's left eye twitches involuntarily."; dialoguerResponseID = 4;}
			else if (effectMultiplier < -1f) {reply = "Amon is freaking pissed off...dude."; dialoguerResponseID = 5;}

			//Engagement and Energy are both raised, and Esteem is initially unaffetcted 
			//As the option is selected more often, Esteem starts to be damaged quicker
			engagement += (3f * (effectMultiplier));
			energy += (5f * (effectMultiplier));
			esteem += (-1f * Mathf.Abs((effectMultiplier-1))*2);

		} else if (x == 3) {
			if (itemRecieved == 1) {
				reply = "Amon sees your paperwork and laughs heartily.";
				engagement -= 0f;
				energy += 2f;
				esteem -= 1f;
				dialoguerResponseID = 18;
			}
			else if (itemRecieved == 2) {
				reply = "Amon sees the photos and begins to chuckle.";
				engagement += 5f;
				esteem += 5f;
				dialoguerResponseID = 19;
			}
			else if (itemRecieved == 3) {
				reply = "Amon takes a swig, and promptly spits it out when his mouth starts burning.";
				engagement += 10f;
				energy += 10f;
				esteem -= 20f;
				dialoguerResponseID = 17;
			}
			else if (itemRecieved == 0) {
				reply = "A trade only works if you got something to trade!";
				engagement--;
				energy--;
				esteem--;
				dialoguerResponseID = 16;
			}

		} else if (x == 4) {
			
			//The enemy's reply is decided here, based on effect multiplier
			if (effectMultiplier >= 1f) {
				reply = "Amon breaks into a giggle fit over your attempt at blackmail.";

				if (firstBlackmail) {
					dialoguerResponseID = 6;
					firstBlackmail = false;
				}else{
					dialoguerResponseID = 7;
				}		
		}

			else if (effectMultiplier < 1f && effectMultiplier >= 0f) {reply = "Amon thinks your coercion is kinda cute."; dialoguerResponseID = 7;}
			else if (effectMultiplier < 0f && effectMultiplier >= -1f) {reply = "Amon is getting enamored with your determination.";dialoguerResponseID = 8;}
			else if (effectMultiplier < -1f) {reply = "Every threat you make is now met with a smile and a cheer.";dialoguerResponseID = 9;}

			//Engagement and Esteem both grow progressively faster
			//but Energy also starts to fall progressively faster
			engagement += (1f * Mathf.Abs((effectMultiplier-1))*2);
			energy += (-2f * Mathf.Abs((effectMultiplier-1))*2);
			esteem += (1f * Mathf.Abs((effectMultiplier-1))*2);


		}

		effectMultiplier -= .5f;
		Debug.Log(brownNoseMultiplyer + " " + bitchOutMultiplyer + " " + blackMailMultiplyer);

		//Sends the string back to the BattleManager
		return reply;
	}
}
