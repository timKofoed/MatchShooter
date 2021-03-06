﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;   // UI elementer i Canvas

public enum ObjektType
{
    objekt1 = 0,
    objekt2 = 1,
    objekt3 = 2,
    objekt4 = 3
}

public enum WeaponType
{
    none = -1,
    vulcan = 0,
    pulse = 1,
    lightning = 2,
    flamer = 3
}

public enum ShopItem
{
	vulcan = 0,
	pulse = 1,
	lightning = 2,
	flamer = 3,
	life = 4
}

public class SceneController : MonoBehaviour {
    
	public static SceneController instance;

	[System.Serializable]
    public struct FaldendeObjekt
    {
        public float spawnChange;
        public GameObject prefab;
    }

    public GameObject faldendeObjektPrefab;
    [SerializeField]
    public List<FaldendeObjekt> faldendeObjekterPrefabs;

    private Vector3 startPosition;

    [SerializeField]
    private Transform slutPosition; // Hvor fjenderne bevæger sig imod

    public EnemySpawnPoints spawnPointsScript;
    public int liv = 5;
    private int score = 0;
    public ObjektType valgteObjektType = ObjektType.objekt1;
    public HighScoreScript highscoreScript;

    public GameObject environmentRoot;
    public GameObject environmentRootClouds;
    public GameObject environmentRootContours;

    public Button knap1;
    public Button knap2;
    public Button knap3;
    public Button knap4;

    public Color knap1Normal;
    public Color knap1Tryk;
    public Color knap2Normal;
    public Color knap2Tryk;
    public Color knap3Normal;
    public Color knap3Tryk;
    public Color knap4Normal;
    public Color knap4Tryk;

    public Gradient gradientLiv;

    private List<Transform> fallingObjects;

    // Det preview i øverst venstre hjørne, som viser hvad vi har trykket på lige nu
    public GameObject objekt1Preview;
    //public GameObject objekt2Preview;
    //public GameObject objekt3Preview;
    //public GameObject objekt4Preview;
    public Image objekt1PreviewIcon;

    public Text scoreFelt;
    public Image livFelt;
    public Image fjendeLivFelt;
    public Image fjendeIkon;

    private float startBreddeForLivBar;
    private int startLiv;

    private Coroutine objektGenerator;

    public Button pressToStart;

    public GameObject playerGun, shopHolder;
    public Transform playerGunLookAt;
    private GameObject objectToLookAt;  //Gem en reference til det fallende objekt som våbnet skal kigge på
    private Quaternion gunPreviousOrientation; //Husk hvor våbnet pegede hen før vi startede med at dreje det
    private float gunTransition = -1.0f; //en værdi vi bruger til at holde styr på hvor langt våbnet har drejet imod objektet (bruges af en Lerp)
    private GunController gunController;

    private WeaponType selectedWeapon = WeaponType.vulcan;

    // Use this for initialization
    void Start () {
		instance = this;
        RestartLevel();
        Debug.Log("Time.fixedDeltaTime: " + Time.fixedDeltaTime);

        gunController = playerGun.GetComponent<GunController>();

        // Når vi starter spillet skal vi sørge for at alle knapperne er farvet korrekt
        ColorBlock knap1Farve = knap1.colors;
        knap1Farve.normalColor = knap1Normal;
        knap1.colors = knap1Farve;
        knap1.GetComponent<Image>().color = knap1Normal;

        ColorBlock knap2Farve = knap2.colors;
        knap2Farve.normalColor = knap2Normal;
        knap2.colors = knap2Farve;
        knap2.GetComponent<Image>().color = knap2Normal;

        ColorBlock knap3Farve = knap3.colors;
        knap3Farve.normalColor = knap3Normal;
        knap3.colors = knap3Farve;
        knap3.GetComponent<Image>().color = knap3Normal;

        ColorBlock knap4Farve = knap4.colors;
        knap4Farve.normalColor = knap4Normal;
        knap4.colors = knap4Farve;
        knap4.GetComponent<Image>().color = knap4Normal;

        StartCoroutine(RotateEnvironment());

        // Quick-fix, so the initial weapon has proper damage, before we press any of the buttons in the side
        SetDamagePotential(10.0f);

		knap1.GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0.3f);
		knap2.GetComponent<Image> ().color = new Color (0.259f, 0.259f, 0.259f, 0.3f);
		knap3.GetComponent<Image> ().color = new Color (0.259f, 0.259f, 0.259f, 0.3f);
		knap4.GetComponent<Image> ().color = new Color (0.259f, 0.259f, 0.259f, 0.3f);

		knap1.GetComponent<Button> ().interactable = true;
		knap2.GetComponent<Button> ().interactable = false;
		knap3.GetComponent<Button> ().interactable = false;
		knap4.GetComponent<Button> ().interactable = false;
    }

    public void SetDamagePotential(float damage)
    {
        gunController.SetDamagePotential(damage);
    }

    public WeaponType GetSelectedWeapon()
    {
        return selectedWeapon;
    }

    public void AddToScore(int addScore)
    {
        score += addScore;
    }

    float contourFlare = 0.0f;
    Color contourColor;
    bool contourIncreasing = true;  // Determines if we're increasing or decreasing the highlights
    private IEnumerator RotateEnvironment()
    {
        contourColor = new Color(1.0f, 1.0f, 1.0f);
        while(true)
        {
            if (contourIncreasing)
            {
                contourFlare += 0.01f;
                if(contourFlare > 1.0f)
                {
                    contourFlare = 1.0f;
                    contourIncreasing = false;
                }
            }
            else
            {
                contourFlare -= 0.01f;
                if (contourFlare < 0.0f)
                {
                    contourFlare = 0.0f;
                    contourIncreasing = true;
                }
            }
                
            float r = 1.0f;
            float g = Mathf.Clamp(contourFlare * 0.8f, 0.3f, 0.8f) ;
            float b = Mathf.Clamp(contourFlare * 0.8f, 0.3f, 0.8f);
            contourColor.r = r;
            contourColor.g = g;
            contourColor.b = b;

            //mat.SetColor("_EmissionColor", finalColor);
            if(environmentRootContours != null)
                environmentRootContours.GetComponent<Renderer>().material.SetColor("_EmissionColor", contourColor);

            if(environmentRoot != null)
            {
                // Rotate the planet
                environmentRoot.transform.Rotate(0.0f, 0.015f, 0.0f);
                
                // Rotate the contours with the planet
                if (environmentRootContours != null)
                    environmentRootContours.transform.Rotate(0.0f, 0.015f, 0.0f);
            }
            

            if(environmentRootClouds != null)
            {
                // Rotate the clouds
                environmentRootClouds.transform.Rotate(0.005f, 0.01f, 0.012f);
            }
                
            yield return new WaitForEndOfFrame();
        }
        
    }

    // Når spillet ikke er aktivt, så fjern alle aktive objekter på skærmen
    public bool ErSpilletAktivt()
    {
        if (pressToStart.gameObject.activeSelf || highscoreScript.isActiveAndEnabled)
            return false;
        else
            return true;
    }

	public void PauseKnapTrykket(int pauseState = -1)
    {
		if (pauseState < 0) 
		{
			// Hvis tiden går hurtigere end "meget langsom", så stop tiden
			if (Time.timeScale > 0.0f)
				Time.timeScale = 0.0f;
			else //...ellers sæt tiden tilbage til standarden
				Time.timeScale = 1.0f;
		}
		else if(pauseState == 0)
		{
			Time.timeScale = 0.0f;
		}
		else
		{
			Time.timeScale = 1.0f;
		}
        
    }

	public void GotoShop()
	{
		PauseKnapTrykket (0);
		shopHolder.SetActive (true);
	}

	public void CloseShop()
	{
		PauseKnapTrykket (1);
		shopHolder.SetActive (false);
	}

	public bool PurchaseItem(ShopItem item, int cost)
	{
		Debug.Log ("køb ("+item+"), cost: " + cost);
		if ((score >= cost) && (liv < startLiv)) 
		{
			Debug.Log ("Ja");
			score -= cost;
			switch (item) 
			{
			case ShopItem.life:
				liv += 5;
				liv = (liv > startLiv)?startLiv:liv;
				break;
				default:
					Debug.Log ("unknown!");
				break;
			}
			return true;
		} 
		else
			return false;
	}

	public bool PurchaseGun(WeaponType gun, int cost)
	{
		if (score >= cost) 
		{
			score -= cost;
			switch (gun) {
			case WeaponType.vulcan:
				knap1.GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0.3f);
				knap1.GetComponent<Button> ().interactable = true;
				break;
			case WeaponType.pulse:
				knap2.GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0.3f);
				knap2.GetComponent<Button> ().interactable = true;
				break;
			case WeaponType.lightning:
				knap3.GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0.3f);
				knap3.GetComponent<Button> ().interactable = true;
				break;
			case WeaponType.flamer:
				knap4.GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0.3f);
				knap4.GetComponent<Button> ().interactable = true;
				break;
			default:
				break;
			}
			return true;
		} else
			return false;
	}
	
    public void SaetValgtVaaben(WeaponType valgtVaaben, ButtonChoose pressedButton)
    {
        selectedWeapon = valgtVaaben;
        objekt1PreviewIcon.sprite = pressedButton.GetIcon();
        switch (valgtVaaben)
        {
            case WeaponType.vulcan:
                F3DFXController.instance.DefaultFXType = F3DFXType.Vulcan;
                break;
            case WeaponType.pulse:
                F3DFXController.instance.DefaultFXType = F3DFXType.PlasmaGun;
                break;
            case WeaponType.lightning:
                F3DFXController.instance.DefaultFXType = F3DFXType.LightningGun;
                break;
            case WeaponType.flamer:
                F3DFXController.instance.DefaultFXType = F3DFXType.FlameRed;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Sæt den valgte værdi og highlight knappen
    /// </summary>
    public void SaetValgtObjekt(int minVaerdi)
    {
        valgteObjektType = (ObjektType)minVaerdi;

        if(valgteObjektType == ObjektType.objekt1)
        {
            // Vi har trykket på Objekt1 i siden af skærmen

            // Tænd for knap1 farven
            ColorBlock newColour1 = knap1.colors;
            newColour1.normalColor = knap1Tryk;
            knap1.colors = newColour1;
            knap1.GetComponent<Image>().color = knap1Tryk;
            objekt1Preview.GetComponent<Image>().color = knap1Tryk;

            // Gør de andre knapper hvide
            ColorBlock newColour2 = knap2.colors;
            newColour2.normalColor = knap2Normal;
            knap2.colors = newColour2;
            knap2.GetComponent<Image>().color = knap2Normal;

            ColorBlock newColour3 = knap3.colors;
            newColour3.normalColor = knap3Normal;
            knap3.colors = newColour3;
            knap3.GetComponent<Image>().color = knap3Normal;

            ColorBlock newColour4 = knap4.colors;
            newColour4.normalColor = knap4Normal;
            knap4.colors = newColour4;
            knap4.GetComponent<Image>().color = knap4Normal;
        }
        else if (valgteObjektType == ObjektType.objekt2)
        {
            // Vi har trykket på Objekt2 i siden af skærmen

            ColorBlock newColour1 = knap1.colors;
            newColour1.normalColor = knap1Normal;
            knap1.colors = newColour1;
            knap1.GetComponent<Image>().color = knap1Normal;

            // Tænd for knap2 farven
            ColorBlock newColour2 = knap2.colors;
            newColour2.normalColor = knap2Tryk;
            knap2.colors = newColour2;
            knap2.GetComponent<Image>().color = knap2Tryk;
            objekt1Preview.GetComponent<Image>().color = knap2Tryk;

            ColorBlock newColour3 = knap3.colors;
            newColour3.normalColor = knap3Normal;
            knap3.colors = newColour3;
            knap3.GetComponent<Image>().color = knap3Normal;

            ColorBlock newColour4 = knap4.colors;
            newColour4.normalColor = knap4Normal;
            knap4.colors = newColour4;
            knap4.GetComponent<Image>().color = knap4Normal;
        }
        else if (valgteObjektType == ObjektType.objekt3)
        {
            // Vi har trykket på Objekt3 i siden af skærmen

            ColorBlock newColour1 = knap1.colors;
            newColour1.normalColor = knap1Normal;
            knap1.colors = newColour1;
            knap1.GetComponent<Image>().color = knap1Normal;

            ColorBlock newColour2 = knap2.colors;
            newColour2.normalColor = knap2Normal;
            knap2.colors = newColour2;
            knap2.GetComponent<Image>().color = knap2Normal;

            // Tænd for knap3 farven
            ColorBlock newColour3 = knap3.colors;
            newColour3.normalColor = knap3Tryk;
            knap3.colors = newColour3;
            knap3.GetComponent<Image>().color = knap3Tryk;
            objekt1Preview.GetComponent<Image>().color = knap3Tryk;

            ColorBlock newColour4 = knap4.colors;
            newColour4.normalColor = knap4Normal;
            knap4.colors = newColour4;
            knap4.GetComponent<Image>().color = knap4Normal;
        }
        else if (valgteObjektType == ObjektType.objekt4)
        {
            // Vi har trykket på Objekt4 i siden af skærmen

            ColorBlock newColour1 = knap1.colors;
            newColour1.normalColor = knap1Normal;
            knap1.colors = newColour1;
            knap1.GetComponent<Image>().color = knap1Normal;

            ColorBlock newColour2 = knap2.colors;
            newColour2.normalColor = knap2Normal;
            knap2.colors = newColour2;
            knap2.GetComponent<Image>().color = knap2Normal;

            ColorBlock newColour3 = knap3.colors;
            newColour3.normalColor = knap3Normal;
            knap3.colors = newColour3;
            knap3.GetComponent<Image>().color = knap3Normal;

            // Tænd for knap4 farven
            ColorBlock newColour4 = knap4.colors;
            newColour4.normalColor = knap4Tryk;
            knap4.colors = newColour4;
            knap4.GetComponent<Image>().color = knap4Tryk;
            objekt1Preview.GetComponent<Image>().color = knap4Tryk;
        }
    }

    IEnumerator LavObjektInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds( Random.Range(1.0f, 2.5f) );

            if (fallingObjects == null)
                fallingObjects = new List<Transform>();

            startPosition = spawnPointsScript.GetSpawnPoint();

            
            int objCount = Random.Range(0, faldendeObjekterPrefabs.Count);

            // Hvis vi vælger ikke at lave dette objekt pga den tilfældige chance, så vælger vi et nyt tal
            while(faldendeObjekterPrefabs[objCount].spawnChange < Random.value)
                objCount = Random.Range(0, faldendeObjekterPrefabs.Count);

            // Lav et nyt objekt af vores prefab (og sørg for at det er tændt)
            fallingObjects.Add( (GameObject.Instantiate(faldendeObjekterPrefabs[objCount].prefab, startPosition, Quaternion.identity) as GameObject).transform);

            // Sæt objektets mål til at være den placering vi har valgt - eller sæt målet til at være MainCamera
            (fallingObjects[fallingObjects.Count - 1]).GetComponent<ObjectController>().Init( (slutPosition != null) ? slutPosition : Camera.main.transform );

            fallingObjects[fallingObjects.Count-1].gameObject.SetActive(true);
        }
    }

    public void RemoveObject(GameObject objectToRemove)
    {
        fallingObjects.Remove(objectToRemove.transform);

        // Slet det GameObject, fordi vi nu er færdige med det
        Destroy(objectToRemove);
    }

    public void MistLiv(int livMistet)
    {
        liv -= livMistet;

        if (liv <= 0)
            GameOver();
    }

    public void GameOver()
    {
        // Hvis objekt generatoren allerede kører, så stop den
        if (objektGenerator != null)
            StopCoroutine(objektGenerator);

        // Vi bør vise Highscore og skrive navn
        StartCoroutine(StartHighscore(score, ""));

        // Så tryk på knap for at starte spillet igen

        // DEBUG: reset med det samme
        //RestartLevel();

        //Vent med at vise "Press to start" indtil vi har indsat vores navn i Highscore feltet
        //pressToStart.gameObject.SetActive(true);
    }

    private IEnumerator StartHighscore(int playerScore, string playerName)
    {
        // Tænd for highscore scriptet
        if (!highscoreScript.gameObject.activeSelf)
            highscoreScript.gameObject.SetActive(true);

        // Vent en update-cyklus, så HighscoreScript´et har tid til at kalde Start()
        yield return new WaitForEndOfFrame();

        // Opdatér highscore felterne til de nyeste FØR vi viser den
        highscoreScript.UpdateScoreTextFields();

        // Send vores nye score til highscoreScript´et
        highscoreScript.UpdateScoreOnDisk(playerScore, playerName);
    }

    public void RestartLevel()
    {
        // Hvis objekt generatoren allerede kører, så stop den
        if (objektGenerator != null)
            StopCoroutine( objektGenerator );

        // Sørg for at PressToStart´en er slukket, hvis nu den var tændt da spillet startede
        pressToStart.gameObject.SetActive(false);

        // Sørg for at Highscore´en er slukket, hvis nu den var tændt da spillet startede
        highscoreScript.gameObject.SetActive(false);

        // Begynd at lave objekter i et interval
        objektGenerator = StartCoroutine(LavObjektInterval());

        // Husk hvor bred liv-baren var da vi startede
        if (startBreddeForLivBar == 0.0f)
            startBreddeForLivBar = livFelt.GetComponent<RectTransform>().sizeDelta.x;
        else
            livFelt.GetComponent<RectTransform>().sizeDelta = 
                new Vector2( startBreddeForLivBar, livFelt.GetComponent<RectTransform>().sizeDelta.y);

        score = 0;

        // Husk hvor mange liv vi havde da vi startede
        if (startLiv == 0)
            startLiv = liv;
        else
            liv = startLiv;
    }

    public bool HarTrykketPaaObjekt(ObjektType objektViTrykkedePaa, GameObject objekt)
    {
        // Hvis vi har trykket på den rigtige objekttype, og spillet ikke er pauset, så giv point
        if ((objektViTrykkedePaa == valgteObjektType) && (Time.timeScale > 0.0f))
        {
            // Vi gemmer en reference til det objekt vi vil kigge på, så vi kan blive ved med at pege på det
            objectToLookAt = objekt;

            playerGunLookAt.LookAt(objekt.transform);

            //nulstil værdien vi bruger til at dreje våbnet
            gunTransition = 0.0f;

            // Husk hvor våbnet pegede hen da vi trykkede på et objekt
            gunPreviousOrientation = playerGun.transform.rotation;

            // Begynd at rotér våbnet
            if (gunController != null)
            {
                // Fortæl våbnet hvor fjenden er, så vi kan sende projektiler mod den
                gunController.SetProjectileEndPosition(objectToLookAt.transform);
                gunController.StartShooting();
            }
                

            score += 5;
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void EnemyReceivedDamage(Sprite icon, float healthPercent)
    {
        if(fjendeLivFelt != null)
        {
            fjendeLivFelt.transform.localScale = new Vector3(healthPercent, 1.0f, 1.0f);
        }
        if(fjendeIkon != null && icon != null)
        {
            fjendeIkon.sprite = icon;
        }
    }

    void FixedUpdate()
    {
        // Vi har en reference til et objekt som vi vil pege våbnet imod
        if(gunTransition <= 1.0f && gunTransition >= 0.0f)
        {
            // Vi bruger den lille Transform til at pege på det objekt vi vil sigte imod, så vi véd hvor våbnet skal pege hen til sidst
            //playerGunLookAt.LookAt(objectToLookAt.transform);

            playerGun.transform.rotation = Quaternion.Lerp(gunPreviousOrientation, playerGunLookAt.rotation, gunTransition);

            

            //forøg værdien vi bruger til at dreje våbnet
            gunTransition += 0.1f;
        }
        else if(gunTransition >= 1.0f)
        {
            // reset
            gunTransition = -1.0f;
            gunController.StopShooting();
        }

    }

	// Update is called once per frame
	void Update () {

        // Sæt vores score ind på score-feltet på skærmen
        scoreFelt.text = score.ToString();

        // Vi ændrer livFeltet´s sizeDelta.x (bredde) til en procentdel af hvor mange liv vi har tilbage
        livFelt.GetComponent<RectTransform>().sizeDelta = new Vector2( ((float)liv/ (float)startLiv)*startBreddeForLivBar , 50f);
        livFelt.GetComponent<Image>().color = gradientLiv.Evaluate( 1.0f - ((float)liv / (float)startLiv)) ;
    }
}
