using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialoguerGui : MonoBehaviour {
	//This keeps track of whether or not the dialogue
	//box is visible to the player
	private bool showing;

	//This holds the dialogue text
	private string _text;

	//Allows the creation of additional 
	//buttons for dialogue branches
	private string[] choices;

	//public GUIStyle style;
	public GUISkin skin;

	private string text;
	private bool textDisplayStarted = false;
	private bool textDisplayComplete = false;
	private bool textSoundEffectStarted = false;
	private bool continuePressed = false;

	public AudioSource textSound;
	public AudioClip blip;

	void Awake(){
		Dialoguer.Initialize ();
	}


	//These add listeners for dialoguer events
	void Start () {

		showing = false;
		Dialoguer.events.onStarted += onStarted;
		Dialoguer.events.onEnded += onEnded;
		Dialoguer.events.onTextPhase += onTextPhase;


		//style.wordWrap = true;


	}


//Creates the dialogue box and buttons
	void OnGUI (){
		GUI.skin = skin;

		if (showing == true) {
			//Draws the dialogue box
			if (textDisplayStarted == false) {
				StartCoroutine (AnimateText (_text));
				textDisplayStarted = true;
			}
			text = GUI.TextArea (new Rect (Screen.width * 0.58f, Screen.height / 10, Screen.width * 0.35f, Screen.height / 3), text);	

			if (textDisplayStarted && !textSoundEffectStarted) {
				textSound.clip = blip;
				textSound.loop = true;
				textSound.Play ();
				textSoundEffectStarted = true;
			}
			if (textDisplayComplete) {
				textSound.Stop ();
				textSoundEffectStarted = false;
			}
			

			//Draws the continue button, or the buttons for the different choices
			if (choices == null) {
				if (GUI.Button (new Rect (Screen.width * 0.58f+20, Screen.height/10+Screen.height/3, Screen.width * 0.325f, 40), "Continue")) {
					continuePressed = true;

					//If the text is not done displaying, pressing continue will
					//cancel the AnimateText co-routine, and display all the text
					//immidiately, while not progressing the dialogue
					if (!textDisplayComplete) {
						StopCoroutine ("AnimateText");
						text = "";
						text = _text;
						_text = "";
						textDisplayComplete = true;

					} 
					//Otherwise, the dialogue moves on to the next node
					else if (textDisplayComplete){
						textDisplayStarted = false;
						textDisplayComplete = false;
						Dialoguer.ContinueDialogue ();
						continuePressed = false;
					}
				}
			} else {

				//Draws multiple continue buttons if there is more than one dialogue choice
				for (int i = 0; i < choices.Length; i++) {
					if (GUI.Button (new Rect (Screen.width * 0.58f+20, Screen.height/10+Screen.height/3 + 40*i, Screen.width * 0.325f, 40), choices[i])) {
						continuePressed = true;

						//See above
						if (!textDisplayComplete) {
							StopCoroutine ("AnimateText");
							text = "";
							text = _text;
							_text = "";
							textDisplayComplete = true;

						} else if (textDisplayComplete){
							textDisplayStarted = false;
							textDisplayComplete = false;
							Dialoguer.ContinueDialogue (i);
							continuePressed = false;
						}
					}
				}
			}
		}


	}

	//onStarted() and onEnded() show/hide the dialoguer window
	private void onStarted(){
		showing = true;
		Debug.Log ("Dialogue started.");
	}

	private void onEnded (){
		showing = false;
		Debug.Log ("Dialogue ended.");
	}

	//This method parses the text data from the dialoguer program
	private void onTextPhase(DialoguerTextData data){
		_text = data.text;
		choices = data.choices;
	}

	//This class uses its own, tweaked version of the "AnimateText" co-routine
	//from the BattleManager
	IEnumerator AnimateText(string stringComplete){
		int i = 0;
		text = "";

		while (i < stringComplete.Length) {

			if (continuePressed)
				break;
			
			text += stringComplete[i++];

			if (i >= stringComplete.Length)
				textDisplayComplete = true;

			
			yield return new WaitForSeconds (0.01f);
		}
	}
}
