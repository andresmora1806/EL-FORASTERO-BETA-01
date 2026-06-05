using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CreditosFinales : MonoBehaviour
{
    public TextMeshProUGUI textoCreditos;
    public float velocidad = 50f;
    public float posicionFinalY = 1200f;
    public float tiempoAntesMenu = 3f;

    private bool termino = false;

    void Start()
    {
        textoCreditos.text =
@"GATON TEAM

Presenta

El Forastero


Desarrollador
Heiner Andrés Suárez Mora


Programación
Heiner Andrés Suárez Mora


Diseño de Niveles
Heiner Andrés Suárez Mora


Arte

Sprites utilizados bajo licencia de uso libre para proyectos no comerciales.

Créditos de los recursos gráficos:
Szadi Art


Agradecimientos

A todos los jugadores que probaron y apoyaron este proyecto.


Gracias por jugar


FIN";
    }

    void Update()
    {
        if (!termino)
        {
            textoCreditos.rectTransform.anchoredPosition +=
                Vector2.up * velocidad * Time.deltaTime;

            if (textoCreditos.rectTransform.anchoredPosition.y >= posicionFinalY)
            {
                termino = true;
                Invoke(nameof(IrAlMenu), tiempoAntesMenu);
            }
        }
    }

    void IrAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}