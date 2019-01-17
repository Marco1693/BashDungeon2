using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ConsoleScript : MonoBehaviour
{

    public string messaggio;
    public Text textObj;
    public GameObject playerGO;
    public GameObject gameManager;
    List<string> oldMessages = new List<string>();
    int contatoreOldMessaggi = 0;
    GameObject consoleText;
    List<string> oggettiLs = new List<string>();
    List<string> stanzeLs = new List<string>();


    //GameObject consoleCanvas;

    int righeMax = 25;

    void Start()
    {
        consoleText = GameObject.Find("ConsoleText");
        playerGO = GameObject.Find("Player");
        //consoleCanvas = GameObject.Find("ConsoleCanvas");
        gameManager = GameObject.Find("GameManager");

        oldMessages.Insert(0, "");

        textObj = consoleText.GetComponent<Text>();
        textObj.text += "<b>Bash@Dungeon:~$ </b>";
    }

    void Update()
    {
        
        foreach (char c in Input.inputString)
        {
            if (!playerGO.GetComponent<PlayerMovement>().BlockedMovement)
            {

                if (c == '\b')
                {

                    if (messaggio.Length != 0)
                    {
                        messaggio = messaggio.Substring(0, messaggio.Length - 1);

                        if (textObj.text.Length != 0)
                        {
                            textObj.text = textObj.text.Substring(0, textObj.text.Length - 1);

                        }
                    }
                }
                else if ((c == '\n') || (c == '\r'))
                {
                    textObj.text += "\n";
                    contatoreOldMessaggi = 0;
                    SplitMessage(messaggio);
                    oldMessages.Insert(1, messaggio);
                    messaggio = "";
                    textObj.text += "<b>Bash@Dungeon:~$ </b>";

                }

                else
                {
                    textObj.text += c;
                    messaggio += c;
                }
            }

            CancellatoreDiRiga();
        }

        //Se premo freccia su voglio usare l ultimo commando inserito
        if (Input.GetKeyUp(KeyCode.UpArrow) && oldMessages.Count > contatoreOldMessaggi + 1)
        {
            textObj.text = textObj.text.Substring(0, textObj.text.Length - messaggio.Length);
            contatoreOldMessaggi += 1;
            messaggio = oldMessages[contatoreOldMessaggi];
            textObj.text += messaggio;
        }

        //torno indietro di una posizione nella lista dei commandi
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            textObj.text = textObj.text.Substring(0, textObj.text.Length - messaggio.Length);
            if (contatoreOldMessaggi > 0)
            {
                contatoreOldMessaggi -= 1;
            }
            messaggio = oldMessages[contatoreOldMessaggi];
            textObj.text += messaggio;
        }

        //tengo la lista dei comandi a max 25 messaggi
        if (oldMessages.Count > 25)
        {
            oldMessages.RemoveAt(oldMessages.Count - 1);
        }

        CancellatoreDiRiga();
    }

    void CancellatoreDiRiga()
    {
        if (textObj.cachedTextGenerator.lineCount > righeMax)
        {
            int valore;

            valore = textObj.cachedTextGenerator.lines[1].startCharIdx;
            textObj.text = textObj.text.Substring(valore, textObj.text.Length - valore);


        }
    }


    void SplitMessage(string message)
    {
        string[] splittedMessage;

        splittedMessage = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (splittedMessage.Length != 0)
        {
            ControlloMessaggio(splittedMessage);
        }

    }


    void ControlloMessaggio(string[] splittedMessage)
    {

        string comando = splittedMessage[0];

        switch (comando)
        {
            case "pwd":
                Pwd(splittedMessage);
                break;


            case "cd":
                Cd(splittedMessage);
                break;

            case "ls":
                Ls(splittedMessage);
                break;

            case "mv":
                Mv(splittedMessage);
                break;

            case "tar":
                Tar(splittedMessage);
                break;

            case "rm":
                Rm(splittedMessage);
                break;

            case "grep":
                Grep(splittedMessage);
                break;
           
            case "cp":
                Cp(splittedMessage);
                break;
            
            case "host":
                Host(splittedMessage);
                break;

            case "ping":
                Ping(splittedMessage);
                break;

            case "ps":
                Ps(splittedMessage);
                break;

            case "kill":
                Kill(splittedMessage);
                break;

            case "shutdown":
                if (gameManager.GetComponent<PlayManager>().FineGioco)
                {
                    Shutdown(splittedMessage);
                    break;
                }
                else
                    goto default;


                    default:
                textObj.text += (splittedMessage[0] + " non e' un comando riconosciuto." + "\n");
                break;
        }

    }

    bool CheckPath(string pathToCheck)
    {
        bool isPathCorrect = true;
        if (pathToCheck.ToCharArray().First() == '/')
        {


            string[] path = pathToCheck.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (path.Length >= 2)
            {
                for (int i = 0; i <= path.Length - 1; i++)
                {

                    if (i == path.Length - 1)
                    {
                        break;
                    }
                    else if (gameManager.GetComponent<LevelGeneration>().GetRoomByName(path[i]) != null)
                    {
                        if (!(gameManager.GetComponent<LevelGeneration>().GetRoomByName(path[i]).childrenRooms.Exists(x => x.nomeStanza == path[i + 1])))
                        {
                            isPathCorrect = false;
                            break;
                        }
                    }
                    else
                    {
                        isPathCorrect = false;
                        break;
                    }
                }
            }
            else if (path.Length == 1)
            {
                if (!(gameManager.GetComponent<LevelGeneration>().GetRoomByName("/").childrenRooms.Exists(x => x.nomeStanza == path[0])))
                {
                    isPathCorrect = false;

                }
            }
            else
            {
                isPathCorrect = false;
            }

        }
        else
        {
            isPathCorrect = false;
        }

        return isPathCorrect;
    }

    bool CheckOggetti(string[] splittedMessage, int positionToSkip)
    {
        bool ciSonoTutti = true;
        string[] listaOggetti = splittedMessage.Skip(positionToSkip).Take(splittedMessage.Length - positionToSkip).ToArray();

        foreach (string s in listaOggetti)
        {
            if (!playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == s))
            {
                ciSonoTutti = false;
                break;
            }
        }

        return ciSonoTutti;
    }

    void RemoveOggetti(string[] splittedMessage)
    {

        string[] listaOggetti = splittedMessage.Skip(3).Take(splittedMessage.Length - 3).ToArray();

        foreach (string s in listaOggetti)
        {

            playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == s).IsActive = false;
            GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + s).SetActive(false);

        }
    }

    List<GameObject> GetGameObjectsList(string[] splittedMessage, int positionToSkip)
    {
        string[] listaOggetti = splittedMessage.Skip(positionToSkip).Take(splittedMessage.Length - positionToSkip).ToArray();
        List<GameObject> listOfGameObj = new List<GameObject>();
        foreach (string s in listaOggetti)
        {

            listOfGameObj.Add(GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + s));

        }
        return listOfGameObj;
    }

    List<Oggetto> GetOggettiFromList(string[] splittedMessage, int positionToSkip)
    {
        string[] listaOggetti = splittedMessage.Skip(positionToSkip).Take(splittedMessage.Length - positionToSkip).ToArray();
        List<Oggetto> listOfOggetti = new List<Oggetto>();
        foreach (string s in listaOggetti)
        {

            listOfOggetti.Add(playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == s));

        }
        return listOfOggetti;
    }

    void SpawnArchivio(string nomeOggetto, Vector3 spawnAtPosition, List<GameObject> oggettiContenuti)
    {
        GameObject selectedPrefab;
        bool isNormalTar = false;
        Oggetto oggetto = new Oggetto(playerGO.GetComponent<PlayerMovement>().currentRoom, nomeOggetto);
        playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Add(oggetto);
        if ((oggettiContenuti.Count == 3 && oggettiContenuti.FindAll(x => x.transform.name.Contains("pezzoChiave")).Count == 3) ||
            oggettiContenuti[0].transform.name.Contains("Gigante"))//Gigante fix tar
        {
            selectedPrefab = gameManager.GetComponent<ObjectPrefabSelector>().PickObjectPrefab("chiave");
        }
        else
        {
            selectedPrefab = gameManager.GetComponent<ObjectPrefabSelector>().PickObjectPrefab(Regex.Replace(nomeOggetto, "[0-9]", ""));
            isNormalTar = true;
        }
            
        GameObject oggettoIstanziato = Instantiate(selectedPrefab) as GameObject;
        if(isNormalTar)
        {
            if (oggetto.nomeOggetto.EndsWith(".tar"))
            {
                oggetto.CanXF = true;
            }
            else if(oggetto.nomeOggetto.EndsWith(".tar.gz"))
            {
                oggetto.CanZXF = true;
            }
        }
        oggettoIstanziato.transform.parent = GameObject.Find("/" + oggetto.CurrentRoom.nomeStanza).transform;
        oggettoIstanziato.transform.position = spawnAtPosition;
        oggettoIstanziato.transform.name = oggetto.nomeOggetto;
        oggettoIstanziato.GetComponent<ObjectBehavior>().oggettiGOArchiviati = oggettiContenuti;

    }

    List<Vector3> GetObjPositionList(string[] splittedMessage)
    {
        string[] listaOggetti = splittedMessage.Skip(3).Take(splittedMessage.Length - 3).ToArray();
        List<Vector3> objPositionList = new List<Vector3>();
        foreach (string s in listaOggetti)
        {

            objPositionList.Add(GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + s).transform.position);

        }
        return objPositionList;

    }


    void Pwd(string[] splittedMessage)
    {
        if (splittedMessage.Length == 1)
        {
            textObj.text += playerGO.transform.parent.name + "\n";
        }

        else
        {
            textObj.text += ("pwd non prevede parametri." + "\n");
        }
    }

    void Cd(string[] splittedMessage)
    {
        if (splittedMessage.Length == 2)
        {
            if ((playerGO.GetComponent<PlayerMovement>().currentRoom.childrenRooms.Exists(x => x.nomeStanza == splittedMessage[1])))
            {
                gameManager.GetComponent<PlayManager>().ClickedObject = null;
                gameManager.GetComponent<PlayManager>().MoveBeforeChangeRoom(gameManager.GetComponent<LevelGeneration>().GetRoomByName(splittedMessage[1]));

            }
            else if ((splittedMessage[1] == "..") && (playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza != "/"))
            {
                gameManager.GetComponent<PlayManager>().ClickedObject = null;
                gameManager.GetComponent<PlayManager>().MoveBeforeChangeRoom(gameManager.GetComponent<LevelGeneration>().GetRoomByName(playerGO.GetComponent<PlayerMovement>().currentRoom.parentRoom.nomeStanza));

            }
            else if ((splittedMessage[1] == "/"))
            {
                gameManager.GetComponent<PlayManager>().ClickedObject = null;
                gameManager.GetComponent<PlayManager>().MoveBeforeChangeRoom(gameManager.GetComponent<LevelGeneration>().GetRoomByName("/"));

            }
            else if (CheckPath(splittedMessage[1]))
            {
                gameManager.GetComponent<PlayManager>().ClickedObject = null;
                string[] path = splittedMessage[1].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                gameManager.GetComponent<PlayManager>().MoveBeforeChangeRoom(gameManager.GetComponent<LevelGeneration>().GetRoomByName(path[path.Length - 1]));

            }
            else
            {
                textObj.text += (splittedMessage[1] + " non è un path corretto." + "\n");
            }
        }
        else
        {
            textObj.text += ("cd prevede un parametro." + "\n");
        }
    }

    void Ls(String[] splittedMessage)
    {
        if (splittedMessage.Length == 1)
        {

            if (playerGO.GetComponent<PlayerMovement>().currentRoom.childrenRooms != null)
            {
                foreach (Room r in playerGO.GetComponent<PlayerMovement>().currentRoom.childrenRooms)
                {
                    stanzeLs.Add("<color=#7F9FCDFF>" + r.nomeStanza + "</color>\n");
                }
                stanzeLs.Sort();

                foreach (string s in stanzeLs)
                {
                    textObj.text += (s);
                }

                stanzeLs.Clear();
            }

            if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti != null)
            {
                foreach (Oggetto o in playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti)
                {

                    if (!o.IsInvisible && o.IsActive)
                    {
                        oggettiLs.Add(o.nomeOggetto + "\n");
                    }
                }
                oggettiLs.Sort();
                
                foreach(string s in oggettiLs)
                {
                    textObj.text += (s);
                }

                oggettiLs.Clear();
            }

        }
        else if (splittedMessage.Length == 2 && splittedMessage[1] == "-a")
        {
            if (playerGO.GetComponent<PlayerMovement>().currentRoom.childrenRooms != null)
            {
                foreach (Room r in playerGO.GetComponent<PlayerMovement>().currentRoom.childrenRooms)
                {
                    stanzeLs.Add("<color=#7F9FCDFF>" + r.nomeStanza + "</color>\n");
                }
                stanzeLs.Sort();

                foreach (string s in stanzeLs)
                {
                    textObj.text += (s);
                }

                stanzeLs.Clear();
            }

            if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti != null)
            {
                foreach (Oggetto o in playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti)
                {
                    if (o.IsActive)
                    {
                        oggettiLs.Add(o.nomeOggetto + "\n");

                        if (o.IsInvisible)
                        {
                            GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + o.nomeOggetto).GetComponent<ObjectBehavior>().isMadeVisible = true;
                        }
                    }
                }

                oggettiLs.Sort();

                foreach (string s in oggettiLs)
                {
                    textObj.text += (s);
                }

                oggettiLs.Clear();
            }
        }
        else
        {
            textObj.text += ("ls non prevede parametri." + "\n");
        }
    }

    void Mv(String[] splittedMessage)
    {
        if (splittedMessage.Length == 3)
        {
            if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == splittedMessage[1]))
            {
                if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]).IsMovable
                    && splittedMessage[2].Contains("/"))
                {
                    if ((splittedMessage[2] == "/"))
                    {

                        GameObject selectedObj;
                        Vector3 oldLocalPos;
                        Oggetto selectedOggetto;

                        selectedObj = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + splittedMessage[1]);
                        oldLocalPos = selectedObj.transform.localPosition;
                        selectedOggetto = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]);
                        if(selectedObj == gameManager.GetComponent<PlayManager>().ClickedObject)
                        {
                            gameManager.GetComponent<PlayManager>().ClickedObject = null;
                        }
                        selectedOggetto.CurrentRoom = gameManager.GetComponent<LevelGeneration>().GetRoomByName(splittedMessage[2]); //cambio currentRoom in oggetto
                        playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Remove(selectedOggetto); // lo elimino dalla sua vecchia stanza
                        gameManager.GetComponent<LevelGeneration>().GetRoomByName(splittedMessage[2]).oggetti.Add(selectedOggetto); // lo aggiungo tra gli oggetti della nuova stanza

                        selectedObj.transform.parent = GameObject.Find("/" + splittedMessage[2]).transform;
                        selectedObj.transform.localPosition = oldLocalPos;
                        selectedObj.name = selectedOggetto.nomeOggetto;

                    }
                    else if (CheckPath(splittedMessage[2]))
                    {
                        string[] path = splittedMessage[2].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        GameObject selectedObj;
                        Vector3 oldLocalPos;
                        Oggetto selectedOggetto;


                        selectedObj = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + splittedMessage[1]);
                        oldLocalPos = selectedObj.transform.localPosition;
                        selectedOggetto = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]);

                        if (selectedObj == gameManager.GetComponent<PlayManager>().ClickedObject)
                        {
                            gameManager.GetComponent<PlayManager>().ClickedObject = null;
                        }

                        selectedOggetto.CurrentRoom = gameManager.GetComponent<LevelGeneration>().GetRoomByName(path[path.Length - 1]); //cambio currentRoom in oggetto
                        playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Remove(selectedOggetto); // lo elimino dalla sua vecchia stanza
                        gameManager.GetComponent<LevelGeneration>().GetRoomByName(path[path.Length - 1]).oggetti.Add(selectedOggetto); // lo aggiungo tra gli oggetti della nuova stanza

                        selectedObj.transform.parent = GameObject.Find("/" + path[path.Length - 1]).transform;
                        selectedObj.transform.localPosition = oldLocalPos;
                        selectedObj.name = selectedOggetto.nomeOggetto;
                    }
                    else
                    {
                        /*//20/11 rinomina archivio
                        if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]).IsRenamable
                           && !splittedMessage[2].Contains("/"))
                        {
                            if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[2]).IsActive)
                                textObj.text += (splittedMessage[2] + "esiste già, scegliere un altro nome");
                            else
                            {
                                GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + splittedMessage[1]).name = splittedMessage[2];
                                playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]).nomeOggetto = splittedMessage[2];
                            }
                        }
                        else
                            if (splittedMessage[2].Contains("/"))*/
                            textObj.text += (splittedMessage[2] + " non è un path corretto." + "\n");
                       /*else
                            textObj.text += (splittedMessage[1] + " non è rinominabile");*/
                    }
                }
                else
                {
                    if(splittedMessage[2].Contains("/"))
                        textObj.text += (splittedMessage[1] + " non è spostabile." + "\n");
                }

            }
            else
            {
                textObj.text += ("Non è presente nessun oggetto col nome di " + splittedMessage[1] + " in questa stanza :o\n");
            }
            //rinomina 21/11
            if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == splittedMessage[1]))
                if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]).IsRenamable
                    && !splittedMessage[2].Contains("/"))
                {
                    if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == splittedMessage[2]))
                        textObj.text += (splittedMessage[2] + " esiste già, scegliere un altro nome\n");
                    else
                    {
                        GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + splittedMessage[1]).name = splittedMessage[2];
                        playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]).nomeOggetto = splittedMessage[2];
                    }
                }
                else
                    if(!splittedMessage[2].Contains("/"))
                        textObj.text += (splittedMessage[1] + " non è rinominabile." + "\n");

        }
        else
        {
            textObj.text += ("mv prevede due parametri." + "\n");
        }
    }

    void Tar(String[] splittedMessage)
    {
        if (splittedMessage.Length >= 3)
        {
            if (splittedMessage.Length >= 4 && splittedMessage[1] == "-cf" && splittedMessage[2].EndsWith(".tar"))
            {
                if (CheckOggetti(splittedMessage, 3))
                {
                    bool canCF = true;
                    foreach (GameObject go in GetGameObjectsList(splittedMessage, 3))
                    {
                        if(!playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == go.transform.name).CanCF)
                        {
                            canCF = false;
                            
                        }
                    }
                    if (canCF)
                    {
                        gameManager.GetComponent<PlayManager>().ClickedObject = null;
                        Vector3 spawnTarPosition = gameManager.GetComponent<PlayManager>().CenterOfVectors(GetObjPositionList(splittedMessage));
                        SpawnArchivio(splittedMessage[2], spawnTarPosition, GetGameObjectsList(splittedMessage, 3));
                        RemoveOggetti(splittedMessage);
                    }
                    else
                    {
                        textObj.text += ("uno o più oggetti selezionati non possono venir archiviati" + "\n");
                    }
                }
                else
                {
                    textObj.text += ("non trovo tutti gli oggetti :c" + "\n");
                }
            }
            else if(splittedMessage.Length < 4 && splittedMessage[1] == "-cf" && splittedMessage[2].EndsWith(".tar"))
            {
                textObj.text += ("il comando tar -cf prevede almeno due parametri" + "\n");
            }
            else if (splittedMessage.Length == 3 && splittedMessage[1] == "-xf" && splittedMessage[2].EndsWith(".tar"))
            {
                if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == splittedMessage[2] && x.IsActive ))
                {
                    if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == splittedMessage[2] && x.CanXF))
                    {
                        gameManager.GetComponent<PlayManager>().ClickedObject = null;
                        playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[2]).IsActive = false;
                        GameObject oggettoSelezionato = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + splittedMessage[2]);

                        foreach (GameObject oggetto in oggettoSelezionato.GetComponent<ObjectBehavior>().oggettiGOArchiviati)
                        {
                            oggetto.SetActive(true);
                            Debug.Log(gameObject.transform.parent.name + " " + oggetto.transform.name);
                            gameManager.GetComponent<LevelGeneration>().GetRoomByName(oggettoSelezionato.transform.parent.name).oggetti.Find(x => x.nomeOggetto.Contains(Regex.Replace(oggetto.transform.name, "[0-9]", ""))).IsActive = true;
                        }
                        
                        oggettoSelezionato.SetActive(false);
                    }
                    else
                    {
                        textObj.text += ("l' oggetto non sembra un archivio..." + "\n");
                    }
                }
                else
                {
                    textObj.text += ("non trovo un archivio con quel nome :c" + "\n");
                }
            }
            else if (splittedMessage.Length != 3 && splittedMessage[1] == "-xf" && splittedMessage[2].EndsWith(".tar"))
            {
                textObj.text += ("il comando tar -xf prevede un parametro" + "\n");
            }
            else if (splittedMessage.Length >= 4 && (splittedMessage[1] == "-czf" || splittedMessage[1] == "-zcf") && splittedMessage[2].EndsWith(".tar.gz"))
            {
                if (CheckOggetti(splittedMessage, 3))
                {
                    bool canCZF = true;
                    foreach (GameObject go in GetGameObjectsList(splittedMessage, 3))
                    {
                        if (!playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == go.transform.name).CanZCF)
                        {
                            canCZF = false;
                        }
                    }
                    if (canCZF)
                    {
                        gameManager.GetComponent<PlayManager>().ClickedObject = null;
                        Vector3 spawnTarPosition = gameManager.GetComponent<PlayManager>().CenterOfVectors(GetObjPositionList(splittedMessage));
                        SpawnArchivio(splittedMessage[2], spawnTarPosition, GetGameObjectsList(splittedMessage, 3));
                        RemoveOggetti(splittedMessage);
                    }
                    else
                    {
                        textObj.text += ("uno o più oggetti selezionati non possono essere archiviati e compressi" + "\n");
                    }
                }
                else
                {
                    textObj.text += ("non trovo tutti gli oggetti :c" + "\n");
                }
            }
            else if (splittedMessage.Length < 4 && (splittedMessage[1] == "-czf" || splittedMessage[1] == "-zcf") && splittedMessage[2].EndsWith(".tar.gz"))
            {
                textObj.text += ("Il comando tar " + splittedMessage[1] + " prevede almeno due parametri" + "\n");
            }
            else if (splittedMessage.Length == 3 && (splittedMessage[1] == "-xzf" || splittedMessage[1] == "-zxf") && splittedMessage[2].EndsWith(".tar.gz"))
            {
                if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == splittedMessage[2] && x.IsActive))
                {
                    if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == splittedMessage[2] && x.CanZXF))
                    {
                        gameManager.GetComponent<PlayManager>().ClickedObject = null;
                        GameObject oggettoSelezionato = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + splittedMessage[2]);
                        playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[2]).IsActive = false;

                        foreach (GameObject oggetto in oggettoSelezionato.GetComponent<ObjectBehavior>().oggettiGOArchiviati)
                        {
                            oggetto.SetActive(true);
                            Debug.Log(Regex.Replace(oggetto.transform.name, "[0-9]", ""));
                            gameManager.GetComponent<LevelGeneration>().GetRoomByName(oggettoSelezionato.transform.parent.name).oggetti.Find(x => x.nomeOggetto == oggetto.name).IsActive = true;
                        }
                        
                        oggettoSelezionato.SetActive(false);
                    }
                    else
                    {
                        textObj.text += ("non sembra essere un archivio compresso..." + "\n");
                    }
                }
                else
                {
                    textObj.text += ("non trovo un archivio con quel nome :c" + "\n");
                }
            }
            else if (splittedMessage.Length == 3 && (splittedMessage[1] == "-xzf" || splittedMessage[1] == "-zxf") && splittedMessage[2].EndsWith(".tar.gz"))
            {
                textObj.text += ("Il comando tar " + splittedMessage[1] + " prevede un parametro" + "\n");
            }
            else if(splittedMessage[1] != "-xzf" && splittedMessage[1] != "-zxf" && splittedMessage[1] != "-zcf" && splittedMessage[1] != "-czf" && splittedMessage[1] != "-xf" && splittedMessage[1] != "-cf")
            {
                textObj.text += ("Errore inatteso. Sicuro di aver inserito tutti i dati utili al comando tar (es. -xf, -cf, -zxf o -zcf) ? " + "\n");
            }
            else if((splittedMessage[1] == "-xzf" || splittedMessage[1] == "-zxf" || splittedMessage[1] == "-zcf" || splittedMessage[1] == "-czf") && !splittedMessage[2].EndsWith(".tar.gz"))
            {
                textObj.text += ("Il nome dell' archivio compresso che si vuole creare deve terminare con \".tar.gz\"" + "\n");
            }
            else if ((splittedMessage[1] == "-xf" || splittedMessage[1] == "-cf") && !splittedMessage[2].EndsWith(".tar"))
            {
                textObj.text += ("Il nome dell' archivio che si vuole creare deve terminare con \".tar\"" + "\n");
            }
        }
        else
        {
            textObj.text += ("tar prevede almeno un parametro." + "\n");
        }
    }

    void Rm(String[] splittedMessage)
    {

        if (splittedMessage.Length == 2)
        {
            if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == splittedMessage[1]))
            {
                if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]).IsRemovable)
                {
                    GameObject selectedObj;
                    Oggetto selectedOggetto;

                    selectedObj = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + splittedMessage[1]);
                    selectedOggetto = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]);
                    if (selectedObj == gameManager.GetComponent<PlayManager>().ClickedObject)
                    {
                        gameManager.GetComponent<PlayManager>().ClickedObject = null;
                    }
                    playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Remove(selectedOggetto);
                    Destroy(selectedObj);

                }
                else
                {
                    textObj.text += ("Non posso eliminarlo !" + "\n");
                }
            }
            else
            {
                textObj.text += ("Non è presente nessun oggetto col nome di " + splittedMessage[1] + " in questa stanza :o\n");
            }
        }
        else
        {
            textObj.text += ("rm prevede un parametro." + "\n");
        }


    }

    void Grep(String[] splittedMessage)
    {
        if (splittedMessage.Length == 3)
        {
            if (splittedMessage[2] == "." || splittedMessage[2] == "*")
            {
                List<Oggetto> oggettiSelezionati = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti;

                if (splittedMessage[1].Contains("*"))
                {
                    splittedMessage[1] = splittedMessage[1].Replace("*", ".{1,}");
                }

                if (splittedMessage[1].Contains(")"))
                {
                    splittedMessage[1] = splittedMessage[1].Replace(")", "/)");
                }
                int contatoreOgg = 0;
                Oggetto oggettoTrovato = null;
                foreach (Oggetto o in oggettiSelezionati)
                {

                    if (o.IsTxt)
                    {

                        if (Regex.IsMatch(o.TestoTxT, splittedMessage[1]))
                        {
                            contatoreOgg++;
                            textObj.text += (o.nomeOggetto + ":\n" + o.TestoTxT + "\n");
                            oggettoTrovato = o;
                        }
                    }

                }
                if(contatoreOgg == 1 && oggettoTrovato != null)
                {
                    gameManager.GetComponent<PlayManager>().FoundWithGrepGO = GameObject.Find("/" + oggettoTrovato.CurrentRoom.nomeStanza + "/" + oggettoTrovato.nomeOggetto);
                }
            }
            else if (CheckOggetti(splittedMessage, 2))
            {
                Oggetto oggettoSelezionato = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[2]);

                if (oggettoSelezionato.IsTxt)
                {
                    if (splittedMessage[1].Contains("*"))
                    {
                        splittedMessage[1] = splittedMessage[1].Replace("*", ".{1,}");
                    }

                    if (Regex.IsMatch(oggettoSelezionato.TestoTxT, @splittedMessage[1]))
                    {
                        textObj.text += (oggettoSelezionato.nomeOggetto + ":\n" + oggettoSelezionato.TestoTxT + "\n");
                    }
                }
            }
            else
            {
                textObj.text += ("Uno o piu' oggetti non sono stati trovati\n");
            }
        }
    }

    void Cp(String[] splittedMessage)
    {
        //se ha 2 parametri
        if (splittedMessage.Length == 3)
        {
            //se l'oggetto da copiare esiste nella stanza in cui mi trovo
            if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == splittedMessage[1]))
            {
                if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]).IsCopiable)
                {
                    //copiare nella dir corrente
                    if (splittedMessage[2] == ".")
                    {
                        GameObject selectedObj;
                        GameObject clone;
                        Vector3 oldLocalPos;
                        Oggetto selectedOggetto;
                        Oggetto originalOggetto;

                        originalOggetto = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]);
                        selectedObj = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + splittedMessage[1]);
                        oldLocalPos = selectedObj.transform.localPosition;
                        Instantiate(selectedObj);

                        clone = GameObject.Find(splittedMessage[1] + "(Clone)");
                        clone.name += originalOggetto.numCloni;
                        clone.transform.parent = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza).transform;
                        selectedOggetto = new Oggetto(playerGO.GetComponent<PlayerMovement>().currentRoom, clone.name);

                        if (selectedObj == gameManager.GetComponent<PlayManager>().ClickedObject)
                        {
                            gameManager.GetComponent<PlayManager>().ClickedObject = null;
                        }
                        playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Add(selectedOggetto);


                        clone.transform.localPosition = oldLocalPos + new Vector3(originalOggetto.numCloni + 1, 0, 0);
                        originalOggetto.numCloni += 1;
                    }
                    //se la destinazione è la root
                    else if (splittedMessage[2] == "/")
                    {
                        GameObject selectedObj;
                        GameObject clone;
                        Vector3 oldLocalPos;
                        Oggetto selectedOggetto;
                        Oggetto originalOggetto;

                        //trova l'Oggetto da copiare
                        originalOggetto = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]);
                        //trova il gameobject che si vuole copiare
                        selectedObj = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + splittedMessage[1]);
                        oldLocalPos = selectedObj.transform.localPosition;
                        //clona il gameobject
                        Instantiate(selectedObj);
                        //trova il gameobject appena istanziato
                        clone = GameObject.Find(splittedMessage[1] + "(Clone)");
                        //aggiunta numero clone
                        clone.name += originalOggetto.numCloni;
                        //sposta il transform all'interno nella stanza specificata
                        clone.transform.parent = GameObject.Find("/" + splittedMessage[2]).transform;
                        //nuovo Oggetto
                        selectedOggetto = new Oggetto(gameManager.GetComponent<LevelGeneration>().GetRoomByName(splittedMessage[2]), clone.name);

                        if (selectedObj == gameManager.GetComponent<PlayManager>().ClickedObject)
                        {
                            gameManager.GetComponent<PlayManager>().ClickedObject = null;
                        }
                        //aggiunge il nuovo Oggetto nella room desiderata
                        gameManager.GetComponent<LevelGeneration>().GetRoomByName(splittedMessage[2]).oggetti.Add(selectedOggetto);

                        //il clone viene spostato
                        clone.transform.localPosition = oldLocalPos + new Vector3(originalOggetto.numCloni + 1, 0, 0);
                        originalOggetto.numCloni++;

                    }
                    //se la destinazione esiste
                    else if (CheckPath(splittedMessage[2]))
                    {
                        GameObject selectedObj;
                        GameObject clone;
                        Vector3 oldLocalPos;
                        Oggetto selectedOggetto;
                        string[] path = splittedMessage[2].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        Oggetto originalOggetto;

                        //trova l'Oggetto da copiare
                        originalOggetto = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]);
                        //trova il gameobject che si vuole copiare
                        selectedObj = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + splittedMessage[1]);
                        oldLocalPos = selectedObj.transform.localPosition;
                        //clona il gameobject
                        Instantiate(selectedObj);
                        //trova il gameobject appena istanziato
                        clone = GameObject.Find(splittedMessage[1] + "(Clone)");
                        //aggiunta numero clone
                        clone.name += originalOggetto.numCloni;
                        //sposta il transform all'interno nella stanza specificata
                        clone.transform.parent = GameObject.Find("/" + path[path.Length - 1]).transform;
                        //nuovo Oggetto
                        selectedOggetto = new Oggetto(gameManager.GetComponent<LevelGeneration>().GetRoomByName(path[path.Length - 1]), clone.name);

                        if (selectedObj == gameManager.GetComponent<PlayManager>().ClickedObject)
                        {
                            gameManager.GetComponent<PlayManager>().ClickedObject = null;
                        }
                        //aggiunge il nuovo Oggetto nella room desiderata
                        gameManager.GetComponent<LevelGeneration>().GetRoomByName(path[path.Length - 1]).oggetti.Add(selectedOggetto);

                        //il clone viene spostato
                        clone.transform.localPosition = oldLocalPos + new Vector3(originalOggetto.numCloni + 1, 0, 0);
                        originalOggetto.numCloni++;
                    }
                    //il path non esiste
                    else
                    {
                        textObj.text += (" \"") + (splittedMessage[2] + "\" non è un path corretto\n");
                    }
                }
                //non è possibile copiare l'oggetto
                else
                {
                    textObj.text += ("Non è possibile copiare l'oggetto\n");
                }
            }
            //l'oggetto non esiste
            else
            {
                textObj.text += ("Non è presente alcun oggetto con il nome di \"" + splittedMessage[1] + "\" in questa stanza\n");
            }
        }
        else
        {
            //ha bisogno di 2 parametri
            textObj.text += ("cp prevede due parametri\n");
        }
    }

    void Host(String[] splittedMessage)
    {
        Shop negozio;
        if (splittedMessage.Length == 2)
        {
            //devo controllare se l'indirizzo esiste e farmi restituire l'ip numerico
            if (gameManager.GetComponent<LevelGeneration>().shops.Exists(x => x.GetIndirizzo() == splittedMessage[1]))
            {
                negozio = gameManager.GetComponent<LevelGeneration>().shops.Find(x => x.GetIndirizzo() == splittedMessage[1]);
                textObj.text += splittedMessage[1] + (" has address ") + (negozio.GetIp()) + ("\n");
                //aggiunge l'ip alla lista degli shop nell'UI se non era già presente
                if (!(gameManager.GetComponent<PlayManager>().addedShops.Exists(x => x.GetComponentInChildren<Text>().text == negozio.GetIp())))
                {
                    gameManager.GetComponent<PlayManager>().AddShop(negozio.GetIp());
                }
            }
            else
            {
                textObj.text += ("l'indirizzo ") + splittedMessage[1] + (" non esiste\n");
            }

        }
        else
        {
            textObj.text += ("il comando host prevede un parametro\n");
        }
    }

    void Ping(String[] splittedMessage)
    {
        Shop negozio;
        if(splittedMessage.Length == 2)
        {
            //restituisci la distanza
            if(gameManager.GetComponent<LevelGeneration>().shops.Exists(x => x.GetIndirizzo() == splittedMessage[1]))
            {
                negozio = gameManager.GetComponent<LevelGeneration>().shops.Find(x => x.GetIndirizzo() == splittedMessage[1]);
                textObj.text += splittedMessage[1] + (" time= ") + Math.Round(negozio.GetDistanza(), 2) + ("ms\n");
            }
            else if (gameManager.GetComponent<LevelGeneration>().shops.Exists(x => x.GetIp() == splittedMessage[1]))
            {
                negozio = gameManager.GetComponent<LevelGeneration>().shops.Find(x => x.GetIp() == splittedMessage[1]);
                textObj.text += splittedMessage[1] + (" time= ") + Math.Round(negozio.GetDistanza(), 2) + ("ms\n");
            }
            else
            {
                textObj.text += ("l'indirizzo inserito non esiste \n");
            }
        }
        else
        {
            textObj.text += ("il comando ping prevede un parametro\n");
        }
    }

    void Ps(String[] splittedMessage)
    {
        if(splittedMessage.Length == 1)
        {
            //mostra processi
            textObj.text += "PID\t\tTime\t\tCMD\n";
            foreach (Processo processo in gameManager.GetComponent<LevelGeneration>().processi)
            {
                //stampa dei processi 
                textObj.text += processo.Pid + ("\t\t") + processo.GetTime() + ("\t") + processo.Nome +("\n");
            }
        }
        else
        {
            textObj.text += ("Il comando ps non prevede altri parametri");
        }
    }

    void Kill(String[] splittedMessage)
    {
        if(splittedMessage.Length == 3)
        {
            if (splittedMessage[1] == "-STOP")
            {
                //Stop process kill -STOP <PID>
            }
            if (splittedMessage[1] == "-CONT")
            {
                //Resume process kill -CONT <PID>
            }
        }
        else
        {
            textObj.text += ("Il comando kill prevede 2 parametri");
        }
    }

    void Shutdown(String[] splittedMessage)
    {
        if (splittedMessage.Length == 1)
        {
            SceneManager.LoadScene("Credits");
        }
    }
}
