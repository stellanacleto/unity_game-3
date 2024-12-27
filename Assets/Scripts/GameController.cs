using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public int totalShip;
    public float oxygen = 20f;
    public TextMeshProUGUI shipText;

    public TextMeshProUGUI oxygenText;

    public GameObject gameOver;
    public GameObject pauseMenu;
    public GameObject win;
    private bool isOxygenPaused = false; // Controle do estado do cronômetro
    public bool isGameOver = false;
    public bool wasGameOver = false;
    private bool isPaused = false;
    public bool isRestart = false;
    public static GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;           
        }      
    }

    void Start()
    {
        instance = this;
        UpdateShipText();
        UpdateOxygenText();
        StartCoroutine(OxygenCountdown());
    }

    void Update()
    {
        HandlePause(); // Verifica a tecla de pausa
    }

    public void PauseOxygenCountdown()
    {
        AudioManager.instance.StopWarningSound();
        isOxygenPaused = true;
    }

    public void ResumeOxygenCountdown()
    {
        isOxygenPaused = false;
    }

    public void UpdateOxygenText()
    {
        oxygenText.text = Mathf.CeilToInt(oxygen).ToString();
    }

    public void UpdateShipText()
    {
        shipText.text = Mathf.CeilToInt(totalShip).ToString();
    }

    public void AddOxygen(float amount)
    {
        oxygen += amount;
        UpdateOxygenText();
    }

    public void ReduceOxygen(float amount)
    {
        oxygen -= amount; // Subtrai o oxigênio
        if(oxygen < 0)
        {
            oxygen = 0;
        }

        UpdateOxygenText();

        if (oxygen <= 0 && !isGameOver)
        {          
            ShowGameOver(); // Game Over se o oxigênio acabar
        }

    }

    public void ShowGameOver()
    {
        gameOver.SetActive(true);
        isGameOver = true;
        wasGameOver = true;
        Time.timeScale = 0f;
        AudioManager.instance.StopBackgroundMusic();
        AudioManager.instance.StopWarningSound();
    }

    public void RestartGame(string lvlName)
    {
        if(!wasGameOver)
        {
            isRestart = true;
        }
        wasGameOver = false;
        AudioManager.instance.PlayBackgroundMusic();
        Time.timeScale = 1;
        SceneManager.LoadScene(lvlName);
    }

    private IEnumerator OxygenCountdown()
    {
        while (oxygen > 0 && !isGameOver)
        {
            if (!isOxygenPaused)
            {
                yield return new WaitForSeconds(1f);
                ReduceOxygen(1f);
            }
            if (oxygen <= 10f && !AudioManager.instance.isWarningPlaying)
            {
                AudioManager.instance.PlayWarningSound();
            }
            if (oxygen == 0f)
            {
                AudioManager.instance.StopWarningSound();
            }
            else if (oxygen > 10f || isGameOver || isPaused)
            {
                AudioManager.instance.StopWarningSound();
            }

            if (isOxygenPaused)
            {
                yield return null; // Pausa o cronômetro
            }
            
        }

        if (!isGameOver)
        {
            ShowGameOver();
        }
    }

    private void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) // Tecla de pausa
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        AudioManager.instance.StopBackgroundMusic();
        AudioManager.instance.StopWarningSound();
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0; // Congela o jogo    
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1; // Retoma o jogo
        AudioManager.instance.PlayBackgroundMusic();
    }

    public void Win()
    {
        AudioManager.instance.StopBackgroundMusic();
        AudioManager.instance.StopWarningSound();
        isPaused = true;
        win.SetActive(true);
        Time.timeScale = 0; // Congela o jogo    
    }

    public void Menu()
    {
        FruitManager.instance.ResetFruits();
        SceneManager.LoadScene("menu");
        isPaused = false;
        isRestart = false;
        isGameOver = false;
        Time.timeScale = 1;
        AudioManager.instance.PlayBackgroundMusic();
        
    }
}
