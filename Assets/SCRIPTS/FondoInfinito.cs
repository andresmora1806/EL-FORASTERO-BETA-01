using UnityEngine;

public class FondoInfinito : MonoBehaviour
{
    public Transform player;
    public float velocidad = 0.5f;
    public float velocidadX = 0.5f;
    public float velocidadY = 0.2f;

    private Transform[] fondos;
    private float anchoSprite;

    void Start()
    {
        fondos = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            fondos[i] = transform.GetChild(i);
        }

        SpriteRenderer sr = fondos[0].GetComponent<SpriteRenderer>();
        anchoSprite = sr.bounds.size.x;
    }
    
    void Update()
    {
        transform.position = new Vector3(
            player.position.x * velocidadX,
            player.position.y * velocidadY,
            transform.position.z
        );

        foreach (Transform fondo in fondos)
        {
            if (player.position.x - fondo.position.x > anchoSprite)
            {
                fondo.position += Vector3.right * anchoSprite * fondos.Length;
            }
        }
    }
}