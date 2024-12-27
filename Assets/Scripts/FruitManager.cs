using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using TMPro;
using UnityEngine.UI;

public class FruitManager : MonoBehaviour
{
    public static FruitManager instance;
    public TextMeshProUGUI fruitText; // Referência ao texto no Canvas
    public int totalFruits = 0; // Total de frutas coletadas
    private int fruitsInCurrentScene = 0; // Frutas disponíveis na cena atual
    public float invincibilityDuration = 10f; // Duração da invencibilidade
    private bool isInvincible = false; // Status do jogador
    public bool isScene = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateFruitText();
    }    

    private void Update()
    {
        UpdateFruitText();
    } 

    private void OnLevelWasLoaded(int level)
    {
        // Procura o texto no Canvas da nova cena
        FindFruitTextInScene();
        UpdateFruitText();
    }

    public void AddFruit(int amount)
    {
        totalFruits += amount;
        fruitsInCurrentScene--; // Diminui a contagem das frutas restantes na cena atual
        UpdateFruitText();
    }

    public void UpdateFruitText()
    {
        if (fruitText == null)
        {
            FindFruitTextInScene(); // Procura o texto na nova cena
        }        
        if (fruitText != null)
        {
            fruitText.text = totalFruits.ToString();
        }
    }

    public void ActivateInvincibility()
    {
        if (totalFruits >= 10 && !isInvincible)
        {
            isScene = true;
            totalFruits -= 10;
            UpdateFruitText();
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        // Aqui você pode ativar um efeito visual para indicar invencibilidade
        Debug.Log("Invencível por 10 segundos!");
        
        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;

        // Aqui você pode desativar o efeito visual de invencibilidade
        Debug.Log("Invencibilidade acabou!");
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    private void FindFruitTextInScene()
    {
        // Procura o texto do Canvas na nova cena
        GameObject textObject = GameObject.Find("FruitText"); // Nome do objeto de texto no Canvas
        if (textObject != null)
        {
            fruitText = textObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("FruitText não foi encontrado na cena.");
        }
    }

    public void ResetFruitsInCurrentScene(int amount)
    {
        totalFruits -= amount;
        //fruitsInCurrentScene++; // Aumenta a contagem das frutas restantes na cena atual
        //UpdateFruitText();
        GameController.instance.isGameOver = false;
    }

    public void ResetFruits(){
        totalFruits = 0;
        fruitsInCurrentScene = 0;
        isInvincible = false;
    }
}
