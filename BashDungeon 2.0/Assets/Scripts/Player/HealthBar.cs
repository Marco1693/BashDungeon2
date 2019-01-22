using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour {

    public float health = 100;
    float maxHealth = 100;

	void Update () {

        transform.localScale = new Vector3((health / maxHealth), transform.localScale.y, transform.localScale.z);
        GameObject.Find("HP").GetComponent<Text>().text = ("HP ") + health + ("/100");
        Die();
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

    public void HealthPotion(float hp)
    {
        if((health + hp) <= maxHealth)
        {
            health += hp;
        }
        else
        {
            health = maxHealth;
        }
    }

    public void Die()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
