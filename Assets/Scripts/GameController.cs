using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public int totalShip;
    public int oxygen = 30;
    public TextMeshProUGUI shipText;

    public float oxygenTime = 30f; // Tempo inicial de oxigênio
    public TextMeshProUGUI oxygenText;

    public GameObject gameOver;
    public GameObject pauseMenu;

    private bool isGameOver = false;
    private bool isPaused = false;
    public static GameController instance;

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

    public void UpdateShipText()
    {
        shipText.text = totalShip.ToString();
    }

    public void UpdateOxygenText()
    {
        oxygenText.text = Mathf.CeilToInt(oxygenTime).ToString() + "s";
    }

    public void AddOxygen(float amount)
    {
        oxygenTime += amount;
        UpdateOxygenText();
    }

    public void ReduceOxygen(int amount)
    {
        oxygen -= amount; // Subtrai o oxigênio
        UpdateOxygenText();

        if (oxygen <= 0)
        {
            ShowGameOver(); // Game Over se o oxigênio acabar
        }
    }

    public void ShowGameOver()
    {
        gameOver.SetActive(true);
        isGameOver = true;
        Time.timeScale = 0f;
    }

    public void RestartGame(string lvlName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(lvlName);
    }

    private IEnumerator OxygenCountdown()
    {
        while (oxygenTime > 0 && !isGameOver)
        {
            yield return new WaitForSeconds(1f);
            oxygenTime -= 1f;
            UpdateOxygenText();
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
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0; // Congela o jogo
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1; // Retoma o jogo
    }
}
