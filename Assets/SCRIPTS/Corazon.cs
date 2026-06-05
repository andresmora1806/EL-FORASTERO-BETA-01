using UnityEngine;

public class Corazon : MonoBehaviour
{
    public int curacion = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // BUSCAR EL SCRIPT PlayerController
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                // CURAR
                player.Curar(curacion);

                // DESTRUIR CORAZÓN
                Destroy(gameObject);
            }
        }
    }
}