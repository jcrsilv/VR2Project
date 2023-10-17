using TMPro;
using UnityEngine;
using System.Collections;


// Importe o namespace UnityEngine.UI

public class NumberPad : MonoBehaviour
{
    public string correctCode = "1234"; // Defina o c�digo correto aqui
    private string userInput = "";
    private int buttonPressCount = 0; // Contador de pressionamentos de bot�es
    public Transform tableLocation; // Localiza��o da mesa onde o cart�o-chave ser� gerado
    public TMP_Text displayText; // Refer�ncia ao componente de Texto para os numeros
    public GameObject keycardPrefab;
    public TMP_Text messageText; // Refer�ncia ao objeto de texto para mensagens
    public float messageDisplayDuration = 2.0f; // Dura��o em segundos para exibir mensagens

    // M�todo para adicionar um n�mero � entrada do usu�rio
    public void AddNumber(int number)
    {
        userInput += number.ToString();
        buttonPressCount++;

        // Verifique se o usu�rio inseriu o n�mero correto de caracteres
        if (buttonPressCount == correctCode.Length)
        {
            CheckCode();
        }

        // Atualize o texto exibido
        UpdateDisplay();
    }

    // M�todo para verificar o c�digo inserido pelo usu�rio
    private void CheckCode()
    {
        if (userInput == correctCode)
        {
            ShowMessage("C�digo correto!", Color.green);
            SpawnKeycard();
        }
        else
        {
            ShowMessage("C�digo incorreto. Tente novamente.", Color.red);
            ResetNumberPad();
        }
    }

    // M�todo para redefinir o teclado num�rico
    public void ResetNumberPad()
    {
        userInput = "";
        buttonPressCount = 0;

        // Limpe o texto exibido quando o teclado num�rico � redefinido
        UpdateDisplay();
    }

    // M�todo para gerar um cart�o-chave na mesa
    private void SpawnKeycard()
    {
        if (tableLocation != null && keycardPrefab != null)
        {
            GameObject keycard = Instantiate(keycardPrefab, tableLocation.position, tableLocation.rotation);

            // Crie o cart�o-chave ou ative um objeto de cart�o-chave existente na localiza��o da mesa
            // Certifique-se de definir a posi��o e a rota��o corretas para o cart�o-chave
            // Exemplo: Instantiate(keycardPrefab, tableLocation.position, tableLocation.rotation);
        }

        // Redefina o teclado num�rico ap�s o cart�o-chave ser gerado
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

    // M�todo para atualizar o texto exibido
    private void UpdateDisplay()
    {
        if (displayText != null)
        {
            displayText.text = userInput;
        }
    }
}


