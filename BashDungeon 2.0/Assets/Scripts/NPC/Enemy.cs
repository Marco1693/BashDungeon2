using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    Animator animator;
    public Room currentRoom;
    GameObject gameManager;
    GameObject player;

	// Use this for initialization
	void Start () {
        animator = this.GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager");
        currentRoom = gameManager.GetComponent<LevelGeneration>().GetRoomByName(this.gameObject.GetComponentInParent<Transform>().name);
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        Fly();
	}

    public void Fly()
    {
        if (currentRoom == player.GetComponent<PlayerMovement>().currentRoom)
        {
            animator.SetBool("PlayerIsHere", true);
        }
        else
        {
            animator.SetBool("PlayerIsHere", false);
        }
    }
}
