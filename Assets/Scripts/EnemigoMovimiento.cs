using UnityEngine;

public class EnemigoMovimiento : MonoBehaviour
{
    [SerializeField] GameObject punto1;
    [SerializeField] GameObject punto2;

    GameObject puntoReferencia;
    Collider2D[] todosLosColliders;
    Rigidbody2D rigidbody2D;
    SpriteRenderer sprite;
    Animator animator;

    float velocidad = 5f;
    float fuerzaReboteJugador = 15f;

    private void Start()
    {
        puntoReferencia = punto1;

        todosLosColliders = GetComponents<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        moverse();
    }

    void moverse()
    {
        if (Vector2.Distance(puntoReferencia.transform.position, transform.position) < 0.1f)
        {
            if (puntoReferencia == punto1) puntoReferencia = punto2;
            else puntoReferencia = punto1;
        }

        float direccionHorizontal = puntoReferencia.transform.position.x - transform.position.x;

        transform.position = Vector2.MoveTowards(transform.position, puntoReferencia.transform.position, velocidad * Time.deltaTime);

        if (direccionHorizontal > 0.01f)
        {
            sprite.flipX = false;
        }
        else if (direccionHorizontal < -0.01f)
        {
            sprite.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Jugador")
        {
            Rigidbody2D rbJugador = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rbJugador != null)
            {
                // Hacemos que de un pequeño salto al caer sobre el enemigo.
                rbJugador.linearVelocity = new Vector2(rbJugador.linearVelocity.x, fuerzaReboteJugador);
            }

            animator.Play("EnemigoMuerte");

            foreach (Collider2D collider in todosLosColliders)
            {
                collider.enabled = false;
            }

            if (rigidbody2D != null)
            {
                rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                rigidbody2D.gravityScale = 1f;
                rigidbody2D.linearVelocity = new Vector2(0, 2f);
            }

            this.enabled = false;

            Destroy(this.gameObject, 2f);
        }
    }
}
