using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop{
    string ipNumerico;
    string indirizzo;
    float distanza;
    float spedizione;
    public List<Prodotto> listaProdotti = new List<Prodotto>();

	public Shop()
    {
        indirizzo = ("www.") + GameObject.Find("GameManager").GetComponent<RandomNamesGenerator>().GenerateName() + (".com");
        ipNumerico = Random.Range(0, 255) + (".") + Random.Range(0, 255) + (".") + Random.Range(0, 255) + (".") + Random.Range(0, 255);
        distanza = Random.Range(50f, 200f);
        spedizione = distanza / 10;
        ProdottiInVendita();
    }

    public string getIp()
    {
        return ipNumerico;
    }

    public string getIndirizzo()
    {
        return indirizzo;
    }

    public float getDistanza()
    {
        return distanza;
    }

    public float getSpedizione()
    {
        return spedizione;
    }

    public void ProdottiInVendita()
    {
        Prodotto prodotto = new Prodotto("kill", 5f * spedizione);
        listaProdotti.Add(prodotto);

        Prodotto test = new Prodotto("test", 7.22f * spedizione);
        listaProdotti.Add(test);
    }
}
