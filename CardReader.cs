using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardReader : XRSocketInteractor
{
    public float minSwipeDistance = 0.005f;
    public float maxSwipeDistance = 0.25f; // Distância máxima permitida para um swipe// Distância mínima para um swipe ser registrado
    public float allowedUprightErrorRange = 0.2f; // Margem de erro permitida para a orientação vertical do cartão

    private Vector3 initialCardPosition;
    private bool isCardInserted = false;
    private XRGrabInteractable insertedCard; // Referência para o cartão inserido

    public GameObject doorLock; // Referência para o objeto da trava da porta

    public GameObject greenLight; // Referência para a luz verde
    public GameObject redLight;   // Referência para a luz vermelha

    public AudioClip swipeSuccessSound; // Som para swipe bem-sucedido
    public AudioClip swipeFailSound;    // Som para swipe inválido

    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();

        // Certifique-se de que as luzes estão desligadas no início
        if (greenLight != null)
        {
            greenLight.SetActive(false);
        }

        if (redLight != null)
        {
            redLight.SetActive(false);
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (isCardInserted)
        {
            // O cartão já foi inserido, você pode interagir com ele agora
            insertedCard = args.interactable as XRGrabInteractable;
        }
        else
        {
            // Insira o cartão no leitor
            XRGrabInteractable card = args.interactable as XRGrabInteractable;

            if (card != null)
            {
                initialCardPosition = card.transform.position;
                isCardInserted = true;
                insertedCard = card; // Configure a instância de insertedCard


                // Adicione uma mensagem de depuração para verificar a detecção do cartão
                Debug.Log("Cartão detectado para inserção.");
            }
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (isCardInserted)
        {

            if (insertedCard != null)
            {
                // Calcule a distância entre a posição inicial e atual do cartão
                float swipeDistance = Vector3.Distance(initialCardPosition, insertedCard.transform.position);
                Debug.Log("Distância do swipe: " + swipeDistance);

                // Verifique a orientação vertical do cartão
                Vector3 cardUp = insertedCard.transform.forward;
                float dot = Vector3.Dot(cardUp, Vector3.up);
                Debug.Log("Orientação vertical do cartão: " + dot);

                // Se a distância do swipe for maior ou igual a minSwipeDistance
                // e a orientação vertical do cartão estiver dentro da margem de erro permitida
                if (swipeDistance >= minSwipeDistance && swipeDistance <= maxSwipeDistance &&
                    dot >= -1 - allowedUprightErrorRange || dot == -1)
                {
                    // Swipe bem-sucedido, faça o que for necessário, como abrir a porta
                    // Remova a trava e a barra da porta
                    // Exemplo: doorLock.SetActive(false);
                    Debug.Log("Swipe bem-sucedido. Porta destravada!");

                    if (doorLock != null)
                    {
                        doorLock.SetActive(false);
                    }

                    if (greenLight != null)
                    {
                        greenLight.SetActive(true);
                    }

                    if (redLight != null)
                    {
                        redLight.SetActive(false);
                    }

                    PlaySound(swipeSuccessSound);
                }
                else
                {
                    Debug.Log("Swipe inválido. Porta ainda trancada!");

                    if (greenLight != null)
                    {
                        greenLight.SetActive(false);
                    }

                    if (redLight != null)
                    {
                        redLight.SetActive(true);
                    }

                    PlaySound(swipeFailSound);
                }
            }

            isCardInserted = false;
            insertedCard = null;
        }
    }

    private void PlaySound(AudioClip sound)
    {
        if (audioSource != null && sound != null)
        {
            audioSource.clip = sound;
            audioSource.Play();
        }
    }
}