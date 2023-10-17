using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorHandle : XRBaseInteractable
{
    public Transform door; // Referência para o objeto da porta
    public float maxDragDistance = 0.35f; // Distância máxima que a porta pode ser arrastada
    public Vector3 doorDirection = Vector3.right; // Direção em que a porta pode ser movida
    public int doorWeight = 20; // Peso da porta para controlar a velocidade

    private Vector3 initialDoorPosition;
    private Vector3 targetDoorPosition;
    private bool isMoving = false;

    public GameObject doorLockingBar; // Referência para o objeto da barra de travamento da porta


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (door == null)
        {
            Debug.LogError("A referência para o objeto da porta não foi configurada.");
            return;
        }
        if (doorLockingBar != null && doorLockingBar.activeSelf)
        {
            Debug.Log("A porta está trancada pela barra de travamento. Não é possível movê-la.");
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
            // Verifique se o objeto selecionado é um interator
            if (selectingInteractor is XRDirectInteractor directInteractor)
            {
                // Obtenha a posição do interator (mão)
                Vector3 interactorPosition = directInteractor.transform.position;

                // Calcule o vetor de arrasto da maçaneta para a mão
                Vector3 pullVector = interactorPosition - transform.position;

                // Use o produto escalar para determinar o movimento na direção correta
                float dotProduct = Vector3.Dot(pullVector.normalized, doorDirection.normalized);

                // Verifique se o movimento é na direção apropriada
                if (dotProduct > 0.0f)
                {
                    // Calcule a velocidade com base no movimento da mão
                    float speed = Mathf.Abs(dotProduct) / Time.deltaTime / doorWeight;

                    // Calcule a nova posição da porta
                    Vector3 newDoorPosition = Vector3.MoveTowards(door.position, targetDoorPosition, speed * Time.deltaTime);

                    // Defina a posição da porta
                    door.position = newDoorPosition;
                }
            }
        }
    }
}