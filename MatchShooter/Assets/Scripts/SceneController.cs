using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;   // UI elementer i Canvas

public enum ObjektType
{
    objekt1 = 1,
    objekt2 = 2,
    objekt3 = 3,
    objekt4 = 4
}

public enum WeaponType
{
    vulcan = 0,
    nr2 = 1,
    nr3 = 2,
    nr4 = 3
}

public class SceneController : MonoBehaviour {
    public GameObject fallendeObjektPrefab;
    public Transform startPosition;
    public int liv = 5;
    public int score = 0;
    public ObjektType valgteObjektType = ObjektType.objekt1;
    public HighScoreScript highscoreScript;

    public GameObject environmentRoot;

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

    public Text scoreFelt;
    public Image livFelt;

    private float startBreddeForLivBar;
    private int startLiv;

    private Coroutine objektGenerator;

    public Button pressToStart;

    public GameObject playerGun;
    public Transform playerGunLookAt;
    private GameObject objectToLookAt;  //Gem en reference til det fallende objekt som våbnet skal kigge på
    private Quaternion gunPreviousOrientation; //Husk hvor våbnet pegede hen før vi startede med at dreje det
    private float gunTransition = -1.0f; //en værdi vi bruger til at holde styr på hvor langt våbnet har drejet imod objektet (bruges af en Lerp)
    private GunController gunController;

    // Use this for initialization
    void Start () {
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
    }

    public void SetDamagePotential(float damage)
    {
        gunController.SetDamagePotential(damage);
    }

    private IEnumerator RotateEnvironment()
    {
        while(true)
        {
            environmentRoot.transform.Rotate(0.0f, 0.015f, 0.0f);
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

    public void PauseKnapTrykket()
    {
        // Hvis tiden går hurtigere end "meget langsom", så stop tiden
        if (Time.timeScale > 0.0f)
            Time.timeScale = 0.0f;
        else //...ellers sæt tiden tilbage til standarden
            Time.timeScale = 1.0f;
        
    }
	
    public void SaetValgtVaaben(WeaponType valgtVaaben)
    {
        switch (valgtVaaben)
        {
            case WeaponType.vulcan:
                F3DFXController.instance.DefaultFXType = F3DFXType.Vulcan;
                break;
            case WeaponType.nr2:
                F3DFXController.instance.DefaultFXType = F3DFXType.PlasmaGun;
                break;
            case WeaponType.nr3:
                F3DFXController.instance.DefaultFXType = F3DFXType.LightningGun;
                break;
            case WeaponType.nr4:
                F3DFXController.instance.DefaultFXType = F3DFXType.RailGun;
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

            // Lav et nyt objekt af vores prefab (og sørg for at det er tændt)
            fallingObjects.Add( (GameObject.Instantiate(fallendeObjektPrefab, startPosition.position, startPosition.rotation) as GameObject).transform);

            fallingObjects[fallingObjects.Count-1].gameObject.SetActive(true);
        }
    }

    public void RemoveObject(GameObject objectToRemove)
    {
        fallingObjects.Remove(objectToRemove.transform);

        // Slet det GameObject, fordi vi nu er færdige med det
        Destroy(objectToRemove);
    }

    public void MistLiv()
    {
        liv -= 1;

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
