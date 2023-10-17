using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TouchButton : XRBaseInteractable
{
    private Renderer buttonRenderer;
    private Color originalColor;
    public Color pressedColor = Color.green; // Cor a ser usada quando o bot�o for pressionado
    public int buttonNumber; // N�mero correspondente ao bot�o
    private NumberPad numberPad; // Refer�ncia ao script NumberPad
    private bool hasBeenPressed = false; // Vari�vel para rastrear se o bot�o j� foi pressionado por uma m�o

    
    private Vector3 originalPosition; // A posi��o original do bot�o
    private Vector3 pressedPosition; 
    private float maxPressDistance = -0.077f; // Dist�ncia m�xima que o bot�o pode ser pressionado no eixo Y


    private int pressSpeed = 2;
    private int returnSpeed = 2;

    protected override void Awake()
    {
        base.Awake();

        // Obt�m o componente Renderer do bot�o
        buttonRenderer = GetComponent<Renderer>();

        // Salva a cor original do bot�o
        originalColor = buttonRenderer.material.color;

        // Encontra o script NumberPad na cena 
        numberPad = FindObjectOfType<NumberPad>();

        // Salva a posi��o original do bot�o
        originalPosition = transform.position;

        // Calcula a posi��o pressionada com base na dist�ncia m�xima no eixo Y
        pressedPosition = originalPosition - new Vector3(0, 0, maxPressDistance);

    }
    private void Update()
    {
        if (hasBeenPressed)
        {
            // Mova o bot�o para a posi��o-alvo quando pressionado
            if (transform.position != pressedPosition)
            {
                float step = pressSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position,pressedPosition, step);
            }
        }
        else
        {
            // Mova o bot�o de volta para a posi��o original quando n�o estiver mais pressionado
            if (transform.position != originalPosition)
            {
                float step = returnSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, step);
            }
        }
    }


    // Este m�todo � chamado quando o controle entra na zona do bot�o
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        Debug.Log("hoverentered");
        base.OnHoverEntered(args);
     
    }

    // Este m�todo � chamado quando o controle sai da zona do bot�o
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        Debug.Log("hoverexited");

        base.OnHoverExited(args);

        
    }

    // Este m�todo � chamado quando o bot�o � pressionado
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("select");
        base.OnSelectEntered(args);

        // Marque o bot�o como pressionado quando uma m�o o selecionar
        hasBeenPressed = true;

        // Muda a cor do bot�o para a cor pressionada
        buttonRenderer.material.color = pressedColor;

        // Chama a fun��o do script NumberPad, passando o n�mero do bot�o como argumento
        if (numberPad != null)
        {
            numberPad.AddNumber(buttonNumber);
        }

    }
    // Este m�todo � chamado quando o bot�o � solto
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        Debug.Log("exited");

        base.OnSelectExited(args);

        // Marque o bot�o como n�o pressionado quando uma m�o o soltar
        hasBeenPressed = false;

        // Retorna a cor do bot�o � sua cor original
        buttonRenderer.material.color = originalColor;
    }
}