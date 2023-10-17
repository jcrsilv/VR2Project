using UnityEngine;

public class KeycardController : MonoBehaviour
{
    private Transform handle; // Refer�ncia ao objeto "Handle"
    private Transform playerController; // Refer�ncia ao controle do jogador

    private Vector3 initialPosition; // A posi��o inicial do Keycard

    private bool isHeld = false; // Flag para rastrear se o Keycard est� sendo segurado

    private void Start()
    {
        // Encontre o objeto "Handle" usando o nome, considere usar tags para identific�-lo
        handle = transform.Find("Handle");

        // Salve a posi��o inicial do Keycard
        initialPosition = transform.position;

        // Encontre o controle do jogador (geralmente � o controle principal da c�mera)
        playerController = Camera.main.transform;
    }

    private void Update()
    {
        // Verifique se o Keycard est� sendo segurado e se h� um controle do jogador
        if (isHeld && playerController != null)
        {
            // Atualize a posi��o do Keycard para seguir o controle do jogador
            transform.position = playerController.position + playerController.forward * 0.5f; // Ajuste a posi��o conforme necess�rio
        }
    }

    public void OnPickup()
    {
        // Quando o Keycard for pego, ative o movimento
        isHeld = true;
    }

    public void OnDrop()
    {
        // Quando o Keycard for solto, desative o movimento e retorne-o � posi��o inicial
        isHeld = false;
        transform.position = initialPosition;
    }
}