using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    public float speed = 2f; // Velocidade de movimento do inimigo
    public float stopDistance = 0.5f; // Distância mínima para parar de seguir o jogador

    public int oxygenDamage = 5; // Quantidade de oxigênio que o jogador perde ao tocar no inimigo
    public float damageCooldown = 1.5f; // Tempo de espera entre danos consecutivos

    private Rigidbody2D rb;
    private float lastDamageTime; // Marca o tempo do último dano causado

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Se não encontrar o jogador, automaticamente procura por ele no início
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        // Verifica a distância entre o inimigo e o jogador
        if (Vector2.Distance(transform.position, player.position) > stopDistance)
        {
            // Move o inimigo na direção do jogador
            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DamagePlayer();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Time.time > lastDamageTime + damageCooldown)
            {
                DamagePlayer();
            }
        }
    }

    void DamagePlayer()
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.ReduceOxygen(oxygenDamage);
        }

        lastDamageTime = Time.time; // Atualiza o tempo do último dano
    }
}
