using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighscoreField : MonoBehaviour {

    public int highscoreIndex;
	public Text scoreBack;
	public Text scoreFront;
	public Text scoreNameBack;
	public Text scoreNameFront;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setScoreAndName(int score, string name)
	{
		scoreBack.text = score.ToString ();
		scoreFront.text = score.ToString ();

		scoreNameBack.text = name;
		scoreNameFront.text = name;
	}
}
