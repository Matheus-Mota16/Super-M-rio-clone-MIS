// Importa bibliotecas fundamentais da Unity e do sistema do C#
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Define a classe do script. Herdar de MonoBehaviour permite que este script 
// seja anexado a um objeto (GameObject) dentro da Unity.
public class playerMovement : MonoBehaviour
{
    // ==========================================
    // VARIÁVEIS PÚBLICAS (Visíveis no Inspector)
    // ==========================================
    public SpriteRenderer sprite; // Controla a imagem do jogador (usado aqui para virar o personagem).
    public Rigidbody2D rig;       // Controla a física do jogador (gravidade, velocidade, colisões).
    public Animator anim;         // Controla as animações do jogador.
    public float speed;           // Define a velocidade de caminhada/corrida.
    public float jumpForce;       // Define a força/altura do pulo.

    // ==========================================
    // VARIÁVEIS SERIALIZADAS (Privadas, mas visíveis no Inspector da Unity)
    // ==========================================
    [SerializeField] bool isJump;                   // Indica se o jogador está no ar (Nota: não está sendo usada neste script atual).
    [SerializeField] bool inFloor = true;           // Indica se o jogador está encostando no chão.
    [SerializeField] LayerMask groundLayer;         // Define quais camadas (Layers) do jogo são consideradas "chão".
    [SerializeField] Transform groundCheck;         // Um ponto vazio (GameObject) colocado nos pés do personagem para checar o chão.
    [SerializeField] float groundCheckRadius = 0.1f;// O tamanho do círculo de colisão gerado nos pés para detectar o chão.

    // ==========================================
    // VARIÁVEIS PRIVADAS
    // ==========================================
    private Vector2 direction; // Guarda a direção do movimento (Eixo X e Y) que será aplicada ao corpo físico.

    // O método Update roda uma vez por frame. 
    // É o melhor lugar para capturar as teclas que o jogador pressiona.
    void Update()
    {
        // Pega a velocidade vertical (Y) atual do personagem. 
        // Se 'rig' for nulo, assume 0 para evitar erros.
        float vy = rig != null ? rig.linearVelocity.y : 0f;

        // Define a nova direção. O eixo X recebe o comando do jogador (-1, 0 ou 1) multiplicado pela velocidade.
        // O eixo Y mantém a velocidade vertical atual (para não atrapalhar a gravidade/pulo).
        direction = new Vector2(Input.GetAxisRaw("Horizontal") * speed, vy);

        // Se o jogador pressionar para a esquerda (valor negativo)...
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            sprite.flipX = true; // Vira o sprite do personagem para a esquerda.
        }

        // Se o jogador pressionar para a direita (valor positivo)...
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            sprite.flipX = false; // Deixa o sprite na sua posição original (virado para a direita).
        }

        // Atualiza a verificação do chão.
        if (groundCheck != null)
        {
            // Cria um círculo invisível nos pés do jogador. Se encostar na camada 'groundLayer', 'inFloor' vira verdadeiro.
            inFloor = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
        }

        // Sistema de pulo: Se apertar o botão de Pulo (Geralmente a barra de Espaço) E estiver no chão...
        if (Input.GetButtonDown("Jump") && inFloor)
        {
            if (rig != null)
            {
                // Aplica uma força instantânea (Impulse) para cima, fazendo o personagem pular.
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    // O método FixedUpdate roda em intervalos de tempo fixos. 
    // É o local correto para aplicar forças e alterar propriedades de física.
    private void FixedUpdate()
    {
        if (rig != null)
            rig.linearVelocity = direction;

        // Primeiro, garantimos que o personagem está tocando o chão
        if (inFloor)
        {
            // Usamos Mathf.Abs para ignorar o sinal negativo e verificar apenas se há velocidade no eixo X
            if (Mathf.Abs(direction.x) > 0)
            {
                anim.Play("playerRun");
            }
            else
            {
                anim.Play("playerIdle");
            }
        }
        else
        {
            // Se inFloor for falso, o personagem está no ar (pulando ou caindo).
            // Se você tiver animações de pulo, é aqui que elas entram!
            // Exemplo: anim.Play("playerJump");
        }
    }

    // O método Awake é chamado assim que o script é inicializado, antes mesmo do Start().
    private void Awake()
    {
        // Se a referência da física (rig) estiver vazia no Inspector...
        if (rig == null)
        {
            // Procura automaticamente o componente Rigidbody2D que está no mesmo objeto e o atribui.
            rig = GetComponent<Rigidbody2D>();
        }
    }
}
