using UnityEngine;
using System.Collections;

public class BattleMusic : MonoBehaviour {

	//The Various audio sources in the scene
	public AudioSource sfx1;
	public AudioSource sfx2;
	private AudioSource music;

	//These clips are used during the end condition
	public AudioClip winSting;
	public AudioClip loseSting;

	public AudioClip battleSong;
	public AudioClip winSong;
	public AudioClip loseSong;

	//Sets the volume of scene components based on options
	//from the title screen
	void Start () {
		music = this.GetComponent <AudioSource> ();

		music.volume = Dialoguer.GetGlobalFloat (3);
		sfx1.volume = Dialoguer.GetGlobalFloat (4);
		sfx2.volume = Dialoguer.GetGlobalFloat (4);

		DontDestroyOnLoad (this);
	}

	public void winSounds(){

		sfx1.Stop ();
		sfx1.clip = winSting;
		sfx1.Play ();

		music.Stop ();
		music.clip = winSong;
		music.Play ();
	}

	public void loseSounds(){

		sfx1.Stop ();
		sfx1.clip = loseSting;
		sfx1.Play ();

		music.Stop ();
		music.clip = loseSong;
		music.Play ();
	}
}
