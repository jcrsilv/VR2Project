using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorHandle : XRBaseInteractable
{
    public Transform door; // Refer�ncia para o objeto da porta
    public float maxDragDistance = 0.35f; // Dist�ncia m�xima que a porta pode ser arrastada
    public Vector3 doorDirection = Vector3.right; // Dire��o em que a porta pode ser movida
    public int doorWeight = 20; // Peso da porta para controlar a velocidade

    private Vector3 initialDoorPosition;
    private Vector3 targetDoorPosition;
    private bool isMoving = false;

    public GameObject doorLockingBar; // Refer�ncia para o objeto da barra de travamento da porta


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (door == null)
        {
            Debug.LogError("A refer�ncia para o objeto da porta n�o foi configurada.");
            return;
        }
        if (doorLockingBar != null && doorLockingBar.activeSelf)
        {
            Debug.Log("A porta est� trancada pela barra de travamento. N�o � poss�vel mov�-la.");
            return;
        }

        initialDoorPosition = door.position;
        targetDoorPosition = initialDoorPosition + (doorDirection.normalized * maxDragDistance);

        isMoving = true;

        Debug.Log("Handle selecionada. Iniciando movimento da porta.");
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        isMoving = false;

        Debug.Log("Handle desselecionada. Parando movimento da porta.");
    }

    private void Update()
    {
        if (isMoving)
        {
            // Verifique se o objeto selecionado � um interator
            if (selectingInteractor is XRDirectInteractor directInteractor)
            {
                // Obtenha a posi��o do interator (m�o)
                Vector3 interactorPosition = directInteractor.transform.position;

                // Calcule o vetor de arrasto da ma�aneta para a m�o
                Vector3 pullVector = interactorPosition - transform.position;

                // Use o produto escalar para determinar o movimento na dire��o correta
                float dotProduct = Vector3.Dot(pullVector.normalized, doorDirection.normalized);

                // Verifique se o movimento � na dire��o apropriada
                if (dotProduct > 0.0f)
                {
                    // Calcule a velocidade com base no movimento da m�o
                    float speed = Mathf.Abs(dotProduct) / Time.deltaTime / doorWeight;

                    // Calcule a nova posi��o da porta
                    Vector3 newDoorPosition = Vector3.MoveTowards(door.position, targetDoorPosition, speed * Time.deltaTime);

                    // Defina a posi��o da porta
                    door.position = newDoorPosition;
                }
            }
        }
    }
}