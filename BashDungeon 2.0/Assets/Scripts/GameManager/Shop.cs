using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop{
    string ipNumerico;
    string indirizzo;
    float distanza;
    float spedizione;
    public List<Prodotto> listaProdotti = new List<Prodotto>();
    bool prodottiSpawnati;

	public Shop()
    {
        indirizzo = ("www.") + GameObject.Find("GameManager").GetComponent<RandomNamesGenerator>().GenerateName() + (".com");
        ipNumerico = Random.Range(0, 255) + (".") + Random.Range(0, 255) + (".") + Random.Range(0, 255) + (".") + Random.Range(0, 255);
        distanza = Random.Range(50f, 200f);
        spedizione = distanza / 10;
        ProdottiInVendita();
    }

    public string GetIp()
    {
        return ipNumerico;
    }

    public string GetIndirizzo()
    {
        return indirizzo;
    }

    public float GetDistanza()
    {
        return distanza;
    }

    public float GetSpedizione()
    {
        return spedizione;
    }

    public bool ProdottiSpawnati
    {
        get
        {
            return prodottiSpawnati;
        }

        set
        {
            prodottiSpawnati = value;
        }
    }

    public void ProdottiInVendita()
    {
        Prodotto prodotto = new Prodotto("kill", 5f);
        listaProdotti.Add(prodotto);

        Prodotto test = new Prodotto("test", 7.22f);
        listaProdotti.Add(test);
    }
}
