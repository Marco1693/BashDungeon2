using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prodotto{

    string nome;
    float prezzo;
    //float spedizione;
	
    public Prodotto(string nome, float prezzo)
    {
        this.nome = nome;
        this.prezzo = prezzo;
    }
    public string Nome
    {
        get
        {
            return nome;
        }

        set
        {
            nome = value;
        }
    }

    public float Prezzo
    {
        get
        {
            return prezzo;
        }

        set
        {
            prezzo = value;
        }
    }

    /*public float Spedizione
    {
        get
        {
            return spedizione;
        }

        set
        {
            spedizione = value;
        }
    }*/

}
