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
        shop = gameManager.GetComponent<LevelGeneration>().shops.Find(x => (x.GetIp()+("\n")) == ip);
        prodotto = shop.listaProdotti.Find(x => x.Nome == nomeProdotto);
        alert = GameObject.Find("Money").GetComponent<Animation>();
    }
	
	void BuyProduct()
    {
        if (gameManager.GetComponent<PlayManager>().playerMoney >= (prodotto.Prezzo + shop.GetSpedizione()))
        {
            if (!prodotto.Nome.Contains("Pozione"))
            {
                gameManager.GetComponent<PlayManager>().SubMoney(prodotto.Prezzo + shop.GetSpedizione());
                gameManager.GetComponent<PlayManager>().listaComprati.Add(prodotto);
                AttivaComando(prodotto.Nome);
                Destroy(this.gameObject);
            }
            else
            {
                if(GameObject.Find("Bar").GetComponent<HealthBar>().health < 100)
                {
                    gameManager.GetComponent<PlayManager>().SubMoney(prodotto.Prezzo + shop.GetSpedizione());
                    gameManager.GetComponent<PlayManager>().listaComprati.Add(prodotto);
                    AttivaComando(prodotto.Nome);
                }
            }
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
            case "rm <Nemico>":
                //attiva comando rm per eliminare il nemico
                break;
            case "Pozione 20HP":
                GameObject.Find("Bar").GetComponent<HealthBar>().HealthPotion(20);
                break;
            case "Pozione 40HP":
                GameObject.Find("Bar").GetComponent<HealthBar>().HealthPotion(40);
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
