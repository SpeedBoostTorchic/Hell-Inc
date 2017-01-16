using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleManager : MonoBehaviour {

	//This is the enemy that the player fights
	//Further script can be found under "Enemy"
	public Enemy enemy;

	//These are the player GUI components; most importantly
	//the player's four combat selection options
	public GameObject ATB;
	public GameObject textBox;
	public GameObject brownNose;
	public GameObject bitchOut;
	public GameObject barterWith;
	public GameObject blackmail;

	//These are the enemy "health bars" on the left side
	public GameObject EngB;
	public GameObject ErgB;
	public GameObject EstB;
	public GameObject TotalB;

	//These tracker images display whether the given
	//attribute has increased or decreased. Further sript
	//can be found under "TrackerBehavior'
	public TrackerBehavior  engagementTracker;
	public TrackerBehavior energyTracker;
	public TrackerBehavior esteemTracker;

	//These components are used for text and dialogue
	public float textAnimateSpeed;
	Text text;
	int selection;
	int phase;

	int silentText;
	int bitchOutText;
	int brownNoseText;
	int blackmailText;
	int barterText;

	public GameObject continueText;

	//Image components holder
	Image engagement;
	Image energy;
	Image esteem;
	Image total;
	public Image total2;
	public Image total3;

	//Booleans used to prevent dialoguer glitches
	private bool continuePressed = false;
	private bool textAnimStarted = false;
	public bool endDialogueInitialized;
	public GameObject screenFaderObject;
	public ScreenFader fade;

	//Stuff used for the inventory screen
	public GameObject inventoryPanel;
	public GameObject inventoryManager;
	public bool inventoryOpen = false;

	//These particle systems for the battle
	public GameObject constantParticle;
	private ParticleSystem constantEmission;
	public GameObject victoryParticle;
	private ParticleSystem victoryEmission;

	//These assets are used entirely for win/loss conditions
	public SpriteRenderer background;
	public BattleMusic music;


	void Awake () {
		//This line is mandatory for engaging the dialoguer program: DO NOT TOUCH!!!!
		Dialoguer.Initialize ();
		screenFaderObject.SetActive (true);
	}

	void Start () {
		//Grabs the textbox so the readout can be altered
		text = textBox.GetComponent<Text> ();
		selection = 0;

		//This variable is used to deterine what "phase" the text is in
		//"Phases" are my way of making sure that things only progress
		//when the player clicks to continue
		phase = 5;

		//These lines grab the images from the health bar objects
		engagement = EngB.GetComponent<Image> ();
		energy = ErgB.GetComponent<Image> ();
		esteem = EstB.GetComponent<Image> ();
		total = TotalB.GetComponent<Image> ();

		//Sends default message out; including tutorial
		StartCoroutine( AnimateText ( enemy.className + " " + enemy.name + " would like to converse!"));
		ATB.GetComponent<Image> ().fillAmount = 0;
		Dialoguer.StartDialogue (0,DialoguerCallback);

		//Sets the enemy's health values as Dialoguer Global Variables
		//so that they can be altered through dialogue sequences
		Dialoguer.SetGlobalFloat (0, enemy.engagement);
		Dialoguer.SetGlobalFloat (1, enemy.energy);
		Dialoguer.SetGlobalFloat (2, enemy.esteem);
		endDialogueInitialized = false;

		engagementTracker.previousHealthPercentage = (enemy.engagement/enemy.maxEngagement);
		energyTracker.previousHealthPercentage = (enemy.energy/enemy.maxEnergy);
		esteemTracker.previousHealthPercentage = (enemy.esteem/enemy.maxEsteem);

		continueText.SetActive (false);

		//Initializes the inventory
		inventoryPanel.SetActive (false);
		inventoryManager = GameObject.Find ("InventoryManager");

		//Initializes the particle systems
		constantParticle.SetActive (true);
		constantEmission = constantParticle.GetComponent<ParticleSystem> ();

		victoryEmission = victoryParticle.GetComponent<ParticleSystem> ();
		victoryEmission.enableEmission = false;
		victoryParticle.SetActive (false);



	}

	void Update () {

		//Total bar indicated when battle is finished
		total.fillAmount = (enemy.getEngagement () + enemy.getEnergy () + enemy.getEsteem ())/2f;
		total2.fillAmount = ((enemy.getEnergy () + enemy.getEsteem ()))/2f;
		total3.fillAmount = ((enemy.getEsteem ()))/2f;

		//Increases size of the particles based on player performance
		constantEmission.startLifetime = (total.fillAmount*1.25f);

		//Keeps track of enem health bars
		engagement.fillAmount = enemy.getEngagement ();
		energy.fillAmount = enemy.getEnergy ();
		esteem.fillAmount = enemy.getEsteem ();


			//Checks to see if ATB timer has run out
			if (ATB.GetComponent<ATB_Bar> ().checkTurn () == false) {

				disableButtons (selection);
			//Player response handled here
			if (phase == 1 && !endDialogueInitialized) {
				
					PlayerAction (selection);
					continueText.SetActive (true);
					phase++;
				}
			//Enemy action handled here
			else if (phase == 3) {
				StopAllCoroutines ();
					//Gets enemy health changes and text
				if (selection != 3)
					StartCoroutine (AnimateText (enemy.getResponse (selection)));
				else if (selection == 3) {
					StartCoroutine (AnimateText (enemy.getResponse (selection)));
				}

					//Makes trackers visible
					engagementTracker.checkTrackerImage (enemy.getEngagement ());
					energyTracker.checkTrackerImage (enemy.getEnergy ());
					esteemTracker.checkTrackerImage (enemy.getEsteem ());
					continueText.SetActive (true);
					
					constantEmission.enableEmission = true;

					phase++;
				} else if (phase == 5) {

					//Updates dialoguer's global variables
					Dialoguer.SetGlobalFloat (0, enemy.engagement);
					Dialoguer.SetGlobalFloat (1, enemy.energy);
					Dialoguer.SetGlobalFloat (2, enemy.esteem);

					Debug.Log (phase);
					Dialoguer.StartDialogue (enemy.dialoguerResponseID, DialoguerCallback);

					phase++;
				}
			//Turn reset
			else if (phase >= 7) {
					//Hides trackers again
					engagementTracker.enabled = false;
					energyTracker.enabled = false;
					esteemTracker.enabled = false;

					//Performs turn reset
					ATB.GetComponent<Image> ().fillAmount = 1f;
					enableButtons ();
					selection = 0;
					phase = 1;

					//Updates enemy health bars to dialoguer setting
					enemy.engagement = Dialoguer.GetGlobalFloat (0);
					enemy.energy = Dialoguer.GetGlobalFloat (1);
					enemy.esteem = Dialoguer.GetGlobalFloat (2);

					//Updates trackers to compensate
					engagementTracker.previousHealthPercentage = (enemy.engagement/enemy.maxEngagement);
					energyTracker.previousHealthPercentage = (enemy.energy/enemy.maxEnergy);
					esteemTracker.previousHealthPercentage = (enemy.esteem/enemy.maxEsteem);

					constantEmission.enableEmission = false;
				}

				//allows the player to click to continue
				//Phase 6 is ommitted so that the player won't skip enemy dialogue
			if (Input.GetMouseButtonDown (0) && phase != 6 && battleLost () == false && battleWon() == false && total.fillAmount < 1f && !inventoryOpen) {
				
				if (!textAnimStarted) {
					phase++;
					continueText.SetActive (false);
				}				
					continuePressed = false;

					if (textAnimStarted) {
						continuePressed = true;
					}
				}
			}
		//Lose Condition
		if (battleLost () && endDialogueInitialized == false) {
			StopAllCoroutines ();
			ATB.GetComponent<Image> ().fillAmount = 0f;
			StartCoroutine( AnimateText (  "You've lost the exchange!"));
			Dialoguer.StartDialogue (15, finalDialoguerCallback);
			endDialogueInitialized = true;

			constantEmission.enableEmission = false;
			background.color = new Color (0.6f, 0.3f , 0.45f);
			music.loseSounds ();
		}
		//Win Condition
		if ( battleWon() && endDialogueInitialized == false) {
			StopAllCoroutines ();
			ATB.GetComponent<Image> ().fillAmount = 0f;
			StartCoroutine( AnimateText ("You've won the exchange!"));
			Dialoguer.StartDialogue (14, finalDialoguerCallback);
			endDialogueInitialized = true;

			constantEmission.enableEmission = false;
			victoryParticle.SetActive (true);
			victoryEmission.enableEmission = true;
			music.winSounds ();
			background.color = new Color (0.85f, 1f , 1f);
		}
	}
	

	//This function detects what the player has selected on their turn
	public void playerSelection(int x){
		if(x == 1){
			//Brown-Nosing
			selection = 1;
		}
		else if (x == 2) {
			//Bitch-Out
			selection = 2;
		}
		else if (x == 3) {
			//Barter
			selection = 3;
		}
		else if (x == 4) {
			//Blackmail
			selection = 4;
		}
		ATB.GetComponent<Image>().fillAmount = 0f;
	}

	//When the player turn is over, this function changes the text to reflect their action and alters the EEE bars accordingly
	 void PlayerAction(int x){
		StopAllCoroutines ();
		if (x == 0) {
			silentText = Random.Range (1, 6);
			if (silentText == 1) {
				StartCoroutine( AnimateText ("You stare meaningfully into their eyes, and say nothing."));
			}
			if (silentText == 2) {
				StartCoroutine( AnimateText ("You attempt to peer into their soul, but the effect is lost when you blink."));
			}
			if (silentText == 3) {
				StartCoroutine( AnimateText ("You try to stare meaningfully, but sneeze by accident. How embarassing."));
			}
			if (silentText == 4) {
				StartCoroutine( AnimateText ("You can't think of anything to say, so you say quiet and go for coolness instead."));
			}
			if (silentText == 5) {
				StartCoroutine( AnimateText ("You're having as hard a time thinking of what to say as I am of what to write."));
			}

		}
		else if (x == 1) {
			brownNoseText = Random.Range (1, 8);
			if (brownNoseText == 1) {
				StartCoroutine( AnimateText ("You praise them for their insight, ingenuity, and general 'in-ness.'"));
			}
			if (brownNoseText == 2) {
				StartCoroutine (AnimateText ("You insinuate that they ought to be running things around here."));
			}
			if (brownNoseText == 3) {
				StartCoroutine (AnimateText ("You float the idea of making them a godparent to your firstborn child."));
			}
			if (brownNoseText == 4) {
				StartCoroutine (AnimateText ("You compliment the feng shui of their cubicle."));
			}
			if (brownNoseText == 5) {
				StartCoroutine (AnimateText ("You jokingly propose marriage."));
			}
			if (brownNoseText == 6) {
				StartCoroutine (AnimateText ("You shake their hand and comment on their firm grip."));
			}
			if (brownNoseText == 7) {
				StartCoroutine (AnimateText ("You listen to their idea for a screenplay, then tell them it sounds brilliant."));
			}
		}
		else if (x == 2) {
			bitchOutText = Random.Range (1, 10);
			if (bitchOutText == 1) {
				StartCoroutine (AnimateText ("You inform them they've dropped their gay card, and offer to return it."));
			}
			if (bitchOutText == 2) {
				StartCoroutine (AnimateText ("You point at their shirt and laugh disparagingly."));
			}
			if (bitchOutText == 3) {
				StartCoroutine (AnimateText ("You call them a gutless milquetoast."));
			}
			if (bitchOutText == 4) {
				StartCoroutine (AnimateText ("You inform them that your dad could beat up their dad."));
			}
			if (bitchOutText == 5) {
				StartCoroutine (AnimateText ("You look them over, shake your head, and sigh."));
			}
			if (bitchOutText == 6) {
				StartCoroutine (AnimateText ("You fire off a stream of insults directed at their new haircut."));
			}
			if (bitchOutText == 7) {
				StartCoroutine (AnimateText ("You do an unflattering impression of their voice."));
			}
			if (bitchOutText == 8) {
				StartCoroutine (AnimateText ("You note that a color-blind clown would have a better wardrobe."));
			}
			if (bitchOutText == 9) {
				StartCoroutine (AnimateText ("You say 'your mom,' and smile proudly at your handiwork."));
			}
		}
		else if (x == 3) {
			
			StartCoroutine (AnimateText ("You open your bag and look for something to give them."));
			inventoryPanel.SetActive (true);
			inventoryOpen = true;

		}
		else if (x == 4) {
			blackmailText = Random.Range (1, 9);
			if (blackmailText == 1) {
				StartCoroutine( AnimateText ("You threaten to start playing Nightcore and Nickelback in the office everyday."));
			}
			if (blackmailText == 2) {
				StartCoroutine (AnimateText ("You threaten to steal their car and paint it neon orange."));
			}
			if (blackmailText == 3) {
				StartCoroutine (AnimateText ("You threaten to fill their next sandwich with non-USDA approved meat."));
			}
			if (blackmailText == 4) {
				StartCoroutine (AnimateText ("You threaten to replace their cat with an uglier, meaner cat."));
			}
			if (blackmailText == 5) {
				StartCoroutine (AnimateText ("You threaten to sneak into their home and steal all the juice."));
			}
			if (blackmailText == 6) {
				StartCoroutine (AnimateText ("You compliment their tie, and remark that it'd be a shame if something happened to it."));
			}
			if (blackmailText == 7) {
				StartCoroutine (AnimateText ("You mention that your uncle works for Google, and could totally ban them from the internet."));
			}
			if (blackmailText == 8) {
				StartCoroutine (AnimateText ("You threaten to kill him, but take it back when you realize how uninspired that is."));
			}
		}

	}
	 void disableButtons(int x){
		//disables player combat options when its not their turn
		bitchOut.GetComponent<Button>().interactable = false;
		barterWith.GetComponent<Button>().interactable = false;
		blackmail.GetComponent<Button>().interactable = false;
		brownNose.GetComponent<Button>().interactable = false;

		//gives the selected button color
		if(x == 1){brownNose.GetComponent<Image>().color = Color.magenta;}
		if(x == 2){bitchOut.GetComponent<Image>().color = Color.cyan;}
		if(x == 3){barterWith.GetComponent<Image>().color = Color.yellow;}
		if(x == 4){blackmail.GetComponent<Image>().color = Color.green ;}

	}
	 void enableButtons(){
		
		//resets the button colors after color change
		brownNose.GetComponent<Image>().color = Color.white;
		bitchOut.GetComponent<Image>().color = Color.white;
		barterWith.GetComponent<Image>().color = Color.white;
		blackmail.GetComponent<Image>().color = Color.white ;


		//re-enables player combat options when their turn resets
		//depending on what options are still chooseable after
		//the enemy turn action
		brownNose.GetComponent<Button>().interactable = Dialoguer.GetGlobalBoolean(0);
		bitchOut.GetComponent<Button>().interactable = Dialoguer.GetGlobalBoolean(1);
		barterWith.GetComponent<Button>().interactable = Dialoguer.GetGlobalBoolean(2);
		blackmail.GetComponent<Button>().interactable = Dialoguer.GetGlobalBoolean(3);
	}

	//Handles inventory selection
	public void itemSelection (int x){
		StopAllCoroutines ();
		barterText = x;
		inventoryPanel.SetActive (false);
		inventoryOpen = false;
		enemy.itemRecieved = barterText;

		if (barterText == 1) {
			StartCoroutine (AnimateText ("You hand them a bunch of paperwork, and then laugh maniacally."));
		} else if (barterText == 2) {
			StartCoroutine (AnimateText ("You show them some incriminating photos, and hope that the person they're incriminating is them."));
		} else if (barterText == 3) {
			StartCoroutine (AnimateText ("You hand the demon some holy water, and wait for their reaction."));
		} else {
			StartCoroutine (AnimateText ("You try to barter with them, but you have nothing to offer!"));
		}
	}

	//Makes sure that the battle doesn't progress
	//until the dialogue is finished
	private void DialoguerCallback(){
		phase++;
	}

	//Ends the battle by returning to Game over or victory scene
	private void finalDialoguerCallback(){
		screenFaderObject.SetActive (true);
		if (!battleLost()) {
			fade.FadeOut (3);
		}
		else if (battleLost ()) {
			fade.FadeOut (2);
		}
	}

	//Checks if the battle has been lost
	private bool battleLost(){
		bool lost = false;

		if (enemy.getEngagement () <= 0 || enemy.getEnergy () <= 0 || enemy.getEsteem () <= 0) {
			lost =  true;
		}
			return lost;
		
	}
	private bool battleWon(){
		bool won = false;

		if (total.fillAmount >= 1f) {
			won =  true;
		}
		return won;

	}

	//This displays button descriptions
	public void actionDescription(int x){

		if (x == 1 && brownNose.GetComponent<Button>().interactable) {
			StopAllCoroutines ();
			StartCoroutine (AnimateText ("Compliment them. Generally raises Esteem, but some people might not like it."));
		}
		if (x == 2 && bitchOut.GetComponent<Button>().interactable) {
			StopAllCoroutines ();
			StartCoroutine (AnimateText ("Insult them, in a good natured way. Generally raises Energy, but some might take offense."));
		}
		if (x == 3 && blackmail.GetComponent<Button>().interactable) {
			StopAllCoroutines ();
			StartCoroutine (AnimateText ("Coerce them, or send a threat. Generally raises Engagement, but some may get angry."));
		}
		if (x == 4 && barterWith.GetComponent<Button>().interactable) {
			StopAllCoroutines ();
			StartCoroutine (AnimateText ("Give them an item. Different items can have wildly differing effects."));
		}
	}
		

	//This co-routine displays text letter by letter to create a 
	//typewriter effect
	public IEnumerator AnimateText(string stringComplete){
		int i = 0;
		text.text = "";
		textAnimStarted = true;

		while (i < stringComplete.Length) {

			if (continuePressed) {
				text.text = stringComplete;
				textAnimStarted = false;
				break;
			}
			
			text.text += stringComplete[i++];
			yield return new WaitForSeconds (textAnimateSpeed);
		}
		textAnimStarted = false;
	}
}
