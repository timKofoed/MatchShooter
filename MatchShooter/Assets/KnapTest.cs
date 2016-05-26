using UnityEngine;
using System.Collections;

public class KnapTest : MonoBehaviour {

    public GameObject grøn;
    public GameObject rød;

    public void KnapTryk() {
        grøn.SetActive( grøn.activeSelf );
        rød.SetActive( rød.activeSelf );
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
