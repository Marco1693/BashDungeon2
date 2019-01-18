using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Processo{

    int pid;
    string nome;
    string time;
    float timer;

    bool isActive = false;

    public Processo(int pid, string nome)
    {
        this.pid = pid;
        this.nome = nome;
        time ="00:00:00";
        timer = 0;
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

    public float Timer
    {
        get
        {
            return timer;
        }
        set
        {
            timer = value;
        }
    }

    public string GetTime()
    {
        return time;
    }

    public void SetTime()
    {
        string minutes,seconds,hours;
        minutes = Mathf.Floor(timer / 60).ToString("00");
        seconds = (timer % 60).ToString("00");
        hours = Mathf.Floor(timer / 3600).ToString("00");
        time = (hours + ":" + minutes + ":" + seconds);
    }
    
    public bool IsActive
    {
        get
        {
            return isActive;
        }

        set
        {
            isActive = value;
        }
    }
}
