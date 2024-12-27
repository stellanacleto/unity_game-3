using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    public bool isJumping;
    public bool doubleJump;
    public int numeroFrutas = 0;

    private Rigidbody2D rig;
    private Animator anim;

    public GameObject potionPanel;
    public GameObject shipPanel;
    public GameObject fruitsPanel;

    private SpriteRenderer sr; // Para alterar a cor do sprite
    private bool isInvincible = false; // Controla se o jogador está invencível
    private Color originalColor; // Armazena a cor original do jogador
    public Color invincibleColor = Color.yellow; // Cor durante a invencibilidade

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color; // Armazena a cor original do sprite
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFruit();
        Move();
        Jump();
        
        if (Input.GetKeyDown(KeyCode.X) && FruitManager.instance.totalFruits >= 10)
        {
            FruitManager.instance.ActivateInvincibility();
            ActivateInvincibility(10f);
        }
        
        
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
                    anim.SetBool("jump", true);
                    doubleJump = false;
                }
            }
            
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.layer == 8 || collision.gameObject.layer == 7){            
            anim.SetBool("jump", false);
            isJumping = false;        
        }
        if(collision.gameObject.tag == "Spike" && !FruitManager.instance.IsInvincible()){
            AudioManager.instance.PlayDieSound();  
            anim.SetTrigger("hit");
            UpdateFruit();
            GameController.instance.ShowGameOver();
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Saw" && !FruitManager.instance.IsInvincible()){
            AudioManager.instance.PlayDieSound();
            anim.SetTrigger("hit");
        }
        if(collision.gameObject.tag == "Smile" && !FruitManager.instance.IsInvincible()){
            AudioManager.instance.PlayDieSound();
            anim.SetTrigger("hit");
            UpdateFruit();
            GameController.instance.ShowGameOver();
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Win"){
            AudioManager.instance.PlayWinSound();
            GameController.instance.Win();
        }
    }

    void OnCollisionExit2D(Collision2D collision){
        if(collision.gameObject.layer == 8 || collision.gameObject.layer == 7)
        {            
            anim.SetBool("jump", true);
            isJumping = true;           
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.CompareTag("Enemy") && !FruitManager.instance.IsInvincible()){
            AudioManager.instance.PlayDieSound();
            anim.SetTrigger("hit");
        }
        if(collider.gameObject.CompareTag("Potion")){
            
            potionPanel.SetActive(true);
        }
        if(collider.gameObject.CompareTag("Ship")){
            
            shipPanel.SetActive(true);
        }
        if(collider.gameObject.CompareTag("Fruits")){
            
            fruitsPanel.SetActive(true);
        }
        if(collider.gameObject.CompareTag("Fruit")){
            
            numeroFrutas++;            
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.CompareTag("Potion")){
            potionPanel.SetActive(false);
        }
        if(collider.gameObject.CompareTag("Ship")){
            shipPanel.SetActive(false);
        }
        if(collider.gameObject.CompareTag("Fruits")){
            
            fruitsPanel.SetActive(false);
        }
    }

    public void UpdateFruit(){
        if (GameController.instance.isGameOver || GameController.instance.isRestart) // Adiciona essa verificação
        {   
            FruitManager.instance.ResetFruitsInCurrentScene(numeroFrutas);
            if(FruitManager.instance.isScene)
            {
                FruitManager.instance.AddFruit(10);
                FruitManager.instance.isScene = false;
            }
            
        }
    }

    public void PlayerReduceOxygen(int amount)
    {
        GameController.instance.ReduceOxygen(amount);

        // Verifica se o jogador ficou sem oxigênio
        if (GameController.instance.oxygen <= 0)
        {   
            AudioManager.instance.PlayDieSound();
            GameController.instance.ShowGameOver();
            Destroy(gameObject);
        }
    }

    public void ActivateInvincibility(float duration)
    {
        if (!isInvincible)
        {
            StartCoroutine(InvincibilityCoroutine(duration));
        }
    }
    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true; // Ativa a invencibilidade
        sr.color = invincibleColor; // Muda para a cor da invencibilidade

        yield return new WaitForSeconds(duration); // Aguarda o tempo de invencibilidade

        sr.color = originalColor; // Restaura a cor original
        isInvincible = false; // Desativa a invencibilidade
    }
}