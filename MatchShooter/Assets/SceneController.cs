using UnityEngine;
using System.Collections;

public enum ObjektType
{
    objekt1,
    objekt2,
    objekt3
}

public class SceneController : MonoBehaviour {
    public GameObject fallendeObjektPrefab;
    public Transform startPosition;
    public int liv = 5;
    public int score = 0;
    public ObjektType valgteObjektType;
	// Use this for initialization
	void Start () {
        StartCoroutine( LavObjektInterval() );
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
	
	}
}
