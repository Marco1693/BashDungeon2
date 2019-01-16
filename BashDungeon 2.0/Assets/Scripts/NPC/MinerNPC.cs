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
                minerNPC.TestoTxT = ("Per far partire un processo usa il comando \"start\" seguito dal nome del cristallo da cui vuoi estrarre\n") +
                   ("Per visualizzare lo stato dei processi usa il comando \".......\"");
            }
        }
    }
}
