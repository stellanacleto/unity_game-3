using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Rigidbody2D rig;
    private Animator anim;
    public Transform player;
    public float speed;
    public Transform rightCol;
    public Transform leftCol;
    public Transform headPoint;
    private bool colliding;
    public LayerMask layer;
    public CircleCollider2D circleCollider2D;

    public int oxygenDamage = 5;
    public float damageCooldown = 1.5f; // Tempo de espera entre danos consecutivos
    private float lastDamageTime;
   
   void Start()
   {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
   }

    // Update is called once per frame
    void Update()
    {
        rig.linearVelocity = new Vector2(speed, rig.linearVelocity.y);

        colliding = Physics2D.Linecast(rightCol.position, leftCol.position, layer);

        if(colliding)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
            speed *= -1f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (!FruitManager.instance.IsInvincible())
            {
                Debug.Log("Smile colidiu com o jogador!");
                DamagePlayer();
            }
            float height = collision.contacts[0].point.y - headPoint.position.y;

            if(height > 0)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                speed = 0;
                anim.SetTrigger("die");
                circleCollider2D.enabled = false;
                rig.bodyType = RigidbodyType2D.Kinematic;
                Destroy(gameObject, 0.36f);
            }
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
