using UnityEngine;
using System.Collections;

public class ObjectController : MonoBehaviour {

    public float hastighed;
    public GameObject objekt;
    private SceneController controller;
    public ObjektType minType;
    private Material mitMaterial;

    void FixedUpdate()
    {
        objekt.transform.position = new Vector3(objekt.transform.position.x,
                                                objekt.transform.position.y - hastighed,
                                                objekt.transform.position.z);
    }

    void HarRamtBunden()
    {
        controller.MistLiv();
    }

    void OnTriggerEnter(Collider other)
    {
        HarRamtBunden();
        Destroy(this.gameObject);
    }

    void OnMouseDown()
    {
        if(controller.HarTrykketPaaObjekt(minType) )
        {
            Destroy(this.gameObject);
        }
        
    }

    // Use this for initialization
    void Start () {
        controller = GameObject.FindWithTag("sceneController").GetComponent<SceneController>();
        int valgtType = Random.Range(0,3);
        minType = (ObjektType)valgtType;
        mitMaterial = new Material( this.gameObject.GetComponent<Renderer>().material );

        if (minType == ObjektType.objekt1)
            mitMaterial.color = Color.blue;
        else if (minType == ObjektType.objekt2)
            mitMaterial.color = Color.red;
        else if (minType == ObjektType.objekt3)
            mitMaterial.color = Color.black;
        else
            Debug.LogError("Jeg kender ikke typen " + minType);

        this.gameObject.GetComponent<Renderer>().material = mitMaterial;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
