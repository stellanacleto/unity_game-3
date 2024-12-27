using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    public float speed = 1f; // Velocidade de movimento do inimigo
    public float stopDistance = 0.5f; // Distância mínima para parar de seguir o jogador

    public int oxygenDamage = 5; // Quantidade de oxigênio que o jogador perde ao tocar no inimigo
    public float damageCooldown = 1.5f; // Tempo de espera entre danos consecutivos

    private Rigidbody2D rb;
    private float lastDamageTime; // Marca o tempo do último dano causado
    private bool isFacingRight = true; // Controle para a direção atual

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
        Vector2 direction = (player.position - transform.position).normalized;

        // Move o inimigo na direção do jogador
        rb.linearVelocity = direction * speed;
        
        // Inverte o morcego se necessário
        if ((direction.x > 0 && isFacingRight) || (direction.x < 0 && !isFacingRight))
        {
            Flip();
        }    
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        // Inverte a escala do morcego no eixo X
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && !FruitManager.instance.IsInvincible())
        {
            Debug.Log("Morcego colidiu com o jogador!");
            DamagePlayer();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
            Debug.Log("Reduzindo oxigênio do jogador...");
            playerScript.PlayerReduceOxygen(oxygenDamage);
        }
        else
        {
            Debug.LogError("Script Player não encontrado no jogador!");
        }

        lastDamageTime = Time.time; // Atualiza o tempo do último dano
    }
}
