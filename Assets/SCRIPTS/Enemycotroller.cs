using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Jugador")]
    public Transform player;

    [Header("Patrullaje")]
    public Transform puntoA;
    public Transform puntoB;

    private Transform objetivoPatrulla;

    [Header("Movimiento")]
    public float detectionRadius = 5.0f;
    public float attackRadius = 1.0f;
    public float speed = 2.0f;

    [Header("Vida")]
    public int vida = 3;

    [Header("Daño")]
    public int dano = 1;
    public float fuerzaRebote = 7f;

    [Header("Ataque")]
    public float tiempoEntreAtaques = 1f;

    [Header("Drop Corazon")]
    public GameObject prefabCorazon;

    [Range(0, 100)]
    public int probabilidadDrop = 100;

    [Header("Drop Diamante")]
    public GameObject prefabDiamante;

    [Range(0, 100)]
    public int probabilidadDiamante = 100;

    private Rigidbody2D rb;
    private Vector2 movement;

    private bool enMovimiento;
    private bool muerto;
    private bool recibiendoDanio;
    private bool playerVivo;
    private bool atacando;

    private float tiempoAtaque;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        objetivoPatrulla = puntoB;

        if (player != null)
            playerVivo = true;
    }

    void Update()
    {
        if (muerto) return;

        if (player != null && playerVivo)
        {
            float distancia = Vector2.Distance(transform.position, player.position);

            if (distancia <= detectionRadius)
            {
                MovimientoJugador();
                Ataque();
            }
            else
            {
                Patrullar();
            }
        }
        else
        {
            Patrullar();
        }

        animator.SetBool("enMovimiento", enMovimiento);
        animator.SetBool("atacando", atacando);
    }

    void FixedUpdate()
    {
        if (!recibiendoDanio && !muerto && !atacando)
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }

    void Patrullar()
    {
        if (puntoA == null || puntoB == null) return;

        Vector2 direccion = (objetivoPatrulla.position - transform.position).normalized;

        movement = new Vector2(direccion.x, 0);

        enMovimiento = true;

        // Girar enemigo
        if (direccion.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (direccion.x > 0)
            transform.localScale = new Vector3(1, 1, 1);

        // Cambiar objetivo
        if (Vector2.Distance(transform.position, objetivoPatrulla.position) < 0.2f)
        {
            if (objetivoPatrulla == puntoA)
                objetivoPatrulla = puntoB;
            else
                objetivoPatrulla = puntoA;
        }
    }

    void MovimientoJugador()
    {
        if (player == null) return;

        PlayerController playerScript = player.GetComponent<PlayerController>();

        if (playerScript != null && playerScript.muerto)
        {
            playerVivo = false;

            movement = Vector2.zero;
            enMovimiento = false;
            atacando = false;

            animator.SetBool("atacando", false);

            return;
        }

        float distancia = Vector2.Distance(transform.position, player.position);

        if (distancia < detectionRadius && distancia > attackRadius)
        {
            Vector2 direccion = (player.position - transform.position).normalized;

            if (direccion.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (direccion.x > 0)
                transform.localScale = new Vector3(1, 1, 1);

            movement = new Vector2(direccion.x, 0);

            enMovimiento = true;
        }
        else
        {
            movement = Vector2.zero;
            enMovimiento = false;
        }
    }

    void Ataque()
    {
        if (player == null || atacando || !playerVivo) return;

        float distancia = Vector2.Distance(transform.position, player.position);

        tiempoAtaque += Time.deltaTime;

        if (distancia <= attackRadius && tiempoAtaque >= tiempoEntreAtaques)
        {
            tiempoAtaque = 0;

            StartCoroutine(AnimacionAtaque());
        }
    }

    IEnumerator AnimacionAtaque()
    {
        atacando = true;

        movement = Vector2.zero;

        animator.SetTrigger("attack");

        yield return new WaitForSeconds(0.3f);

        if (!playerVivo)
        {
            atacando = false;
            yield break;
        }

        if (player != null)
        {
            float distancia = Vector2.Distance(transform.position, player.position);

            if (distancia <= attackRadius)
            {
                PlayerController playerScript = player.GetComponent<PlayerController>();
                PlayerLife vidaJugador = player.GetComponent<PlayerLife>();

                if (vidaJugador != null)
                {
                    vidaJugador.RecibirDanio(dano);
                }

                if (playerScript != null)
                {
                    Vector2 direccionDanio = transform.position;

                    playerScript.RecibeDanio(direccionDanio);

                    playerVivo = !playerScript.muerto;

                    if (!playerVivo)
                    {
                        enMovimiento = false;
                        movement = Vector2.zero;
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.3f);

        atacando = false;
    }

    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (recibiendoDanio || muerto) return;

        vida -= cantDanio;

        recibiendoDanio = true;

        animator.SetTrigger("hit");

        if (vida <= 0)
        {
            muerto = true;

            enMovimiento = false;
            atacando = false;

            movement = Vector2.zero;

            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;

            int random = Random.Range(0, 100);

            // DROP CORAZONES
            if (random < probabilidadDrop)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 offset = new Vector2(
                        Random.Range(-0.5f, 0.5f),
                        Random.Range(0f, 0.5f)
                    );

                    GameObject corazon = Instantiate(
                        prefabCorazon,
                        (Vector2)transform.position + offset,
                        Quaternion.identity
                    );

                    Rigidbody2D rbCorazon = corazon.GetComponent<Rigidbody2D>();

                    if (rbCorazon != null)
                    {
                        float fuerzaX = Random.Range(-4f, 4f);
                        float fuerzaY = Random.Range(3f, 6f);

                        rbCorazon.AddForce(
                            new Vector2(fuerzaX, fuerzaY),
                            ForceMode2D.Impulse
                        );
                    }
                }
            }

            // DROP DIAMANTES
            if (random < probabilidadDiamante)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 offset = new Vector2(
                        Random.Range(-1.2f, 1.2f),
                        Random.Range(0f, 0.5f)
                    );

                    GameObject diamante = Instantiate(
                        prefabDiamante,
                        (Vector2)transform.position + offset,
                        Quaternion.identity
                    );

                    Rigidbody2D rbDiamante = diamante.GetComponent<Rigidbody2D>();

                    if (rbDiamante != null)
                    {
                        float fuerzaX = Random.Range(-3f, 3f);
                        float fuerzaY = Random.Range(3f, 5f);

                        rbDiamante.AddForce(
                            new Vector2(fuerzaX, fuerzaY),
                            ForceMode2D.Impulse
                        );
                    }
                }
            }

            Destroy(gameObject);

            return;
        }
        else
        {
            Vector2 rebote = (transform.position - (Vector3)direccion).normalized;

            rb.linearVelocity = Vector2.zero;

            rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);

            StartCoroutine(DesactivaDanio());
        }
    }

    IEnumerator DesactivaDanio()
    {
        yield return new WaitForSeconds(0.4f);

        recibiendoDanio = false;

        rb.linearVelocity = Vector2.zero;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}