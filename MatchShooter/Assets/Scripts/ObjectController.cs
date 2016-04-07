using UnityEngine;
using System.Collections;

/// <summary>
/// Det script som sidder på de faldende objekter, som styrer hvad de véd og hvordan de ser ud
/// </summary>
public class ObjectController : MonoBehaviour {

    // Den hastighed som objektet falder med
    public float hastighed;
    public bool shouldRotate = true;
    public bool shouldScale = true;

    [SerializeField]
    private Vector3 rotation;

    // Det objekt som falder (det objekt dette script sidder på)
    public GameObject objekt;

    // En reference til sceneControlleren
    private SceneController controller;

    // Den slags objekt dette GameObject er
    public ObjektType minType;

    // Den material som er på dette objekt, som vi ændrer farven på
    private Material mitMaterial;

    // Hvor dette objekt skal bevæge sig hen imod
    private Transform slutTransform;

    // husk hvor vi startede, så vi kan lave en Lerp (Linear Interpolation) mod målet
    private Vector3 startPosition;

    // Hvor langt har vi bevæget os mod målet [0.0 .. 1.0]
    private float lerpProgress = 0.0f;

    // Hvor meget liv dette objekt har
    [SerializeField]
    private float health = 10.0f;

    // Hvor meget skade dette objekt giver spilleren, hvis man ikke får det skudt ned
    [SerializeField]
    private int damagePotential = 1;

    // Hvor mange sekunder det tager dette objekt at fade-in, i stedet for bare at blinke ind når det bliver lavet
    [SerializeField]
    private float fadeInTime = 1.0f;
    private float fadeInStartTime = 0.0f;
    private bool isFadingIn = true;

    void FixedUpdate()
    {
        if (!controller.ErSpilletAktivt())
        {
            Debug.Log("Spillet er IKKE aktivt. Slet (" + this.gameObject.name + ")");
            Destroy(this.gameObject);
        }

        // Hvis vi har et mål, så er det det vi går mod. Ellers bevæg objektet nedad
        if(slutTransform)
        {
            lerpProgress += 0.01f * hastighed;
            this.transform.position = Vector3.Lerp(startPosition, slutTransform.position, lerpProgress);
        }
        else
        {
            // Ved hver update, ryk dette objekt ned med værdien i "hastighed"
            objekt.transform.position = new Vector3(objekt.transform.position.x,
                                                    objekt.transform.position.y - hastighed,
                                                    objekt.transform.position.z);
        }

        if(shouldRotate)
            this.transform.Rotate(rotation);

        if((Time.realtimeSinceStartup < fadeInStartTime + fadeInTime) && isFadingIn)
        {
            // fade the object in
            Color modifiedColor = mitMaterial.color;
            modifiedColor.a = (Time.realtimeSinceStartup - fadeInStartTime) / fadeInTime;
            mitMaterial.color = modifiedColor;
        }
        else
        {
            isFadingIn = false; // stop fading in, and set the transparency to fully visible
            Color modifiedColor = mitMaterial.color;
            modifiedColor.a = 1.0f;
            mitMaterial.color = modifiedColor;
        }
        

    }

    /// <summary>
    /// Sæt de ting som dette objekt skal vide, såsom hvor det skal bevæge sig imod.
    /// </summary>
    /// <param name="slutPos"></param>
    public void Init(Transform slutPos)
    {
        slutTransform = slutPos;
        hastighed *= Mathf.Clamp(Random.value, 0.02f, 1.0f);

        rotation = new Vector3( 0.8f * Random.value, 0.8f * Random.value, 0.8f * Random.value);
        float newScale = Random.value * 1.5f;

        if(shouldScale)
        {
            newScale = Mathf.Clamp(newScale, 0.5f, 2.0f);
            this.transform.localScale = new Vector3(newScale, newScale, newScale);
        }
        
    }

    public void TagSkade(float skade)
    {
        health -= skade;

        if(health <= 0.0f)
        {
            controller.RemoveObject(this.gameObject);
        }
    }

    /// <summary>
    /// Når dette objekt har ramt bunden, skal den fjerne liv og måske point
    /// </summary>
    void HarRamtBunden()
    {
        // Kald funktionen "MistLiv" i sceneControlleren
        controller.MistLiv(damagePotential);
    }

    /// <summary>
    /// Dette objekt har ramt en Collider, som er sat til "Trigger"
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bund")
        {
            // Kald funktionen HarRamtBunden, som gør det der skal gøres nu
            HarRamtBunden();

            // Fjern dette objekt fra spillet
            controller.RemoveObject(this.gameObject);
           
        }
    }

    /// <summary>
    /// Musen har trykket på objektet
    /// </summary>
    void OnMouseDown()
    {
        // Hvis sceneControlleren´s valgteObjekt er det samme som minType, så slet dette objekt
        /*if(controller.HarTrykketPaaObjekt(minType, this.gameObject) )    // Denne funktion giver også point
        {
            controller.RemoveObject(this.gameObject);
        }*/
    }

    // Use this for initialization
    void Start () {
        controller = GameObject.FindWithTag("sceneController").GetComponent<SceneController>();
        int valgtType = Random.Range(0,4) + 1;
        minType = (ObjektType)valgtType;
        mitMaterial = new Material( this.gameObject.GetComponentInChildren<Renderer>().material );

        if (minType == ObjektType.objekt1)
            mitMaterial.color = controller.knap1Tryk;
        else if (minType == ObjektType.objekt2)
            mitMaterial.color = controller.knap2Tryk;
        else if (minType == ObjektType.objekt3)
            mitMaterial.color = controller.knap3Tryk;
        else if (minType == ObjektType.objekt4)
            mitMaterial.color = controller.knap4Tryk;
        else
            Debug.LogError("Jeg kender ikke typen " + minType);

        // også sæt objectet til at udstråle den valgte farve
        Color emissiveColor = new Color(mitMaterial.color.r / 2.0f, mitMaterial.color.g / 2.0f, mitMaterial.color.b / 2.0f);
        mitMaterial.SetColor("_EmissionColor", emissiveColor);

        this.gameObject.GetComponentInChildren<Renderer>().material = mitMaterial;

        // Husk hvor vi startede, så vi kan bevæge objektet mod målet
        startPosition = this.transform.position;
        lerpProgress = 0.0f;
        fadeInStartTime = Time.realtimeSinceStartup;
        Color modifiedColor = mitMaterial.color;
        modifiedColor.a = 0.0f;
        mitMaterial.color = modifiedColor;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
