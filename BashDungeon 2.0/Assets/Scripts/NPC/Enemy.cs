﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    Animator animator;
    public Room currentRoom;
    GameObject gameManager;
    GameObject player;
    //NavMeshAgent m_agent;
    Vector3 playerPosition;
   // Rigidbody m_Rigidbody;
    public float m_Speed = 0.5f;

	// Use this for initialization
	void Start () {
        animator = this.GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager");
        currentRoom = gameManager.GetComponent<LevelGeneration>().GetRoomByName(this.transform.parent.name);
        player = GameObject.Find("Player");
        //m_Rigidbody = this.gameObject.GetComponent<Rigidbody>();
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
            //m_Rigidbody.velocity = transform.forward * m_Speed *Time.deltaTime;
            transform.position += transform.forward * 2 * Time.deltaTime;
            //m_agent.destination = playerPosition;
        }
        else
        {
            animator.SetBool("PlayerIsHere", false);
        }
    }
}
