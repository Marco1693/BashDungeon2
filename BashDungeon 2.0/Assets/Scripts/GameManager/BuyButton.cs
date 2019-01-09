using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour {

    Button productClicked;
    string nomeProdotto;
    string ip;
    GameObject gameManager;
    Shop shop;
    Prodotto prodotto;
    Animation alert;
    // Use this for initialization
    void Start ()
    {
        productClicked = this.GetComponent<Button>();
        nomeProdotto = this.GetComponentsInChildren<Text>()[0].text;
        productClicked.onClick.AddListener(BuyProduct);
        gameManager = GameObject.Find("GameManager");
        ip = gameManager.GetComponent<PlayManager>().ipBuy;
        shop = gameManager.GetComponent<LevelGeneration>().shops.Find(x => x.GetIp() == ip);
        prodotto = shop.listaProdotti.Find(x => x.Nome == nomeProdotto);
        alert = GameObject.Find("Money").GetComponent<Animation>();
    }
	
	void BuyProduct()
    {
        if (gameManager.GetComponent<PlayManager>().playerMoney >= (prodotto.Prezzo + shop.GetSpedizione()))
        {
            gameManager.GetComponent<PlayManager>().SubMoney(prodotto.Prezzo + shop.GetSpedizione());
            gameManager.GetComponent<PlayManager>().listaComprati.Add(prodotto);
            AttivaComando(prodotto.Nome);
            Destroy(this.gameObject);
        }
        else
        {
            if (!alert.isPlaying)
            {
                alert.Play("redAlert");
                StartCoroutine(StopAlert());
            }
        }
    }

    void AttivaComando(string comando)
    {
        switch (comando)
        {
            case "kill":
                //attiva comando kill
                break;
            default:
                //il comando non esiste
                break;
        }
    }

    IEnumerator StopAlert()
    {
        yield return new WaitForSeconds(2);
        alert.Stop("redAlert");

    }
}
