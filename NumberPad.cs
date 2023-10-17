using TMPro;
using UnityEngine;
using System.Collections;


// Importe o namespace UnityEngine.UI

public class NumberPad : MonoBehaviour
{
    public string correctCode = "1234"; // Defina o código correto aqui
    private string userInput = "";
    private int buttonPressCount = 0; // Contador de pressionamentos de botões
    public Transform tableLocation; // Localização da mesa onde o cartão-chave será gerado
    public TMP_Text displayText; // Referência ao componente de Texto para os numeros
    public GameObject keycardPrefab;
    public TMP_Text messageText; // Referência ao objeto de texto para mensagens
    public float messageDisplayDuration = 2.0f; // Duração em segundos para exibir mensagens

    // Método para adicionar um número à entrada do usuário
    public void AddNumber(int number)
    {
        userInput += number.ToString();
        buttonPressCount++;

        // Verifique se o usuário inseriu o número correto de caracteres
        if (buttonPressCount == correctCode.Length)
        {
            CheckCode();
        }

        // Atualize o texto exibido
        UpdateDisplay();
    }

    // Método para verificar o código inserido pelo usuário
    private void CheckCode()
    {
        if (userInput == correctCode)
        {
            ShowMessage("Código correto!", Color.green);
            SpawnKeycard();
        }
        else
        {
            ShowMessage("Código incorreto. Tente novamente.", Color.red);
            ResetNumberPad();
        }
    }

    // Método para redefinir o teclado numérico
    public void ResetNumberPad()
    {
        userInput = "";
        buttonPressCount = 0;

        // Limpe o texto exibido quando o teclado numérico é redefinido
        UpdateDisplay();
    }

    // Método para gerar um cartão-chave na mesa
    private void SpawnKeycard()
    {
        if (tableLocation != null && keycardPrefab != null)
        {
            GameObject keycard = Instantiate(keycardPrefab, tableLocation.position, tableLocation.rotation);

            // Crie o cartão-chave ou ative um objeto de cartão-chave existente na localização da mesa
            // Certifique-se de definir a posição e a rotação corretas para o cartão-chave
            // Exemplo: Instantiate(keycardPrefab, tableLocation.position, tableLocation.rotation);
        }

        // Redefina o teclado numérico após o cartão-chave ser gerado
        ResetNumberPad();
    }

    private void ShowMessage(string text, Color color)
    {
        if (messageText != null)
        {
            messageText.text = text;
            messageText.color = color;
            StartCoroutine(HideMessageAfterDelay());
        }
    }

    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDisplayDuration);
        if (messageText != null)
        {
            messageText.text = "";
        }
    }

    // Método para atualizar o texto exibido
    private void UpdateDisplay()
    {
        if (displayText != null)
        {
            displayText.text = userInput;
        }
    }
}


