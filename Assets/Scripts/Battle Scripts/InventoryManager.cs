using UnityEngine;
using System.Collections;

public class InventoryManager : MonoBehaviour {

	//These integers keep track of which items the player has and how many
	public int paperwork = 1;
	public int incriminatingPhotos = 1;
	public int holyWater = 1;
	public int bible;

	//These strings hold the description of each
	public string pwDescription = "A charm to ward off rogue accountants and marauding lawyers.";
	public string ipDescription = "Not sure who's actually IN the photo, but it's probably incriminating someone.";
	public string hwDescription = "Or, as its colloquially known here: 'Satan's Mouthwash.'";
	public string bbDescription = "Good for thumping.";

	// Ensures inventory is persistant
	void Start () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
