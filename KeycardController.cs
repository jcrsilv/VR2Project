using UnityEngine;

public class KeycardController : MonoBehaviour
{
    private Transform handle; // Referência ao objeto "Handle"
    private Transform playerController; // Referência ao controle do jogador

    private Vector3 initialPosition; // A posição inicial do Keycard

    private bool isHeld = false; // Flag para rastrear se o Keycard está sendo segurado

    private void Start()
    {
        // Encontre o objeto "Handle" usando o nome, considere usar tags para identificá-lo
        handle = transform.Find("Handle");

        // Salve a posição inicial do Keycard
        initialPosition = transform.position;

        // Encontre o controle do jogador (geralmente é o controle principal da câmera)
        playerController = Camera.main.transform;
    }

    private void Update()
    {
        // Verifique se o Keycard está sendo segurado e se há um controle do jogador
        if (isHeld && playerController != null)
        {
            // Atualize a posição do Keycard para seguir o controle do jogador
            transform.position = playerController.position + playerController.forward * 0.5f; // Ajuste a posição conforme necessário
        }
    }

    public void OnPickup()
    {
        // Quando o Keycard for pego, ative o movimento
        isHeld = true;
    }

    public void OnDrop()
    {
        // Quando o Keycard for solto, desative o movimento e retorne-o à posição inicial
        isHeld = false;
        transform.position = initialPosition;
    }
}