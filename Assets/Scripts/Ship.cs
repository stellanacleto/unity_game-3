using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D boxCollShip;
    public GameObject collectedShip;
    public int nShip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        boxCollShip = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            AudioManager.instance.PlayPickupSound();
            sr.enabled = false;
            boxCollShip.enabled = false;
            collectedShip.SetActive(true);

            GameController.instance.totalShip += nShip;
            GameController.instance.UpdateShipText();
            

            Destroy(gameObject, 0.25f);
        }
    }
}
