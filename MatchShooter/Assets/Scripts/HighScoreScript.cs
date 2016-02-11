using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreScript : MonoBehaviour 
{
    //private GameController gameController;

	// Når man gemmer ting på harddisken via PlayerPrefs, så gemmer man ting i et "dictionary", som har en "key" og en "value"
	// En "key" er altid en string, som du kan se som et slags variabel-navn
	// En "value" er det som den indeholder (integer, float eller string)
	// Vi skal først definere nogle "keys", som ikke må blive ændret. De fortæller os hvad vores "variabler" hedder i PlayerPrefs.
	private const string highScoreOnDisk1         = "Score_1_Points";    // 'const' står for 'constant', hvilket betyder at der kun er én, og den ændrer sig aldrig
	private const string highScoreNameOnDisk1     = "Score_1_Name";
	
	private const string highScoreOnDisk2         = "Score_2_Points";
	private const string highScoreNameOnDisk2     = "Score_2_Name";
	
	private const string highScoreOnDisk3         = "Score_3_Points";
	private const string highScoreNameOnDisk3     = "Score_3_Name";

	private const string highScoreOnDisk4         = "Score_4_Points";
	private const string highScoreNameOnDisk4     = "Score_4_Name";

	private const string highScoreOnDisk5         = "Score_5_Points";
	private const string highScoreNameOnDisk5     = "Score_5_Name";

	private const string highScoreOnDisk6         = "Score_6_Points";
	private const string highScoreNameOnDisk6     = "Score_6_Name";

	private const string highScoreOnDisk7         = "Score_7_Points";
	private const string highScoreNameOnDisk7     = "Score_7_Name";

	private const string highScoreOnDisk8         = "Score_8_Points";
	private const string highScoreNameOnDisk8     = "Score_8_Name";

	private const string highScoreOnDisk9         = "Score_9_Points";
	private const string highScoreNameOnDisk9     = "Score_9_Name";

	private const string highScoreOnDisk10         = "Score_10_Points";
	private const string highScoreNameOnDisk10     = "Score_10_Name";

	public HighscoreField[] scoreFields;

	//I'm placing the strings into arrays so it will be easier to programmatically iterate through all of them
	private string[] highScoreNamesOnDisk;	//An array of the strings we defined above to find the highscore names on disk
	private string[] highScoresOnDisk;          //An array of the strings we defined above to find the highscore scores on disk

    public InputField inputNameFromUser;    // The InputField in which the user will give his name, when we´ve beaten a score
    public GameObject touchToStart;     // The GameObject which starts the game

	void Start () 
	{
        // Find the GameObject in the scene, and then get the component, which is the script we made (GameController)
        //gameController = (GameObject.FindGameObjectWithTag("controller") as GameObject).GetComponent<GameController>();

        highScoreNamesOnDisk = new string[10];
		highScoresOnDisk = new string[10];

		//Først skal vi sørge for at vores "keys" findes, og at vi kan hente fra dem
		if( !PlayerPrefs.HasKey(highScoreOnDisk1) )        //Hvis der ikke er en "key" med dette navn, så lav en ny tom key med det navn
			PlayerPrefs.SetInt(highScoreOnDisk1, 0);
        highScoresOnDisk[0] = highScoreOnDisk1;
		
		if( !PlayerPrefs.HasKey(highScoreNameOnDisk1) )    
			PlayerPrefs.SetString(highScoreNameOnDisk1, "N/A");
		highScoreNamesOnDisk [0] = highScoreNameOnDisk1;
		
		if( !PlayerPrefs.HasKey(highScoreOnDisk2) )    
			PlayerPrefs.SetInt(highScoreOnDisk2, 0);
		highScoresOnDisk [1] = highScoreOnDisk2;
		
		if( !PlayerPrefs.HasKey(highScoreNameOnDisk2) )    
			PlayerPrefs.SetString(highScoreNameOnDisk2, "N/A");
		highScoreNamesOnDisk [1] = highScoreNameOnDisk2;
		
		if( !PlayerPrefs.HasKey(highScoreOnDisk3) )    
			PlayerPrefs.SetInt(highScoreOnDisk3, 0);
		highScoresOnDisk [2] = highScoreOnDisk3;
		
		if( !PlayerPrefs.HasKey(highScoreNameOnDisk3) )    
			PlayerPrefs.SetString(highScoreNameOnDisk3, "N/A");
		highScoreNamesOnDisk [2] = highScoreNameOnDisk3;

		if( !PlayerPrefs.HasKey(highScoreOnDisk4) )    
			PlayerPrefs.SetInt(highScoreOnDisk4, 0);
		highScoresOnDisk [3] = highScoreOnDisk4;
		
		if( !PlayerPrefs.HasKey(highScoreNameOnDisk4) )    
			PlayerPrefs.SetString(highScoreNameOnDisk4, "N/A");
		highScoreNamesOnDisk [3] = highScoreNameOnDisk4;

		if( !PlayerPrefs.HasKey(highScoreOnDisk5) )    
			PlayerPrefs.SetInt(highScoreOnDisk5, 0);
		highScoresOnDisk [4] = highScoreOnDisk5;
		
		if( !PlayerPrefs.HasKey(highScoreNameOnDisk5) )    
			PlayerPrefs.SetString(highScoreNameOnDisk5, "N/A");
		highScoreNamesOnDisk [4] = highScoreNameOnDisk5;

		if( !PlayerPrefs.HasKey(highScoreOnDisk6) )    
			PlayerPrefs.SetInt(highScoreOnDisk6, 0);
		highScoresOnDisk [5] = highScoreOnDisk6;
		
		if( !PlayerPrefs.HasKey(highScoreNameOnDisk6) )    
			PlayerPrefs.SetString(highScoreNameOnDisk6, "N/A");
		highScoreNamesOnDisk [5] = highScoreNameOnDisk6;

		if( !PlayerPrefs.HasKey(highScoreOnDisk7) )    
			PlayerPrefs.SetInt(highScoreOnDisk7, 0);
		highScoresOnDisk [6] = highScoreOnDisk7;
		
		if( !PlayerPrefs.HasKey(highScoreNameOnDisk7) )    
			PlayerPrefs.SetString(highScoreNameOnDisk7, "N/A");
		highScoreNamesOnDisk [6] = highScoreNameOnDisk7;

		if( !PlayerPrefs.HasKey(highScoreOnDisk8) )    
			PlayerPrefs.SetInt(highScoreOnDisk8, 0);
		highScoresOnDisk [7] = highScoreOnDisk8;
		
		if( !PlayerPrefs.HasKey(highScoreNameOnDisk8) )    
			PlayerPrefs.SetString(highScoreNameOnDisk8, "N/A");
		highScoreNamesOnDisk [7] = highScoreNameOnDisk8;

		if( !PlayerPrefs.HasKey(highScoreOnDisk9) )    
			PlayerPrefs.SetInt(highScoreOnDisk9, 0);
		highScoresOnDisk [8] = highScoreOnDisk9;
		
		if( !PlayerPrefs.HasKey(highScoreNameOnDisk9) )    
			PlayerPrefs.SetString(highScoreNameOnDisk9, "N/A");
		highScoreNamesOnDisk [8] = highScoreNameOnDisk9;

		if( !PlayerPrefs.HasKey(highScoreOnDisk10) )    
			PlayerPrefs.SetInt(highScoreOnDisk10, 0);
		highScoresOnDisk [9] = highScoreOnDisk10;
		
		if( !PlayerPrefs.HasKey(highScoreNameOnDisk10) )    
			PlayerPrefs.SetString(highScoreNameOnDisk10, "N/A");
		highScoreNamesOnDisk [9] = highScoreNameOnDisk10;

		UpdateScoreTextFields ();
	}

	int[] oldScores;
	string[] oldNames;
	
	int[] newScores;
	string[] newNames;

    private HighscoreField scoreFieldToUpdate;

    /// <summary>
    /// If we have a score field active, then we´re giving it a name
    /// </summary>
    /// <returns></returns>
    public bool isNameTypingInProgress()
    {
        return (scoreFieldToUpdate != null);
    }

    //Når vi har en ny score, så skal vi hente alle de scores (og navne) fra harddisken, og sammenligne med vores nye score
    public void UpdateScoreOnDisk(int myScore, string myName)
	{
		oldScores = new int[10];
		oldNames = new string[10];

		newScores = new int[10];
		newNames = new string[10];

        int scoreFieldIndexWeBeat = -1;
        
		for (int i = 0; i < 10; i++) 
		{
			oldScores[i] = PlayerPrefs.GetInt(highScoresOnDisk[i]);
			oldNames[i] = PlayerPrefs.GetString(highScoreNamesOnDisk[i]);
			newScores[i] = oldScores[i];
			newNames[i] = oldNames[i];
		}

        // 0 is HIGHEST score, so we start looking at the LOWEST score first. If we beat it, then we move up the line.
        for (int i = scoreFields.Length - 1; i >= 0; i--)
        {
			//Both check the score, and if we did NOT beat the score, then stop checking. If we DID beat the score, then keep going down the list
			//NOTE: the array is backwards, so index 0 is the score at the top of the list, and the index at .length-1 is the last one on the highscore
			if( !didWeBeatTheScore(i, myScore))
                break;
            else
                scoreFieldIndexWeBeat = i;
        }

        //Remember which highscore place we beat, so we can ask the user for his name
        if (scoreFieldIndexWeBeat != -1)
        {
            scoreFieldToUpdate = scoreFields[scoreFieldIndexWeBeat];    //and I need to keep the index for the newNames[] to store the name on disk
            scoreFieldToUpdate.scoreFront.text = myScore.ToString();
            scoreFieldToUpdate.scoreNameFront.text = "Enter Name";
            scoreFieldToUpdate.highscoreIndex = scoreFieldIndexWeBeat; //saving the index to the field´s tag
            Debug.Log("We beat the score. Now give me your name!");
        }
        else
            SaveHighscoreToDisk();  //we didn´t 

    }

    private void SaveHighscoreToDisk(string nameToSave = "", int nameIndex = -1, int scoreToSave = -1)
    {
        bool nameSaved = false;
        bool scoreSaved = false;

        string debugString = "";

        if (nameIndex >= 0 && nameToSave != "")
        {
            // Save the newly made name to disk at the correct index
            PlayerPrefs.SetString(highScoreNamesOnDisk[nameIndex], nameToSave);
            newNames[nameIndex] = nameToSave;
            nameSaved = true;
        }

        if (nameIndex >= 0 && scoreToSave >= 0)
        {
            PlayerPrefs.SetInt(highScoresOnDisk[nameIndex], scoreToSave);
            newScores[nameIndex] = scoreToSave;
            scoreSaved = true;
        }

        Debug.Log("saving ("+nameToSave+")("+scoreToSave+")");

        //save the new scores to disk
        for (int i = 0; i < scoreFields.Length; i++)
        {
            debugString += "("+i+") ("+newNames[i]+") ("+newScores[i]+")\n";
            //if(scoreSaved)
                PlayerPrefs.SetInt(highScoresOnDisk[i], newScores[i]);

            //if (nameSaved)
                PlayerPrefs.SetString(highScoreNamesOnDisk[i], newNames[i]);
        }
        Debug.Log(debugString);
        //update the scores in the visual Text fields on screen first, because it's faster to do
        UpdateScoreTextFields();

        ResumeGame();
    }

    private void ResumeGame()
    {
        // Activate the button to restart the game
        if (touchToStart != null)
            touchToStart.SetActive(true);

        // Hide the highscore and all highscore fields
        this.gameObject.SetActive(false);
    }

	public bool didWeBeatTheScore(int scoreIndexToCheck, int newScore)
	{
		Debug.Log ("newScore("+newScore+") scoreIndexToCheck("+scoreIndexToCheck+") ScoreOnDisk ("+newScores [scoreIndexToCheck]+")");
        //Se om vi har slået den scoren på denne plads
        if (newScore > newScores[scoreIndexToCheck]) 
		{
			//Vi har slået denne værdi, så FØRST skub dens værdier til den næste score før vi sætter vores værdier ind
            if (scoreIndexToCheck + 1 < scoreFields.Length)    //Er der en score under den vi lige har slået? Hvis ja, så skub nuværende score ét felt ned
            {
                newScores[scoreIndexToCheck + 1] = newScores[scoreIndexToCheck];
                newNames[scoreIndexToCheck + 1] = newNames[scoreIndexToCheck];
            }

			//Indsæt vores navn og score
			newScores [scoreIndexToCheck] = newScore;
			newNames [scoreIndexToCheck] = "PlayerX";

			return true;
		} 
		else 
		{
			return false;
		}

	}

	public void UpdateScoreTextFields()
	{
		for (int i = 0; i < scoreFields.Length; i++) 
		{
            scoreFields[i].setScoreAndName(PlayerPrefs.GetInt(highScoresOnDisk[i]), PlayerPrefs.GetString(highScoreNamesOnDisk[i]));
        }
	}

    void Update()
    {
        // We have a field ready, so we need to show the InputField
        if (scoreFieldToUpdate != null)
        {
            // If we haven´t enabled the user-input field yet, so we need to activate it
            if (inputNameFromUser != null && (!inputNameFromUser.enabled || !inputNameFromUser.gameObject.activeSelf))
            {
                //gameController.SetMainScreenUI(MainScreenController.UIState.InputName);
                inputNameFromUser.gameObject.SetActive(true);
                inputNameFromUser.enabled = true;
            }
        }
        else if (inputNameFromUser != null && scoreFieldToUpdate == null && inputNameFromUser.gameObject.activeSelf)
        {
            // If we don´t have a score to update, but the InputField is active, then we need to hide the field
            inputNameFromUser.gameObject.SetActive(false);
            //gameController.SetMainScreenUI(MainScreenController.UIState.Highscore);
        }

    }

    /// <summary>
    /// Update the name while we´re typing into the field
    /// </summary>
    /// <param name="otherField"></param>
    public void UpdateNameFromUser(InputField otherField)
    {
        // We have a field ready, so we need to update that field based on what has been typed
        if (scoreFieldToUpdate != null)
        {
            scoreFieldToUpdate.scoreNameFront.text = otherField.text;
        }
    }

    /// <summary>
    /// Disable the input field when we´re done typing the name AND save the name to disk
    /// </summary>
    /// <param name="otherField"></param>
    public void RemoveInputField(InputField otherField)
    {
        // Save the newly made name to disk at the correct index with the rest of the updated scores
        SaveHighscoreToDisk(scoreFieldToUpdate.scoreNameFront.text, scoreFieldToUpdate.highscoreIndex);
        //PlayerPrefs.SetString(highScoreNamesOnDisk[scoreFieldToUpdate.highscoreIndex], scoreFieldToUpdate.scoreNameFront.text);

        UpdateScoreTextFields();

        scoreFieldToUpdate = null;

        // Fjern det navn fra input-feltet som vi lige har skrevet
        otherField.text = "";

        // Disable the inputField
        //otherField.gameObject.SetActive(false);
        //gameController.SetMainScreenUI(MainScreenController.UIState.Highscore);
    }
	
}
