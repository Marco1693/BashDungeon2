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
    }
	
	void OpenShop()
    {
        shop = gameManager.GetComponent<LevelGeneration>().shops.Find(x => x.getIp() == ip);
        gameManager.GetComponent<PlayManager>().GoToShop(ip);
        if (!shop.ProdottiSpawnati)
        {
            gameManager.GetComponent<LevelGeneration>().SpawnShopProducts(shop);
            shop.ProdottiSpawnati = true;
        }

    }
}
