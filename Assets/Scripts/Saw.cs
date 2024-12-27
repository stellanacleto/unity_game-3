using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float speed;
    public float moveTime;
    private bool dirRight = true;
    private float timer;
    public Transform player;
    public int sawOxygenDamage = 5;
    public float sawDamageCooldown = 1.5f;
    private float sawLastDamageTime;

    // Update is called once per frame
    void Update()
    {
        if(dirRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        timer += Time.deltaTime;

        if(timer >= moveTime)
        {
            dirRight = !dirRight;
            timer = 0f;
        }

    }

    void OnCollisionEnter2D(Collision2D collision){
        
        if (collision.gameObject.CompareTag("Player") && !FruitManager.instance.IsInvincible())
        {
            AudioManager.instance.PlayDieSound();
            Debug.Log("A serra colidiu com o jogador!");
            SawDamagePlayer();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time > sawLastDamageTime + sawDamageCooldown)
            {
                SawDamagePlayer();
            }
        }
    }

    void SawDamagePlayer()
    {
        Player playerScript = player.GetComponent<Player>();
        
        if (playerScript != null)
        {
            playerScript.PlayerReduceOxygen(sawOxygenDamage);
        }
        else
        {
            Debug.LogError("Script Player não encontrado no jogador!");
        }

        sawLastDamageTime = Time.time; // Atualiza o tempo do último dano
    }
}
