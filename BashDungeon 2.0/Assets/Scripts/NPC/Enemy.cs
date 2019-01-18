using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    Animator animator;
    public Room currentRoom;
    GameObject gameManager;
    GameObject player;
    NavMeshAgent m_agent;
    Vector3 playerPosition;

	// Use this for initialization
	void Start () {
        animator = this.GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager");
        currentRoom = gameManager.GetComponent<LevelGeneration>().GetRoomByName(this.transform.parent.name);
        player = GameObject.Find("Player");
        //m_agent = this.gameObject.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        playerPosition = player.transform.position;
        Fly();
	}

    public void Fly()
    {
        if (currentRoom == player.GetComponent<PlayerMovement>().currentRoom)
        {
            animator.SetBool("PlayerIsHere", true);
            transform.LookAt(playerPosition);
            transform.forward += transform.forward * 0.5f * Time.deltaTime;
            //m_agent.destination = playerPosition;
        }
        else
        {
            animator.SetBool("PlayerIsHere", false);
        }
    }
}
