using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryPanel : MonoBehaviour {

	//Counts the numer of each item available
	public Text PWCount;
	public Text HWCount;
	public Text IPCount;

	//Holds the buttons
	public Button pw;
	public Button ip;
	public Button hw;

	//Battle and inventory managers
	public BattleManager battle;
	public InventoryManager inventory;

	//Holds the battle log, so it can show descriptions
	public Text battleLog;

	// Use this for initialization
	void Start () {
		inventory = GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		PWCount.text = "x" + inventory.paperwork.ToString ();
		IPCount.text = "x" + inventory.incriminatingPhotos.ToString ();
		HWCount.text = "x" + inventory.holyWater.ToString ();

		if (inventory.paperwork <= 0) {
			pw.enabled = false;	
		}
		if (inventory.incriminatingPhotos <= 0) {
			ip.enabled = false;
		}
		if (inventory.holyWater  <= 0) {
			hw.enabled = false;
		}
	}

	public void showDescription(int x){
		StopAllCoroutines ();

		if (x == 0) {
			StartCoroutine (AnimateText ("Give them nothing, and hope they don't get upset."));
		}
		if (x == 1) {
			StartCoroutine (AnimateText (inventory.pwDescription));
		}
		if (x == 2) {
			StartCoroutine (AnimateText (inventory.ipDescription));
		}
		if (x == 3) {
			StartCoroutine (AnimateText (inventory.hwDescription));
		}

	}

	public void pickItem(int x){

		if (x == 1 && inventory.paperwork > 0) {
			inventory.paperwork -= 1;
			battle.itemSelection (1);
		}
		else if (x == 2 && inventory.incriminatingPhotos > 0) {
			inventory.incriminatingPhotos -= 1;
			battle.itemSelection (2);
		}
		else if (x == 3 && inventory.holyWater > 0) {
			inventory.holyWater -= 1;
			battle.itemSelection (3);
		} else if (x==0){
			battle.itemSelection (0);
		}

	}

	public IEnumerator AnimateText(string stringComplete){
		int i = 0;
		battleLog.text = "";
		while (i < stringComplete.Length) {
			battleLog.text += stringComplete[i++];
			yield return new WaitForSeconds (battle.textAnimateSpeed);
		}
	}
}
