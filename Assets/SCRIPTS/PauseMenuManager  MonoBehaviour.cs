using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Pausa")]
    public GameObject pauseMenu;
    public GameObject pauseButton;

    [Header("Game Over")]
    public GameObject panelGameOver;

    [Header("Victoria")]
    public GameObject panelVictoria;

    [Header("Estrellas")]
    public Image estrella1;
    public Image estrella2;
    public Image estrella3;

    public Sprite estrellaVacia;
    public Sprite estrellaLlena;

    [Header("Siguiente Nivel")]
    public string siguienteNivel;

    private bool terminado = false;

    void Start()
    {
        Time.timeScale = 1f;

        panelGameOver.SetActive(false);
        pauseMenu.SetActive(false);
        panelVictoria.SetActive(false);

        // Estrellas vacías al iniciar
        estrella1.sprite = estrellaVacia;
        estrella2.sprite = estrellaVacia;
        estrella3.sprite = estrellaVacia;
    }

    // 🔴 GAME OVER
    public void MostrarGameOver()
    {
        panelGameOver.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 🔄 REINICIAR
    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 🏠 IR AL MENÚ
    public void SalirAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MENU");
    }

    // ⏸ PAUSAR
    public void PauseGame()
    {
        Time.timeScale = 0f;

        if (pauseButton != null)
            pauseButton.SetActive(false);

        if (pauseMenu != null)
            pauseMenu.SetActive(true);
    }

    // ▶️ CONTINUAR
    public void ResumeGame()
    {
        Time.timeScale = 1f;

        if (pauseButton != null)
            pauseButton.SetActive(true);

        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    // 🔄 REINICIAR DESDE PAUSA
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 🎉 COMPLETAR NIVEL
    public void CompletarNivel(int cantidadEstrellas)
    {
        if (terminado) return;

        terminado = true;

        Time.timeScale = 0f;

        panelVictoria.SetActive(true);

        // Reiniciar estrellas
        estrella1.sprite = estrellaVacia;
        estrella2.sprite = estrellaVacia;
        estrella3.sprite = estrellaVacia;

        // Ejecutar animación
        StartCoroutine(AnimarEstrellas(cantidadEstrellas));
    }

    // ⭐ ANIMACIÓN DE ESTRELLAS
    IEnumerator AnimarEstrellas(int cantidad)
    {
        yield return new WaitForSecondsRealtime(0.5f);

        if (cantidad >= 1)
        {
            estrella1.sprite = estrellaLlena;
        }

        yield return new WaitForSecondsRealtime(0.5f);

        if (cantidad >= 2)
        {
            estrella2.sprite = estrellaLlena;
        }

        yield return new WaitForSecondsRealtime(0.5f);

        if (cantidad >= 3)
        {
            estrella3.sprite = estrellaLlena;
        }
    }

    // ➡️ SIGUIENTE NIVEL
    public void SiguienteNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(siguienteNivel);
    }

    // 🔄 REINTENTAR
    public void Reintentar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 🏠 MENÚ
    public void IrMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MENU");
    }

    // ❌ SALIR
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}