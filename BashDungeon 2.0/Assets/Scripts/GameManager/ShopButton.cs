using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour {
    Button ipClicked;
    GameObject gameManager;
    string ip;
    Shop shop;

	// Use this for initialization
	void Start()
    {
        ipClicked = this.GetComponent<Button>();
        ipClicked.onClick.AddListener(OpenShop);
        gameManager = GameObject.Find("GameManager");
        ip = this.GetComponentInChildren<Text>().text;
        gameManager.GetComponent<PlayManager>().ipBuy = ip;
    }
	
	void OpenShop()
    {
        shop = gameManager.GetComponent<LevelGeneration>().shops.Find(x => (x.GetIp()+("\n")) == ip);
        gameManager.GetComponent<PlayManager>().GoToShop(ip);
        foreach (GameObject prodotto in gameManager.GetComponent<PlayManager>().addedProducts)
        {
            Destroy(prodotto);
        }
        gameManager.GetComponent<LevelGeneration>().SpawnShopProducts(shop);
    }
}
