using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HistoriaIntro : MonoBehaviour
{
    [Header("Referencias")]
    public TMP_Text textoHistoria;
    public TMP_Text textoContinuar;

    [Header("Configuración")]
    public float velocidadEscritura = 0.03f;
    public string escenaSiguiente = "Nivel1";

    private string[] frases =
    {
        "Hace muchos años, el mundo vivía en paz.",

        "Los pueblos crecían, los bosques estaban llenos de vida y los reinos prosperaban sin grandes amenazas.",

        "Pero todo cambió el día en que ocurrió La Ruptura.",

        "Una enorme grieta se abrió en las profundidades de la tierra, liberando una energía oscura desconocida.",

        "Desde su interior comenzaron a surgir criaturas monstruosas que rápidamente se expandieron por todo el mundo, corrompiendo bosques, montañas y ciudades enteras.",

        "Los reinos intentaron defenderse...",

        "Pero fallaron.",

        "Uno tras otro cayeron ante el avance de la oscuridad.",

        "Con el paso de los años, gran parte del mundo quedó abandonada y en ruinas.",

        "Solo unos pocos asentamientos lograron sobrevivir.",

        "Lejos de aquel caos, escondida entre las montañas, existía una pequeña aldea donde vivía un joven llamado Eron.",

        "Desde niño siempre sintió curiosidad por lo desconocido.",

        "Mientras los demás aprendían a sobrevivir, él soñaba con explorar el mundo más allá de las montañas.",

        "Pero su vida cambió para siempre cuando La Ruptura alcanzó incluso los lugares más alejados.",

        "Su padre, un antiguo guerrero que conocía secretos sobre la energía oscura, decidió enfrentarse a la amenaza.",

        "Antes de partir, le entregó una espada envuelta en un tenue brillo violeta.",

        "Aquella arma parecía estar viva.",

        "Antes de marcharse, le dijo:",

        "\"Esta espada no es solo un arma... algún día entenderás por qué es tuya.\"",

        "Y desapareció.",

        "Eron nunca volvió a verlo.",

        "Pasaron los años.",

        "Los monstruos se volvieron más fuertes.",

        "Las sombras comenzaron a acercarse a la aldea.",

        "Y una noche, la oscuridad finalmente llegó.",

        "Las criaturas atacaron sin descanso.",

        "Las defensas cayeron.",

        "Las casas ardieron.",

        "Sin nadie más capaz de proteger su hogar, Eron tomó la espada de su padre.",

        "En el instante en que la sostuvo, la hoja despertó.",

        "Una energía violeta recorrió su cuerpo y reaccionó ante la presencia de la corrupción.",

        "Visiones extrañas aparecieron ante sus ojos.",

        "Vio la grieta.",

        "Vio a su padre.",

        "Y vio algo oculto en las profundidades de La Ruptura.",

        "Algo que parecía controlar toda la oscuridad.",

        "Fue entonces cuando comprendió que su destino no era quedarse en la aldea.",

        "Debía descubrir qué ocurrió realmente con su padre.",

        "Debía encontrar el origen de La Ruptura.",

        "Y debía detener la oscuridad antes de que consumiera el mundo por completo.",

        "Su viaje estaba a punto de comenzar..."
    };

    private int indiceActual = 0;
    private bool esperandoInput = false;

    void Start()
    {
        textoContinuar.gameObject.SetActive(false);
        StartCoroutine(EscribirFrase());
    }

    void Update()
    {
        if (esperandoInput && Input.GetKeyDown(KeyCode.Space))
        {
            indiceActual++;

            if (indiceActual >= frases.Length)
            {
                SceneManager.LoadScene(escenaSiguiente);
            }
            else
            {
                textoContinuar.gameObject.SetActive(false);
                StartCoroutine(EscribirFrase());
            }
        }
    }

    IEnumerator EscribirFrase()
    {
        esperandoInput = false;
        textoHistoria.text = "";

        foreach (char letra in frases[indiceActual])
        {
            textoHistoria.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        textoContinuar.gameObject.SetActive(true);
        esperandoInput = true;
    }
}