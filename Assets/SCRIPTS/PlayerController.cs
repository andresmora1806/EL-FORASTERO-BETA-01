using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Sonidos")]
    public AudioSource audioSource;

    public AudioClip sonidoSalto;
    public AudioClip sonidoAtaque;
    public AudioClip sonidoDanio;

    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;
    public float fuerzaRebote = 6f;

    [Header("Checkpoint")]
    public Vector2 checkpointActual;
    private bool respawneando;

    [Header("Suelo")]
    public LayerMask capaSuelo;
    public Transform groundCheck;
    public float radioSuelo = 0.25f;

    [Header("Vida")]
    public int vidaMaxima = 5;
    public int vidaActual = 5;

    [Header("UI Corazones")]
    public Image[] corazones;
    public Sprite corazonLleno;
    public Sprite corazonVacio;

    [Header("Ataque")]
    public Transform attackPoint;
    public float attackRadius = 0.5f;
    public LayerMask enemyLayer;
    public int attackDamage = 1;

    [Header("Menú")]
    public PauseMenuManager pauseMenuManager;

    private bool enSuelo;
    private bool recibiendoDanio;
    private bool atacando;
    public bool muerto;

    private int coins;
    private bool invulnerable;
    private Rigidbody2D rb;
    public Animator animator;
    public TMP_Text textcoins;
    

    float movimiento;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        checkpointActual = transform.position;

        vidaActual = vidaMaxima;

        ActualizarCorazones();
    }

    void Update()
    {
        if (!muerto)
        {
            movimiento = Input.GetAxis("Horizontal");

            enSuelo = Physics2D.OverlapCircle(
                groundCheck.position,
                radioSuelo,
                capaSuelo
            );

            // SALTO
            if (enSuelo && Input.GetKeyDown(KeyCode.Space) && !recibiendoDanio)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

                rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
                audioSource.PlayOneShot(sonidoSalto);

            }

            // ATAQUE
            if (Input.GetMouseButtonDown(0) && !atacando && enSuelo)
            {
                atacando = true;

                animator.SetTrigger("atack");
                

                Invoke(nameof(DesactivaAtaque), 0.3f);
            }
        }

        // ANIMACIONES
        animator.SetBool("ensuelo", enSuelo);
        animator.SetBool("muerto", muerto);
        animator.SetFloat("velocidadY", rb.linearVelocity.y);
        animator.SetFloat("movement", Mathf.Abs(movimiento));
    }

    void FixedUpdate()
    {
        if (!recibiendoDanio && !atacando && !muerto)
        {
            rb.linearVelocity = new Vector2(
                movimiento * velocidad,
                rb.linearVelocity.y
            );

            if (movimiento < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (movimiento > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    // =========================
    // CHECKPOINT
    // =========================

    public void ActivarCheckpoint(Vector2 nuevaPosicion)
    {
        checkpointActual = nuevaPosicion;
    }

    public void Respawn()
    {
        if (respawneando)
            return;

        respawneando = true;

        transform.position = checkpointActual;

        rb.linearVelocity = Vector2.zero;

        StartCoroutine(RespawnCooldown());
    }

    IEnumerator RespawnCooldown()
    {
        yield return new WaitForSeconds(1f);

        respawneando = false;
    }

    // =========================
    // ATAQUE
    // =========================

    void DesactivaAtaque()
    {
        atacando = false;
    }

    // LLAMAR DESDE LA ANIMACIÓN
    public void AplicarDanio()
    {
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            enemyLayer
        );
        audioSource.PlayOneShot(sonidoAtaque);

        foreach (Collider2D enemigo in enemigos)
        {
            EnemyController enemy1 =
                enemigo.GetComponent<EnemyController>();

            Enemy2Controller enemy2 =
                enemigo.GetComponent<Enemy2Controller>();

            Enemy3Controller enemy3 =
                enemigo.GetComponent<Enemy3Controller>();

            Enemy4Controller enemy4 =
                enemigo.GetComponent<Enemy4Controller>();

            Enemy5Controller enemy5 =
                enemigo.GetComponent<Enemy5Controller>();

            


            // DAÑO ENEMY 1
            if (enemy1 != null)
            {
                enemy1.RecibeDanio(
                    transform.position,
                    attackDamage
                );
            }

            // DAÑO ENEMY 2
            if (enemy2 != null)
            {
                enemy2.RecibeDanio(
                    transform.position,
                    attackDamage
                );
            }

            // DAÑO ENEMY 3
            if (enemy3 != null)
            {
                enemy3.RecibeDanio(
                    transform.position,
                    attackDamage
                );
            }

            // DAÑO ENEMY 4
            if (enemy4 != null)
            {
                enemy4.RecibeDanio(
                    transform.position,
                    attackDamage
                );
            }


            // DAÑO ENEMY 5
            if (enemy5 != null)
            {
                enemy5.RecibeDanio(
                    transform.position,
                    attackDamage
                );
            }
        }
    }

    // =========================
    // DAÑO
    // =========================

    public void RecibeDanio(Vector2 posicionEnemigo)
    {
        if (recibiendoDanio || muerto)
            return;

        vidaActual--;

        if (vidaActual < 0)
            vidaActual = 0;

        ActualizarCorazones();

        recibiendoDanio = true;

        animator.SetTrigger("hit");
        audioSource.PlayOneShot(sonidoDanio);


        Debug.Log("Vida actual: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
        else
        {
            Vector2 direccion =
                (transform.position - (Vector3)posicionEnemigo).normalized;

            rb.linearVelocity = Vector2.zero;

            rb.AddForce(
                direccion * fuerzaRebote,
                ForceMode2D.Impulse
            );

            Invoke(nameof(ResetDanio), 0.4f);
        }
    }

    void ResetDanio()
    {
        recibiendoDanio = false;
    }

    // =========================
    // CURAR
    // =========================

    public void Curar(int cantidad)
    {
        if (muerto)
            return;

        vidaActual += cantidad;

        if (vidaActual > vidaMaxima)
            vidaActual = vidaMaxima;

        ActualizarCorazones();

        Debug.Log("Vida actual: " + vidaActual);
    }

    // =========================
    // UI CORAZONES
    // =========================

    void ActualizarCorazones()
    {
        if (corazones.Length == 0)
        {
            Debug.LogError("NO HAY CORAZONES EN EL ARRAY");
            return;
        }

        for (int i = 0; i < corazones.Length; i++)
        {
            if (corazones[i] == null)
                continue;

            if (i < vidaActual)
            {
                corazones[i].sprite = corazonLleno;
            }
            else
            {
                corazones[i].sprite = corazonVacio;
            }
        }
    }

    // =========================
    // MUERTE
    // =========================

    void Morir()
    {
        if (muerto)
            return;

        Debug.Log("EL JUGADOR MURIÓ");

        muerto = true;

        // DETENER TODO
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        // CONGELAR EL PERSONAJE
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

      
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            2f,
            capaSuelo
        );

        if (hit.collider != null)
        {
            transform.position = new Vector2(
                transform.position.x,
                hit.point.y + 0.05f
            );
        }

        animator.SetTrigger("dead");

        Invoke(nameof(MostrarGameOver), 1.2f);
    }

    void MostrarGameOver()
{
    rb.simulated = false;

    rb.constraints = RigidbodyConstraints2D.None;
    rb.freezeRotation = true;

    this.enabled = false;

    if (pauseMenuManager != null)
    {
        pauseMenuManager.MostrarGameOver();
    }
}

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // MONEDAS
        if (collision.CompareTag("coins"))
        {
            Destroy(collision.gameObject);

            coins++;

            textcoins.text = coins.ToString();
        }

        // SIGUIENTE NIVEL
        if (collision.CompareTag("sig"))
        {
            PauseMenuManager pause =
                FindFirstObjectByType<PauseMenuManager>();

            pause.CompletarNivel(3);
        }
        // SPIKES
        if (collision.CompareTag("spikes"))
        {
            if (muerto)
                return;

            vidaActual--;

            if (vidaActual < 0)
                vidaActual = 0;

            ActualizarCorazones();

            if (vidaActual <= 0)
            {
                Morir();
            }
            else
            {
                Respawn();
            }
        }

        // CURACIÓN
        if (collision.CompareTag("heart"))
        {
            Curar(1);

            Destroy(collision.gameObject);
        }
    }

  

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(
                groundCheck.position,
                radioSuelo
            );
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(
                attackPoint.position,
                attackRadius
            );
        }
    }
}