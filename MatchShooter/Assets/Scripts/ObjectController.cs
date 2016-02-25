using UnityEngine;
using System.Collections;

/// <summary>
/// Det script som sidder på de faldende objekter, som styrer hvad de véd og hvordan de ser ud
/// </summary>
public class ObjectController : MonoBehaviour {

    // Den hastighed som objektet falder med
    public float hastighed;

    // Det objekt som falder (det objekt dette script sidder på)
    public GameObject objekt;

    // En reference til sceneControlleren
    private SceneController controller;

    // Den slags objekt dette GameObject er
    public ObjektType minType;

    // Den material som er på dette objekt, som vi ændrer farven på
    private Material mitMaterial;

    void FixedUpdate()
    {
        if (!controller.ErSpilletAktivt())
        {
            Debug.Log("Spillet er IKKE aktivt. Slet (" + this.gameObject.name + ")");
            Destroy(this.gameObject);
        }

        // Ved hver update, ryk dette objekt ned med værdien i "hastighed"
        objekt.transform.position = new Vector3(objekt.transform.position.x,
                                                objekt.transform.position.y - hastighed,
                                                objekt.transform.position.z);
    }

    /// <summary>
    /// Når dette objekt har ramt bunden, skal den fjerne liv og måske point
    /// </summary>
    void HarRamtBunden()
    {
        // Kald funktionen "MistLiv" i sceneControlleren
        controller.MistLiv();
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
        if(controller.HarTrykketPaaObjekt(minType, this.gameObject) )    // Denne funktion giver også point
        {
            controller.RemoveObject(this.gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        controller = GameObject.FindWithTag("sceneController").GetComponent<SceneController>();
        int valgtType = Random.Range(0,4) + 1;
        minType = (ObjektType)valgtType;
        mitMaterial = new Material( this.gameObject.GetComponent<Renderer>().material );

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

        this.gameObject.GetComponent<Renderer>().material = mitMaterial;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
