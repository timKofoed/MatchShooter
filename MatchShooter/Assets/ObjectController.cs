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
        // Kald funktionen HarRamtBunden, som gør det der skal gøres nu
        HarRamtBunden();

        // Slet dette GameObject, fordi vi nu er færdige med det
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Musen har trykket på objektet
    /// </summary>
    void OnMouseDown()
    {
        // Hvis sceneControlleren´s valgteObjekt er det samme som minType, så slet dette objekt
        if(controller.HarTrykketPaaObjekt(minType) )    // Denne funktion giver også point
        {
            Destroy(this.gameObject);
        }
        
    }

    // Use this for initialization
    void Start () {
        controller = GameObject.FindWithTag("sceneController").GetComponent<SceneController>();
        int valgtType = Random.Range(0,4) + 1;
        minType = (ObjektType)valgtType;
        mitMaterial = new Material( this.gameObject.GetComponent<Renderer>().material );

        if (minType == ObjektType.objekt1)
            mitMaterial.color = Color.blue;
        else if (minType == ObjektType.objekt2)
            mitMaterial.color = Color.red;
        else if (minType == ObjektType.objekt3)
            mitMaterial.color = Color.black;
        else if (minType == ObjektType.objekt4)
            mitMaterial.color = Color.yellow;
        else
            Debug.LogError("Jeg kender ikke typen " + minType);

        this.gameObject.GetComponent<Renderer>().material = mitMaterial;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
