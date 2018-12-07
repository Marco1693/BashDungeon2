using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DogSitterNPC : MonoBehaviour {
GameObject playerGO;
Oggetto dogSitterNPC;
GameObject gameManager;
Room lootRoom;
bool roomLocked = false;
bool primoIncontro = false;
bool thereIsFood = false;
string questText = "I tre cani hanno bisogno di un osso nella stanza ";

    // Use this for initialization
    void Start()
    {
        playerGO = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager");

        Vector3 oggettoPosition = new Vector3();
        Oggetto bone = new Oggetto(gameManager.GetComponent<LevelGeneration>().RandomRoomNoLevelOrRoot(), "osso");
        bone.IsMovable = true;
        bone.CurrentRoom.oggetti.Add(bone);
        //bone.IsCopiable = true;
        GameObject selectedPrefab = gameManager.GetComponent<ObjectPrefabSelector>().PickObjectPrefab(Regex.Replace(bone.nomeOggetto, "[0-9]", ""));

        GameObject oggettoIstanziato = Instantiate(selectedPrefab) as GameObject;

        oggettoPosition.y = oggettoIstanziato.transform.position.y;
        oggettoPosition.x = oggettoIstanziato.transform.position.x + (bone.CurrentRoom.gridPos.x * 24);
        oggettoPosition.z = oggettoIstanziato.transform.position.z + (bone.CurrentRoom.gridPos.y * 24);

        oggettoIstanziato.transform.position = oggettoPosition;
        oggettoIstanziato.name = bone.nomeOggetto;
        oggettoIstanziato.transform.localScale *= 2;
        oggettoIstanziato.transform.parent = GameObject.Find("/" + bone.CurrentRoom.nomeStanza).transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == "dogSitterNPC") && !roomLocked)
        {
            dogSitterNPC = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == "dogSitterNPC");
            foreach (Room room in dogSitterNPC.CurrentRoom.childrenRooms)
            {
                if (room.oggetti.Find(x => x.nomeOggetto == "frammentoPergamena") != null)
                {
                    room.IsLocked = true;
                    roomLocked = true;
                    lootRoom = room;
                }
            }
        }
        else if (roomLocked && !thereIsFood)
        {
            if (dogSitterNPC.CurrentRoom.oggetti.Find(x => x.nomeOggetto.Contains("osso")) != null)
            {
                /*Destroy(ossoObj);
                dogSitterNPC.CurrentRoom.oggetti.Remove(osso);*/
                Oggetto osso = dogSitterNPC.CurrentRoom.oggetti.Find(x => x.nomeOggetto == "osso");
                osso.IsCopiable = true;
                dogSitterNPC.TestoTxT = "Beh.. ora c'è un osso\n ma non credo basti per tutti e tre...";
                thereIsFood = true;
                //gameManager.GetComponent<PlayManager>().RemoveQuest(questText);
                //lootRoom.IsLocked = false;
                //Destroy(this);
            }
        }
        else if(roomLocked && thereIsFood)
        {
            if (dogSitterNPC.CurrentRoom.oggetti.Find(x => x.nomeOggetto == "osso(Clone)1") != null)
            {
                float speed = 0.5f;
                Oggetto osso = dogSitterNPC.CurrentRoom.oggetti.Find(x => x.nomeOggetto == "osso");
                dogSitterNPC.TestoTxT = "Ben fatto! Ora puoi passare";
                lootRoom.IsLocked = false;
                gameManager.GetComponent<PlayManager>().RemoveQuest(questText);
                for (int i = 0; i < 3; i++)
                {
                    if (i > 0)
                    {
                        GameObject ossoObj = GameObject.Find("/" + dogSitterNPC.CurrentRoom.nomeStanza + "/" + osso.nomeOggetto + "(Clone)" + (i - 1));
                        GameObject dog = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + "caneAffamato" + i);
                        dog.GetComponent<Animation>().Play();
                        dog.transform.position += (ossoObj.transform.position - dog.transform.position) * Time.deltaTime * speed;
                        StartCoroutine(DogWalk(5f, dog));
                    }
                    else
                    {
                        GameObject ossoObj = GameObject.Find("/" + dogSitterNPC.CurrentRoom.nomeStanza + "/" + osso.nomeOggetto);
                        GameObject dog = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + "caneAffamato");
                        dog.GetComponent<Animation>().Play();
                        dog.transform.position += (ossoObj.transform.position - dog.transform.position) * Time.deltaTime * speed;
                        StartCoroutine(DogWalk(5f, dog));
                    }
                }
            }

        }

        if ((!primoIncontro) && (gameManager.GetComponent<PlayManager>().ClickedObject == gameObject) && (playerGO.GetComponent<PlayerMovement>().BlockedMovement))
        {
            gameManager.GetComponent<PlayManager>().AddQuest(questText + gameManager.GetComponent<PlayManager>().GetPath(dogSitterNPC.CurrentRoom));
            primoIncontro = true;
        }

    }
    IEnumerator DogWalk(float time, GameObject dog)
    {
        yield return new WaitForSeconds(time);
        dog.GetComponent<Animation>().Stop();
        Destroy(this);
    }
}

