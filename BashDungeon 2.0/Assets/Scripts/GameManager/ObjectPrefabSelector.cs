using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPrefabSelector : MonoBehaviour {

    public GameObject cassa;
    public GameObject archivio;
    public GameObject pergamena;
    public GameObject npc;
    public GameObject tappeto;
    public GameObject barile;
    public GameObject tavolino;
    public GameObject libreria;
    public GameObject pozione;
    public GameObject chiave;
    public GameObject pezzoChiave0;
    public GameObject pezzoChiave1;
    public GameObject pezzoChiave2;
    public GameObject cuccioloNascosto;
    public GameObject osso;
    public GameObject crateShop;
    public GameObject moonStone;
    public GameObject blueCrystal;
    public GameObject purpleCrystal;

    public GameObject pergamenaFoundUI;
    public GameObject setQuest;
    public GameObject shopFoundUI;
    public GameObject product;

    public GameObject defaultLevel;

    public Sprite axe;
    public Sprite potion;
    public Sprite bigPotion;

    public List<GameObject> lootPrefab;

    public List<GameObject> standardRoomPrefab;

    public List<GameObject> level1Prefab;
    public List<GameObject> level2Prefab;
    public List<GameObject> level3Prefab;
    public List<GameObject> level4Prefab;
    public List<GameObject> level5Prefab;

    bool isSpawnedProcessi = false;


    private bool isTrapLevelBeingSpawn = false; //We have 2 trap-level in both level 2 and 3 Prefabs. I want to spawn it only one time.

    public GameObject PergamenaFoundUI
    {
        get
        {
            return pergamenaFoundUI;
        }
    }

    public GameObject SetQuest
    {
        get
        {
            return setQuest;
        }
    }

    public GameObject ShopFoundUI
    {
        get
        {
            return shopFoundUI;
        }
    }

    public GameObject PickObjectPrefab(string nomeOggetto)
    {
        if (nomeOggetto.Contains("cuccioloNascosto"))
        {
            return cuccioloNascosto;
        }
        if (nomeOggetto.Contains("pezzoChiave0"))
        {
            return pezzoChiave0;
        }
        if (nomeOggetto.Contains("pezzoChiave1"))
        {
            return pezzoChiave1;
        }
        if (nomeOggetto.Contains("pezzoChiave2"))
        {
            return pezzoChiave2;
        }

        if (nomeOggetto.Contains("chiave"))
        {
            return chiave;
        }

        if (nomeOggetto.EndsWith(".tar") || nomeOggetto.EndsWith(".tar.gz"))
        {
            return archivio;
        }

        if (nomeOggetto.Contains("cassa"))
        {
            return cassa;
        }

        if (nomeOggetto.Contains("tappeto"))
        {
            return tappeto;
        }
        if (nomeOggetto.Contains("barile"))
        {
            return barile;
        }
        if (nomeOggetto.Contains("tavolino"))
        {
            return tavolino;
        }

        if (nomeOggetto.Contains("libreria"))
        {
            return libreria;
        }

        if (nomeOggetto.Contains("pozione"))
        {
            return pozione;
        }

        if (nomeOggetto.Contains("pergamena"))
        {
            return pergamena;
        }

        if (nomeOggetto.Contains("NPC"))
        {
            return npc;
        }
        
        if (nomeOggetto.Contains ("osso"))
        {
            return osso;
        }

        if (nomeOggetto.Contains("crateShop"))
        {
            return crateShop;
        }

        if (nomeOggetto.Contains("moonStone"))
        {
            return moonStone;
        }

        if (nomeOggetto.Contains("blueCrystal"))
        {
            return blueCrystal;
        }

        if(nomeOggetto.Contains("purpleCrystal"))
        {
            return blueCrystal;
        }
        else
        {
            return pergamena;
        }
    }

    public GameObject PickStandardRoomPrefab()
    {
        int random;
        if (!isSpawnedProcessi)
        {
            isSpawnedProcessi = true;
            Debug.Log("stanza processi spawnata");
            return standardRoomPrefab[3];
        }
        random = Random.Range(0, standardRoomPrefab.Count);
        while(random == 3)
        {
            random = Random.Range(0, standardRoomPrefab.Count);
        }
        return standardRoomPrefab[random];
    }

    public GameObject PickLevelPrefab(int level)
    {
        GameObject chosenPrefab;
        level++;


        if (level == 1 && level1Prefab != null)
        {
            chosenPrefab = level1Prefab[Random.Range(0, level1Prefab.Count)];
        }
        else if (level == 2 && level2Prefab != null)
        {
            chosenPrefab = level2Prefab[Random.Range(0, level2Prefab.Count)];
            if(chosenPrefab.name == "trappoleRoomPrefab" && isTrapLevelBeingSpawn)
            {
                level--;//fix lvl 4 
                return PickLevelPrefab(level);
            }

        }
        else if (level == 3 && level3Prefab != null)
        {
            chosenPrefab = level3Prefab[Random.Range(0, level3Prefab.Count)];
            if (chosenPrefab.name == "trappoleRoomPrefab" && isTrapLevelBeingSpawn)
            {
                level--;//fix lvl 4
                return PickLevelPrefab(level);
            }
        }
        else if (level == 4 && level4Prefab != null)
        {
            chosenPrefab = level4Prefab[Random.Range(0, level4Prefab.Count)];
        }
        else if (level == 5 && level5Prefab != null)
        {
            chosenPrefab = level5Prefab[Random.Range(0, level5Prefab.Count)];
        }
        else
        {
            chosenPrefab = defaultLevel;
        }

        if (chosenPrefab.name == "trappoleRoomPrefab")
        {
            isTrapLevelBeingSpawn = true;
        }

        return chosenPrefab;
    }

    public GameObject PickLootPrefab(int numberLoot)
    {
        GameObject chosenPrefab;
        if(numberLoot <= lootPrefab.Count-1)
        {
            chosenPrefab = lootPrefab[numberLoot];
        }
        else
        {
            chosenPrefab = defaultLevel;
        }
        return chosenPrefab;
    }

    // Prefab Sprites
    public Sprite Axe
    {
        get
        {
            return axe;
        }

        set
        {
            axe = value;
        }
    }

    public Sprite Potion
    {
        get
        {
            return potion;
        }

        set
        {
            potion = value;
        }
    }

    public Sprite BigPotion
    {
        get
        {
            return bigPotion;
        }

        set
        {
            bigPotion = value;
        }
    }

}
