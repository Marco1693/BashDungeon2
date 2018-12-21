using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prodotto{

    string nome;
    float prezzo;
    //float spedizione;
    Sprite icon;
	
    public Prodotto(string nome, float prezzo, Sprite icon)
    {
        this.nome = nome;
        this.prezzo = prezzo;
        this.icon = icon;
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

    public Sprite Icon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
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
