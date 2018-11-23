﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectBehavior : MonoBehaviour {

    public bool isMadeVisible;
    bool isBeingCompressed;
    bool isVisible;

    //GameObject playerGO;
   
    Color myColorAlphaZero;
    Color myColorFullAlpha;
    public List<GameObject> oggettiGOArchiviati;
    public List<Oggetto> oggettiArchiviati;

    MeshRenderer[] meshRendererInChildren;

    GameObject gameManager;

    public bool IsBeingCompressed
    {
        get
        {
            return isBeingCompressed;
        }

        set
        {
            isBeingCompressed = value;
        }
    }

    void Start()
    {
        //playerGO = GameObject.FindGameObjectWithTag("Player");
        if(transform.childCount == 0)
        {
            transform.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            myColorAlphaZero = transform.GetComponent<MeshRenderer>().material.color;
            myColorAlphaZero.a = 0;
            myColorFullAlpha = transform.GetComponent<MeshRenderer>().material.color;
            myColorFullAlpha.a = 75;
        }
        else
        {
            meshRendererInChildren = transform.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer meshR in meshRendererInChildren)
            {
                meshR.material.EnableKeyword("_EMISSION");
            }

            myColorAlphaZero = transform.GetComponentInChildren<MeshRenderer>().material.color;
            myColorAlphaZero.a = 0;
            myColorFullAlpha = transform.GetComponentInChildren<MeshRenderer>().material.color;
            myColorFullAlpha.a = 75;
        }
        
        gameManager = GameObject.Find("GameManager");

    }

    private void Update()
    {

        if (transform.name.Contains("."))
        {
            isVisible = false;
        }

        if (!isVisible)
        {
            if (transform.name.Contains(".") && !isMadeVisible)
            {
                isVisible = false;
                if(transform.childCount == 0)
                {
                    transform.GetComponent<MeshRenderer>().material.color = myColorAlphaZero;
                }
                else
                {
                    foreach(MeshRenderer meshR in meshRendererInChildren)
                    {
                        meshR.material.color = myColorAlphaZero;
                    }
                   
                }
                
            }
            else if (transform.name.Contains(".") && isMadeVisible)
            {
                if (transform.childCount == 0)
                {
                    transform.GetComponent<MeshRenderer>().material.color = myColorFullAlpha;
                }
                else
                {
                    foreach (MeshRenderer meshR in meshRendererInChildren)
                    {
                        meshR.material.color = myColorFullAlpha;
                    }
                }
            }

            if (transform.childCount == 0)
            {
                if (transform.GetComponent<MeshRenderer>().material.color.a == 75)
                {
                    isVisible = true;
                }
            }
            else
            {
                if (transform.GetComponentInChildren<MeshRenderer>().material.color.a == 75)
                {
                    isVisible = true;
                }
            }
        }

        if(this.gameObject.name.EndsWith(".tar.gz") && !IsBeingCompressed)
        {
            Vector3 scaledVector = this.gameObject.transform.localScale;
            scaledVector.x /= 2;
            scaledVector.z /= 2;
            scaledVector.y /= 2;
            this.gameObject.transform.localScale = scaledVector;

            IsBeingCompressed = true;
        }

        if(oggettiGOArchiviati.Count == 1)
        {
            if(oggettiGOArchiviati[0].transform.name.Contains("Gigante") && IsBeingCompressed)
            {
                Oggetto thisOggetto = gameManager.GetComponent<LevelGeneration>().GetRoomByName(gameObject.transform.parent.name).oggetti.Find(x => x.nomeOggetto.Contains(gameObject.transform.name) && x.IsActive);
               /* if (oggettiGOArchiviati[0].transform.name.Contains("chiave"))
                    thisOggetto.nomeOggetto = thisOggetto.nomeOggetto.Replace(thisOggetto.nomeOggetto, "chiave");
                else*/
                    thisOggetto.nomeOggetto = thisOggetto.nomeOggetto.Replace(".tar.gz", "");
                thisOggetto.IsMovable = true;
                thisOggetto.IsTar = false;
                thisOggetto.IsRenamable = true;
                /*if (oggettiGOArchiviati[0].transform.name.Contains("chiave"))
                    gameObject.transform.name = gameObject.transform.name.Replace(gameObject.transform.name, "chiave");
                else*/
                    gameObject.transform.name = gameObject.transform.name.Replace(".tar.gz", "");
                oggettiGOArchiviati.Remove(oggettiGOArchiviati[0]);
            }
        }
        else if(oggettiGOArchiviati.Count == 3 && oggettiGOArchiviati.FindAll(x => x.transform.name.Contains("pezzoChiave")).Count == 3)
        {
            foreach(GameObject go in oggettiGOArchiviati)
            {
                gameManager.GetComponent<LevelGeneration>().GetRoomByName(gameObject.transform.parent.name).oggetti.Find(x => x.nomeOggetto == go.name).IsActive = false;
            }
            Oggetto thisOggetto = gameManager.GetComponent<LevelGeneration>().GetRoomByName(gameObject.transform.parent.name).oggetti.Find(x => x.nomeOggetto.Contains(gameObject.transform.name) && x.IsActive);
            thisOggetto.nomeOggetto = "chiave";
            thisOggetto.IsMovable = true;
            thisOggetto.IsTar = false;
            gameObject.transform.name = "chiave";

            oggettiGOArchiviati.Clear();
        }
    }

    public void SettaOff()
    {
        this.enabled = false;
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            gameManager.GetComponent<PlayManager>().ClickedObject = gameObject;
        }
    }

    private void OnMouseEnter()
    {
        gameManager.GetComponent<PlayManager>().IsMouseOverObj = true;
    }
    private void OnMouseExit()
    {
        gameManager.GetComponent<PlayManager>().IsMouseOverObj = false;
    }


}
