using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public TextMeshProUGUI messageText; // Referência ao texto na tela
    public GameObject messagePanel; // Painel que contém a mensagem
    public string[] storyLines; // Linhas da mensagem para contar a história
    public float typingSpeed = 0.05f; // Velocidade de digitação

    private int currentLine = 0; // Linha atual da história
    private bool isTyping = false; // Controle de digitação em andamento

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Pausa o cronômetro do oxigênio
            GameController.instance.PauseOxygenCountdown();

            // Exibe o painel e inicia a digitação
            messagePanel.SetActive(true);
            AudioManager.instance.PlayComputerSound();
            AudioManager.instance.StopWarningSound();
            if (!isTyping)
            {
                StartCoroutine(TypeMessage());
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Fecha o painel e retoma o cronômetro
            messagePanel.SetActive(false);
            GameController.instance.ResumeOxygenCountdown();
            AudioManager.instance.StopComputerSound();
        }
    }

    IEnumerator TypeMessage()
    {
        isTyping = true;
        messageText.text = "";

        foreach (char letter in storyLines[currentLine].ToCharArray())
        {
            messageText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        AudioManager.instance.StopComputerSound();
        isTyping = false;
        currentLine = (currentLine + 1) % storyLines.Length; // Avança para a próxima linha
    }
}
