using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TouchButton : XRBaseInteractable
{
    private Renderer buttonRenderer;
    private Color originalColor;
    public Color pressedColor = Color.green; // Cor a ser usada quando o botão for pressionado
    public int buttonNumber; // Número correspondente ao botão
    private NumberPad numberPad; // Referência ao script NumberPad
    private bool hasBeenPressed = false; // Variável para rastrear se o botão já foi pressionado por uma mão

    
    private Vector3 originalPosition; // A posição original do botão
    private Vector3 pressedPosition; 
    private float maxPressDistance = -0.077f; // Distância máxima que o botão pode ser pressionado no eixo Y


    private int pressSpeed = 2;
    private int returnSpeed = 2;

    protected override void Awake()
    {
        base.Awake();

        // Obtém o componente Renderer do botão
        buttonRenderer = GetComponent<Renderer>();

        // Salva a cor original do botão
        originalColor = buttonRenderer.material.color;

        // Encontra o script NumberPad na cena 
        numberPad = FindObjectOfType<NumberPad>();

        // Salva a posição original do botão
        originalPosition = transform.position;

        // Calcula a posição pressionada com base na distância máxima no eixo Y
        pressedPosition = originalPosition - new Vector3(0, 0, maxPressDistance);

    }
    private void Update()
    {
        if (hasBeenPressed)
        {
            // Mova o botão para a posição-alvo quando pressionado
            if (transform.position != pressedPosition)
            {
                float step = pressSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position,pressedPosition, step);
            }
        }
        else
        {
            // Mova o botão de volta para a posição original quando não estiver mais pressionado
            if (transform.position != originalPosition)
            {
                float step = returnSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, step);
            }
        }
    }


    // Este método é chamado quando o controle entra na zona do botão
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        Debug.Log("hoverentered");
        base.OnHoverEntered(args);
     
    }

    // Este método é chamado quando o controle sai da zona do botão
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        Debug.Log("hoverexited");

        base.OnHoverExited(args);

        
    }

    // Este método é chamado quando o botão é pressionado
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("select");
        base.OnSelectEntered(args);

        // Marque o botão como pressionado quando uma mão o selecionar
        hasBeenPressed = true;

        // Muda a cor do botão para a cor pressionada
        buttonRenderer.material.color = pressedColor;

        // Chama a função do script NumberPad, passando o número do botão como argumento
        if (numberPad != null)
        {
            numberPad.AddNumber(buttonNumber);
        }

    }
    // Este método é chamado quando o botão é solto
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        Debug.Log("exited");

        base.OnSelectExited(args);

        // Marque o botão como não pressionado quando uma mão o soltar
        hasBeenPressed = false;

        // Retorna a cor do botão à sua cor original
        buttonRenderer.material.color = originalColor;
    }
}