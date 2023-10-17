using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardReader : XRSocketInteractor
{
    public float minSwipeDistance = 0.005f;
    public float maxSwipeDistance = 0.25f; // Dist�ncia m�xima permitida para um swipe// Dist�ncia m�nima para um swipe ser registrado
    public float allowedUprightErrorRange = 0.2f; // Margem de erro permitida para a orienta��o vertical do cart�o

    private Vector3 initialCardPosition;
    private bool isCardInserted = false;
    private XRGrabInteractable insertedCard; // Refer�ncia para o cart�o inserido

    public GameObject doorLock; // Refer�ncia para o objeto da trava da porta

    public GameObject greenLight; // Refer�ncia para a luz verde
    public GameObject redLight;   // Refer�ncia para a luz vermelha

    public AudioClip swipeSuccessSound; // Som para swipe bem-sucedido
    public AudioClip swipeFailSound;    // Som para swipe inv�lido

    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();

        // Certifique-se de que as luzes est�o desligadas no in�cio
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
            // O cart�o j� foi inserido, voc� pode interagir com ele agora
            insertedCard = args.interactable as XRGrabInteractable;
        }
        else
        {
            // Insira o cart�o no leitor
            XRGrabInteractable card = args.interactable as XRGrabInteractable;

            if (card != null)
            {
                initialCardPosition = card.transform.position;
                isCardInserted = true;
                insertedCard = card; // Configure a inst�ncia de insertedCard


                // Adicione uma mensagem de depura��o para verificar a detec��o do cart�o
                Debug.Log("Cart�o detectado para inser��o.");
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
                // Calcule a dist�ncia entre a posi��o inicial e atual do cart�o
                float swipeDistance = Vector3.Distance(initialCardPosition, insertedCard.transform.position);
                Debug.Log("Dist�ncia do swipe: " + swipeDistance);

                // Verifique a orienta��o vertical do cart�o
                Vector3 cardUp = insertedCard.transform.forward;
                float dot = Vector3.Dot(cardUp, Vector3.up);
                Debug.Log("Orienta��o vertical do cart�o: " + dot);

                // Se a dist�ncia do swipe for maior ou igual a minSwipeDistance
                // e a orienta��o vertical do cart�o estiver dentro da margem de erro permitida
                if (swipeDistance >= minSwipeDistance && swipeDistance <= maxSwipeDistance &&
                    dot >= -1 - allowedUprightErrorRange || dot == -1)
                {
                    // Swipe bem-sucedido, fa�a o que for necess�rio, como abrir a porta
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
                    Debug.Log("Swipe inv�lido. Porta ainda trancada!");

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