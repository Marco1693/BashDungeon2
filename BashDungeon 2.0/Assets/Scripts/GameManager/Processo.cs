using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Processo{

    int pid;
    string nome;
    string time;

	void Start () {
		
	}
	
	void Update () {
		
	}

    public Processo(int pid, string nome)
    {
        this.pid = pid;
        this.nome = nome;
        time ="00:00:00";
    }

    public int Pid
    {
        get
        {
            return pid;
        }
        set
        {
            pid = value;
        }
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

    public string GetTime()
    {
        return time;
    }

    public void SetTime(string ore, string minuti , string secondi)
    {
        time = ore + (":") + minuti + (":") + secondi;
    }

}
