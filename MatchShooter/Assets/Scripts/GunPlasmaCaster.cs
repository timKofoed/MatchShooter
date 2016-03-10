using UnityEngine;
using System.Collections;

public class GunPlasmaCaster : MonoBehaviour {

    // Det objekt som faktisk er våben-modellen, så vi kan rotere den når den skyder
    public GameObject gunGraphic;

    public GameObject muzzleFlashPrefab;
    public GameObject muzzleFlashPlacement;
    private ParticleSystem muzzleFlash;

    public GameObject fuelHolderRight;
    public GameObject fuelHolderLeft;
    private float fuelScaleStart;
    private float fuelRemaining = 1.0f;

    // Det projektil som vi vil lave og sende mod fjenden
    [SerializeField]
    private GameObject projectilePrefab;
    private GameObject projectile;

    [SerializeField]
    private Transform projectileStart;
    private Transform projectileEnd;
    private float projectileLerpProgress = -1.0f;   // -1: bruges ikke. 0: start sekvensen. [0..1]: sekvensen er igang

    private bool isShooting = false;

    private Vector3 localScaleTemp;

    void FixedUpdate()
    {
        if (isShooting)
        {
            fuelRemaining -= 0.005f;
            fuelRemaining = (fuelRemaining < 0.0f)?0.0f:fuelRemaining;

            localScaleTemp.x = fuelScaleStart;
            localScaleTemp.y = fuelScaleStart;
            localScaleTemp.z = fuelScaleStart * fuelRemaining;

            if (fuelHolderLeft != null)
                fuelHolderLeft.transform.localScale = localScaleTemp;

            if (fuelHolderRight != null)
                fuelHolderRight.transform.localScale = localScaleTemp;
        }
        //    gunGraphic.transform.Rotate(Vector3.up, 10.0f); //Rotér rundt om Y-aksen

        if(projectile != null && projectileLerpProgress >= 0)
        {
            // vi er færdige, så processen skal nu stoppes, og projektilet skal fjernes (og evt lave en eksplosion)
            if(projectileLerpProgress >= 1.0f)
            {
                projectileLerpProgress = -1.0f;
                Debug.Log("DESTROY the particle");
                Destroy(projectile);
            }
            else
            {
                projectileLerpProgress += Time.deltaTime * 0.01f;
                projectileLerpProgress = Mathf.Clamp(projectileLerpProgress, 0.0f, 1.0f);   // sørg for at projektilet ikke flyver igennem fjenden
                projectile.transform.position = Vector3.Lerp(projectileStart.position, projectileEnd.position, projectileLerpProgress); // flyt projektilet
            }
           
        }
    }

    public void StartShooting(Transform targetPos = null)
    {
        isShooting = true;

        if (targetPos != null)
        {
           /* Debug.Log("Target: " + targetPos.name);

            GameObject projectile = GameObject.Instantiate(projectilePrefab, projectileStart.position, projectileStart.rotation) as GameObject;
            projectile.transform.SetParent(projectileStart.parent);
            projectile.transform.localPosition = Vector3.zero;
            projectile.transform.localScale = Vector3.one;
            projectileEnd = targetPos;      // remember where the projectile is supposed to hit the target
            projectileLerpProgress = 0.0f;  // reset the lerp progress

            EditorApplication.isPaused = true;*/
        }
            



        //GameObject muzzleFlashObject = (GameObject.Instantiate(muzzleFlashPrefab, muzzleFlashPlacement.transform.position, muzzleFlashPrefab.transform.rotation) as GameObject);
        //muzzleFlashObject.transform.parent = muzzleFlashPlacement.transform;
        //muzzleFlash = muzzleFlashObject.GetComponent<ParticleSystem>();

        //if (muzzleFlash != null)
        //    muzzleFlash.GetComponent<ParticleSystem>().Play();
    }

    public void StopShooting()
    {
        isShooting = false;
        //if (muzzleFlash != null)
        //    muzzleFlash.GetComponent<ParticleSystem>().Stop();
    }

    // Use this for initialization
    void Start()
    {
        if (fuelHolderLeft != null)
            fuelScaleStart = fuelHolderLeft.transform.localScale.z;

        if (fuelHolderRight != null)
            fuelScaleStart = fuelHolderRight.transform.localScale.z;

        if (muzzleFlash != null)
            muzzleFlash.GetComponent<ParticleSystem>().Stop();
    }
}
