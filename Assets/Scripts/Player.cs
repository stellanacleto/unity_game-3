using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    public bool isJumping;
    public bool doubleJump;

    public int oxygen = 20;

    private Rigidbody2D rig;
    private Animator anim;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move(){
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"),0f, 0f);
        transform.position += movement * Time.deltaTime * Speed;

        if (Input.GetAxis("Horizontal") > 0f)
        {
            anim.SetBool("walk", true);
            transform.eulerAngles = new Vector3(0f,0f,0f);
        }
        if (Input.GetAxis("Horizontal") < 0f)
        {
            anim.SetBool("walk", true);
            transform.eulerAngles = new Vector3(0f,180f,0f);
        }
        if (Input.GetAxis("Horizontal") == 0f)
        {
            anim.SetBool("walk", false);
        }
        
    }

    void Jump(){
        if(Input.GetButtonDown("Jump")){
            if(!isJumping){
                rig.AddForce(new Vector2(0f,JumpForce), ForceMode2D.Impulse);
                anim.SetBool("jump", true);
                doubleJump = true;
            }
            else {
                if(doubleJump){
                    rig.AddForce(new Vector2(0f,JumpForce), ForceMode2D.Impulse);
                    anim.SetBool("jump", false);
                    doubleJump = false;
                }
            }
            
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.layer == 8){
            anim.SetBool("jump", false);
            isJumping = false;
        }
        if(collision.gameObject.tag == "Spike"){
            GameController.instance.ShowGameOver();
            Destroy(gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D collision){
        if(collision.gameObject.layer == 8){
            anim.SetBool("jump", true);
            isJumping = true;
        }
    }

    public void ReduceOxygen(int amount)
    {
        oxygen -= amount;
        Debug.Log("Oxygen: " + oxygen);

        // Verifica se o jogador ficou sem oxigênio
        if (oxygen <= 0)
        {
            GameController.instance.ShowGameOver();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy") // Verifica se é um inimigo
        {
            GameController.instance.ReduceOxygen(5); // Reduz 5 porções de oxigênio
        }
    }

}
