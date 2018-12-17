using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {
    string ipNumerico;
    string indirizzo;
    float distanza;

	public Shop()
    {
        indirizzo = ("www.") + GameObject.Find("GameManager").GetComponent<RandomNamesGenerator>().GenerateName() + (".com");
        ipNumerico = Random.Range(0, 255) + (".") + Random.Range(0, 255) + (".") + Random.Range(0, 255) + (".") + Random.Range(0, 255);
        distanza = Random.Range(1f, 500f);
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
}
