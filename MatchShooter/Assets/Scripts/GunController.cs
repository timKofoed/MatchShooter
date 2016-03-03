using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunController : MonoBehaviour {

    public List<GameObject> guns;
    private GameObject gunActive;

    private bool isShooting = false;

    public void StartShooting()
    {
        if (gunActive != null)
            gunActive.BroadcastMessage("StartShooting");
    }

    public void StopShooting()
    {
        if (gunActive != null)
            gunActive.BroadcastMessage("StopShooting");
    }

	// Use this for initialization
	void Start ()
    {
        SelectGun(1);
    }
	
    public void SelectGun(int gunIndex = 0)
    {
        // Hvis vi har en liste af våben, og der er mindst ét våben, så kan vi kigge i listen
        if (guns != null && guns.Count > 0)
        {
            // Hvis det er det valgte våben, så tænd for det, ellers sluk for det
            for (int i = 0; i < guns.Count; i++)
            {
                if(i == gunIndex)
                {
                    gunActive = guns[i];
                    if (guns[i] != null)
                        guns[i].SetActive(true);
                }
                else
                {
                    if (guns[i] != null)
                        guns[i].SetActive(false);
                }
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
