using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
    public CanvasGroup logo;
    public float tiempoAparecer = 2f;
    public float tiempoVisible = 2f;
    public float tiempoDesaparecer = 2f;

    public string escenaMenu = "Menu";

    void Start()
    {
        StartCoroutine(MostrarLogo());
    }

    IEnumerator MostrarLogo()
    {
        // Empieza invisible
        logo.alpha = 0;

        // Aparecer lentamente
        float t = 0;
        while (t < tiempoAparecer)
        {
            t += Time.deltaTime;
            logo.alpha = Mathf.Lerp(0, 1, t / tiempoAparecer);
            yield return null;
        }

        // Esperar visible
        yield return new WaitForSeconds(tiempoVisible);

        // Desaparecer lentamente
        t = 0;
        while (t < tiempoDesaparecer)
        {
            t += Time.deltaTime;
            logo.alpha = Mathf.Lerp(1, 0, t / tiempoDesaparecer);
            yield return null;
        }

        // Cargar menú
        SceneManager.LoadScene(escenaMenu);
    }
}
