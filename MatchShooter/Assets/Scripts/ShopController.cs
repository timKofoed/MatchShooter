using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShopController : MonoBehaviour {

	private SceneController controller;
	[SerializeField]
	private List<int> itemPrices;
	private bool[] itemCanBeBought;
	[SerializeField]
	private List<GameObject> purchaseIcons;
	[SerializeField]
	private List<Text> gunPriceTexts;


	public void PurchaseAttempt(int itemToBuy)
	{
		ShopItem itemToBuyAttempt = (ShopItem)itemToBuy;

		if ((WeaponType)itemToBuyAttempt != WeaponType.none && itemToBuy <= (int)WeaponType.flamer) 
		{
			if (controller.PurchaseGun ((WeaponType)itemToBuy, itemPrices [(itemToBuy)])) {
				//Debug.Log ("Purchased " + (WeaponType)gunToBuy);	
				purchaseIcons [itemToBuy].GetComponent<Image> ().color = new Color (0.259f, 0.259f, 0.259f, 0.3f);
				purchaseIcons [itemToBuy].GetComponent<Button> ().interactable = false;
			}
			//else
			//	Debug.Log ("Did NOT Purchase " + gunToBuy);
		} 
		else 
		{
			// Køb ekstra ting
			if(itemToBuyAttempt == ShopItem.life)
			{
				if (controller.PurchaseItem (itemToBuyAttempt, itemPrices [itemToBuy])) 
				{
					//Debug.Log ("JA");
					//purchaseIcons [(int)itemToBuyAttempt].GetComponent<Image> ().color = new Color (0.259f, 0.259f, 0.259f, 0.3f);
					//purchaseIcons [(int)itemToBuyAttempt].GetComponent<Button> ().interactable = false;
				} 
				else 
				{
					//Debug.Log ("NEJ");
				}

			}
		}
	}

	private IEnumerator 

	// Use this for initialization
	void Start () {
		controller = GameObject.FindWithTag("sceneController").GetComponent<SceneController>();
		itemCanBeBought = new bool[5] {true, false, false, false, false};

		for (int i = 0; i < itemPrices.Count; i++) 
		{
			gunPriceTexts [i].text = itemPrices [i].ToString();
			if (itemCanBeBought.Length > i)
			{
				if (itemCanBeBought [i]) 
				{
					purchaseIcons [i].GetComponent<Image> ().color = new Color (0.259f, 0.259f, 0.259f, 0.3f);
					purchaseIcons [i].GetComponent<Button> ().interactable = false;
				} 
				else 
				{
					purchaseIcons [i].GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0.3f);
					purchaseIcons [i].GetComponent<Button> ().interactable = true;
				}
			}

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
