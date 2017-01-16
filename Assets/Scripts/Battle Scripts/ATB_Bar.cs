using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ATB_Bar : MonoBehaviour {

	//This indicates how much of the ATB bar will be drained per second
	public float speed = 0.1f;
	Image ATB;
	public float percentage;

	void Start(){
		//grabs the image of the ATB bar
		ATB = this.GetComponent<Image> (); 
	}
	
	// Update is called once per frame
	void Update () {
		 percentage = ATB.fillAmount;

		//this causes the bar to deplete after 5 seconds
		if (percentage > 0) {
			ATB.fillAmount = percentage - (speed * Time.deltaTime);
		}
		//script that resets the bar to 0 in case of error causing the the fillamount to drop negative
		else if (percentage < 0) {
			ATB.fillAmount = 0;
		}
	
	}
	//function checks to see if it is the player's turn or not
	public bool checkTurn(){
		bool isTurn = true;
		if (percentage == 0) {
			isTurn =  false;
		}
		return isTurn;
	}

	//functions allow the starting and stopping of the meter during selection
	public void pause(){
		speed = 0;
	}
	public void unpause(){
		speed = 0.2f;
	}
}
