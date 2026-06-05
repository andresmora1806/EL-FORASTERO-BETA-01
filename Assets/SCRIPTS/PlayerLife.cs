using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 5;
    public int vidaActual = 5;

    [Header("UI Corazones")]
    public Image[] corazones;
    public Sprite corazonLleno;
    public Sprite corazonVacio;

    [Header("Muerte")]
    public Animator animator;
    private bool muerto = false;

    void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarCorazones();
    }

    void Update()
    {
        // TEST DAÑO
        if (Input.GetKeyDown(KeyCode.K))
        {
            RecibirDanio(1);
        }

        // TEST CURACIÓN
        if (Input.GetKeyDown(KeyCode.L))
        {
            Curar(1);
        }
    }

    // =========================
    // RECIBIR DAÑO
    // =========================
    public void RecibirDanio(int cantidad)
    {
        if (muerto)
            return;

        vidaActual -= cantidad;

        if (vidaActual < 0)
            vidaActual = 0;

        ActualizarCorazones();

        Debug.Log("Vida actual: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
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
    // ACTUALIZAR UI
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
        muerto = true;

        Debug.Log("EL JUGADOR MURIÓ");

        if (animator != null)
        {
            animator.SetTrigger("death");
        }

        // Desactivar movimiento si quieres:
        // GetComponent<PlayerController>().enabled = false;
    }
}