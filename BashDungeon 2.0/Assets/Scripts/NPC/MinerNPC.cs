using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerNPC : MonoBehaviour
{
    GameObject playerGO;
    Oggetto minerNPC;
    GameObject gameManager;

    bool primoIncontro = false;
    
    void Start()
    {
        playerGO = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == "minerNPC"))
        {
            minerNPC = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == "minerNPC");
            if(!primoIncontro && gameManager.GetComponent<PlayManager>().ClickedObject == gameObject)
            {
                gameManager.GetComponent<PlayManager>().memory += 5;
                primoIncontro = true;
                gameManager.GetComponent<PlayManager>().ClickedObject = null;
            }
            if(primoIncontro && gameManager.GetComponent<PlayManager>().ClickedObject == gameObject)
            {
                minerNPC.TestoTxT = ("Per vedere i processi di estrazione disponibili puoi usare il comando \"ps\"\n") +
                   ("Per far ripartire un processo puoi usare il comando \"kill\" seguito da \"-CONT\" e il suo PID\n") +
                   ("Per mettere in pausa un processo puoi usare il comando \"kill\" seguido da \"-STOP\" e il suo PID\n");
            }
        }
    }
}
