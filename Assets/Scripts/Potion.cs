using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private SpriteRenderer sr;
    private CapsuleCollider2D capsule;
    public GameObject collected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        capsule = GetComponent<CapsuleCollider2D>();
    }

  
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            AudioManager.instance.PlayPickupSound();
            sr.enabled = false;
            capsule.enabled = false;
            collected.SetActive(true);
            
            GameController.instance.AddOxygen(10f); // Adiciona 10 segundos de oxigênio

            Destroy(gameObject, 0.25f);
        }
    }
}
