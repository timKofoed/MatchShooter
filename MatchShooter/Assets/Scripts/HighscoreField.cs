using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighscoreField : MonoBehaviour {

    public int highscoreIndex;
	public Text scoreFront;
	public Text scoreNameFront;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setScoreAndName(int score, string name)
	{
		scoreFront.text = score.ToString ();
		scoreNameFront.text = name;
	}
}
