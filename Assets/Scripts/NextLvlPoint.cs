using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLvlPoint : MonoBehaviour
{
    public string lvlName;
    public int numberShips;

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Player" && GameController.instance.totalShip == numberShips){
            SceneManager.LoadScene(lvlName);
            FruitManager.instance.isScene = false;
        }
    
    }
}
