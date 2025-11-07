using UnityEngine;

public class PlataformaMovimiento : MonoBehaviour
{
    [SerializeField] GameObject punto1;
    [SerializeField] GameObject punto2;

    GameObject puntoReferencia;
    float velocidad = 5f;

    private void Start()
    {
        puntoReferencia = punto1;
    }

    private void Update()
    {
        if (Vector2.Distance(puntoReferencia.transform.position, transform.position) < 0.1f)
        {
            if (puntoReferencia == punto1) puntoReferencia = punto2;
            else puntoReferencia = punto1;
        }

        transform.position = Vector2.MoveTowards(transform.position, puntoReferencia.transform.position, velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Jugador")
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Jugador")
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
