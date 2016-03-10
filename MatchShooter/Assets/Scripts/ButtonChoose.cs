using UnityEngine;
using System.Collections;

public class ButtonChoose : MonoBehaviour {
    public int minVaerdi;
    public SceneController sceneController;

    public WeaponType weaponType;

    [SerializeField]
    private float weaponDamage = 1.0f;

    /// <summary>
    /// Når vi har trykket på denne knap, sender vi minVærdi til sceneController
    /// </summary>
	public void TrykketPaaKnap()
    {
        // Vi ændrer minVærdi fra at være en int til ObjektType, og sender
        // til sceneController
        //sceneController.SaetValgtObjekt( minVaerdi );
        sceneController.SetDamagePotential(weaponDamage);
        sceneController.SaetValgtVaaben( weaponType );
    }
}
