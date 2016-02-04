using UnityEngine;
using System.Collections;
using UnityEngine.UI;   // UI elementer i Canvas

public enum ObjektType
{
    objekt1 = 1,
    objekt2 = 2,
    objekt3 = 3,
    objekt4 = 4
}

public class SceneController : MonoBehaviour {
    public GameObject fallendeObjektPrefab;
    public Transform startPosition;
    public int liv = 5;
    public int score = 0;
    public ObjektType valgteObjektType = ObjektType.objekt1;

    public Button knap1;
    public Button knap2;
    public Button knap3;
    public Button knap4;

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

    // Use this for initialization
    void Start () {
        RestartLevel();
        Debug.Log("Time.fixedDeltaTime: " + Time.fixedDeltaTime);
    }

    public bool ErSpilletAktivt()
    {
        if (pressToStart.gameObject.activeSelf)
            return false;
        else
            return true;
    }

    public void PauseKnapTrykket()
    {
        // Hvis tiden går hurtigere end "meget langsom", så stop tiden
        if (Time.timeScale > 0.001f)
            Time.timeScale = 0.001f;
        else //...ellers sæt tiden tilbage til standarden
            Time.timeScale = 1.0f;
        
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
            newColour1.normalColor = Color.blue;
            knap1.colors = newColour1;
            knap1.GetComponent<Image>().color = Color.blue;
            objekt1Preview.GetComponent<Image>().color = Color.blue;

            // Gør de andre knapper hvide
            ColorBlock newColour2 = knap2.colors;
            newColour2.normalColor = Color.white;
            knap2.colors = newColour2;
            knap2.GetComponent<Image>().color = Color.white;

            ColorBlock newColour3 = knap3.colors;
            newColour3.normalColor = Color.white;
            knap3.colors = newColour3;
            knap3.GetComponent<Image>().color = Color.white;

            ColorBlock newColour4 = knap4.colors;
            newColour4.normalColor = Color.white;
            knap4.colors = newColour4;
            knap4.GetComponent<Image>().color = Color.white;
        }
        else if (valgteObjektType == ObjektType.objekt2)
        {
            // Vi har trykket på Objekt2 i siden af skærmen

            ColorBlock newColour1 = knap1.colors;
            newColour1.normalColor = Color.white;
            knap1.colors = newColour1;
            knap1.GetComponent<Image>().color = Color.white;

            // Tænd for knap2 farven
            ColorBlock newColour2 = knap2.colors;
            newColour2.normalColor = Color.red;
            knap2.colors = newColour2;
            knap2.GetComponent<Image>().color = Color.red;
            objekt1Preview.GetComponent<Image>().color = Color.red;

            ColorBlock newColour3 = knap3.colors;
            newColour3.normalColor = Color.white;
            knap3.colors = newColour3;
            knap3.GetComponent<Image>().color = Color.white;

            ColorBlock newColour4 = knap4.colors;
            newColour4.normalColor = Color.white;
            knap4.colors = newColour4;
            knap4.GetComponent<Image>().color = Color.white;
        }
        else if (valgteObjektType == ObjektType.objekt3)
        {
            // Vi har trykket på Objekt3 i siden af skærmen

            ColorBlock newColour1 = knap1.colors;
            newColour1.normalColor = Color.white;
            knap1.colors = newColour1;
            knap1.GetComponent<Image>().color = Color.white;

            ColorBlock newColour2 = knap2.colors;
            newColour2.normalColor = Color.white;
            knap2.colors = newColour2;
            knap2.GetComponent<Image>().color = Color.white;

            // Tænd for knap3 farven
            ColorBlock newColour3 = knap3.colors;
            newColour3.normalColor = Color.black;
            knap3.colors = newColour3;
            knap3.GetComponent<Image>().color = Color.black;
            objekt1Preview.GetComponent<Image>().color = Color.black;

            ColorBlock newColour4 = knap4.colors;
            newColour4.normalColor = Color.white;
            knap4.colors = newColour4;
            knap4.GetComponent<Image>().color = Color.white;
        }
        else if (valgteObjektType == ObjektType.objekt4)
        {
            // Vi har trykket på Objekt4 i siden af skærmen

            ColorBlock newColour1 = knap1.colors;
            newColour1.normalColor = Color.white;
            knap1.colors = newColour1;
            knap1.GetComponent<Image>().color = Color.white;

            ColorBlock newColour2 = knap2.colors;
            newColour2.normalColor = Color.white;
            knap2.colors = newColour2;
            knap2.GetComponent<Image>().color = Color.white;

            ColorBlock newColour3 = knap3.colors;
            newColour3.normalColor = Color.white;
            knap3.colors = newColour3;
            knap3.GetComponent<Image>().color = Color.white;

            // Tænd for knap4 farven
            ColorBlock newColour4 = knap4.colors;
            newColour4.normalColor = Color.yellow;
            knap4.colors = newColour4;
            knap4.GetComponent<Image>().color = Color.yellow;
            objekt1Preview.GetComponent<Image>().color = Color.yellow;
        }
    }

    IEnumerator LavObjektInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds( Random.Range(1.0f, 2.5f) );
            GameObject.Instantiate(fallendeObjektPrefab, startPosition.position, startPosition.rotation);
        }
        
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

        // Så tryk på knap for at starte spillet igen

        // DEBUG: reset med det samme
        //RestartLevel();
        pressToStart.gameObject.SetActive(true);
    }

    public void RestartLevel()
    {
        // Hvis objekt generatoren allerede kører, så stop den
        if (objektGenerator != null)
            StopCoroutine( objektGenerator );

        pressToStart.gameObject.SetActive(false);

        // Begynd at lave objekter i et interval
        objektGenerator = StartCoroutine(LavObjektInterval());

        // Husk hvor bred liv-baren var da vi startede
        if (startBreddeForLivBar == 0.0f)
            startBreddeForLivBar = livFelt.GetComponent<RectTransform>().sizeDelta.x;
        else
            livFelt.GetComponent<RectTransform>().sizeDelta = 
                new Vector2( startBreddeForLivBar, livFelt.GetComponent<RectTransform>().sizeDelta.y);

        // Husk hvor mange liv vi havde da vi startede
        if (startLiv == 0)
            startLiv = liv;
        else
            liv = startLiv;
    }

    public bool HarTrykketPaaObjekt(ObjektType objektViTrykkedePaa)
    {
        if (objektViTrykkedePaa == valgteObjektType)
        {
            score += 5;
            return true;
        }
        else
        {
            return false;
        }
        
    }

	// Update is called once per frame
	void Update () {
        // Sæt vores score ind på score-feltet på skærmen
        scoreFelt.text = score.ToString();

        // Vi ændrer livFeltet´s sizeDelta.x (bredde) til en procentdel af hvor mange liv vi har tilbage
        livFelt.GetComponent<RectTransform>().sizeDelta = new Vector2( ((float)liv/ (float)startLiv)*startBreddeForLivBar , 50f);
    }
}
