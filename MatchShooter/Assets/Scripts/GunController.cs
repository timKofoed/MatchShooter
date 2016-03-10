using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunController : MonoBehaviour {

    public List<GameObject> guns;
    private GameObject gunActive;

    [SerializeField]
    private Transform projectileEnd;

    private bool isShooting = false;
    private static float damageDealing = 1.0f;

    public void SetDamagePotential(float damage)
    {
        GunController.damageDealing = damage;
    }

    public static void TargetHit(GameObject objectHit)
    {
        if(objectHit.tag == "TargetToHit")
        {
            //Debug.Log("Target hit: " + objectHit.name + " damage: " + damageDealing);
            objectHit.BroadcastMessage("TagSkade", damageDealing, SendMessageOptions.DontRequireReceiver);
        }
        

    }

    // Send the target position via the broadcast, so the guns themselves can decide when and how to instantiate their projectiles
    public void StartShooting()
    {
        if (gunActive != null)
            gunActive.BroadcastMessage("StartShooting", projectileEnd);
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
	
    public void SetProjectileEndPosition(Transform endPos)
    {
        if(projectileEnd != null && endPos != null)
            projectileEnd.position = endPos.position;
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
