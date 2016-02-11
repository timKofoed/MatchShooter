using UnityEngine;
using System.Collections;

public class ButtonChoose : MonoBehaviour {
    public int minVaerdi;
    public SceneController sceneController;
	
    /// <summary>
    /// Når vi har trykket på denne knap, sender vi minVærdi til sceneController
    /// </summary>
	public void TrykketPaaKnap()
    {
        // Vi ændrer minVærdi fra at være en int til ObjektType, og sender
        // til sceneController
        sceneController.SaetValgtObjekt( minVaerdi );

    }
}
