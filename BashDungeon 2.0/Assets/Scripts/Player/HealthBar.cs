using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public float health = 100;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = new Vector3((health / 100f), transform.localScale.y, transform.localScale.z);
        GameObject.Find("HP").GetComponent<Text>().text = ("HP ") + health + ("/100");
    }

    public void LoseHealth(float hp)
    {
        if((health - hp) > 0)
        {
            health -= hp;
        }
        else
        {
            health = 0;
        }        
    }
}
