using TMPro;
using UnityEngine;

public class PlayerDiamonds : MonoBehaviour
{
    public int diamantes;

    public TextMeshProUGUI textoDiamantes;

    void Start()
    {
        ActualizarUI();
    }

    public void SumarDiamantes(int cantidad)
    {
        diamantes += cantidad;

        ActualizarUI();
    }

    void ActualizarUI()
    {
        if (textoDiamantes != null)
        {
            textoDiamantes.text = diamantes.ToString();
        }
    }
}