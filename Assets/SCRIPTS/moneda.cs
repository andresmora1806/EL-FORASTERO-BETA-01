using UnityEngine;

public class moneda : MonoBehaviour
{
    public int valor = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDiamonds player = other.GetComponent<PlayerDiamonds>();

            if (player != null)
            {
                player.SumarDiamantes(valor);
            }

            Destroy(gameObject);
        }
    }
}