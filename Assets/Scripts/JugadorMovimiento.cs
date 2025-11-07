using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class JugadorMovimiento : MonoBehaviour
{
    InputAction actionMoverse;
    InputAction actionSaltar;
    Rigidbody2D rigidbody2D;
    CapsuleCollider2D collider2D;

    // Variables serializadas (parámetros visibles en Unity).
    [SerializeField] LayerMask layerSuelo;
    [Header("Parámetros iniciales")]
    [SerializeField] float velocidad = 7f;
    [SerializeField] float fuerzaSalto = 22f;
    [SerializeField] int maximosSaltos = 2;

    // Comprobación de suelo.
    [Header("Comprobación de Suelo")]
    [SerializeField] Transform tocaSueloPunto;
    [SerializeField] float tocaSueloRadio = 0.2f; // El radio del círculo de detección

    // UI vidas
    [Header("UI Vidas")]
    [SerializeField] Image[] corazonesUI;

    [Header("Parámetros de Boosts")]
    [SerializeField] float velocidadBoost = 14f; // El doble de velocidad
    [SerializeField] float fuerzaSaltoBoost = 30f; // Salto más fuerte
    [SerializeField] float duracionBoost = 5f; // 5 segundos

    // Variables privadas para guardar los valores originales
    private float velocidadOriginal;
    private float fuerzaSaltoOriginal;

    // Referencias a las Coroutines (para poder pararlas si coges otra)
    private Coroutine boostVelocidadActivo;
    private Coroutine boostSaltoActivo;

    int saltosRestantes;
    int maximoVidas = 3;
    int vidasRestantes;

    // Propiedades públicas.
    public int SaltosRestantes { get; private set; }
    public int VidasRestantes { get; private set; }
    public float InputHorizontal { get; private set; }
    public float VelocidadVertical { get; private set; }
    public bool AcabaDeRecibirDano { get; private set; }

    // Propiedades públicas estáticas.
    public static string NivelActualNombre;
    public static int NivelActualIndex;

    // Inicializamos y asignamos las variables arriba declaradas.
    void Start()
    {
        // Inputs.
        actionMoverse = InputSystem.actions.FindAction("Move");
        actionSaltar = InputSystem.actions.FindAction("Jump");

        // Activamos los inputs.
        actionMoverse.Enable();
        actionSaltar.Enable();

        // Componentes GameObject.
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<CapsuleCollider2D>();

        // Inicializamos variables.
        saltosRestantes = maximosSaltos;
        vidasRestantes = maximoVidas;
        VidasRestantes = vidasRestantes;
        AcabaDeRecibirDano = false;
        velocidadOriginal = velocidad;
        fuerzaSaltoOriginal = fuerzaSalto;

        // Si el punto de comprobación de suelo no está asignado, mostramos un error.
        if (tocaSueloPunto == null)
        {
            Debug.LogError("El 'Ground Check Point' falta en el Inspector!");
        }

        // Guardamos el nombre del nivel actual.
        NivelActualNombre = SceneManager.GetActiveScene().name;
        Debug.Log("Nivel Actual: " + NivelActualNombre);
        NivelActualIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Nivel Actual: " + NivelActualIndex);

        ActualizarUICorazones();
    }

    void Update()
    {
        Moverse();

        // Si tocamos el suelo, reseteamos los saltos.
        if (estaEnElSuelo() && rigidbody2D.linearVelocity.y <= 0.1f)
        {
            saltosRestantes = maximosSaltos;
            //Debug.Log("El jugador ha tocado el suelo");
        }
    }

    public bool estaEnElSuelo()
    {
        // Arreglo para evitar que reseteara los saltos al tocar paredes o techos.
        if (tocaSueloPunto == null) return false;

        return Physics2D.OverlapCircle(
            tocaSueloPunto.position,
            tocaSueloRadio,
            layerSuelo
            );
        /*
        return Physics2D.BoxCast(
        collider2D.bounds.center,
        collider2D.bounds.size,
        0f,
        Vector2.down,
        .1f,
        layerSuelo
        );
        */
    }

    public void RecibirDano(int danho)
    {
        if (vidasRestantes >= 0)
        {
            vidasRestantes -= danho;
            VidasRestantes = vidasRestantes;
            AcabaDeRecibirDano = true;

            ActualizarUICorazones();
        }
    }

    public void ResetearDanho()
    {
        AcabaDeRecibirDano = false;
    }

    void ActualizarUICorazones()
    {
        // Recorremos todos los índices de corazones en la UI.
        for (int i = 0; i < corazonesUI.Length; i++)
        {
            if (i < vidasRestantes)
            {
                // Activamos la imagen del corazón.
                corazonesUI[i].enabled = true;
            }
            else
            {
                // Si no, la desactivamos.
                corazonesUI[i].enabled = false;
            }
        }
    }

    public void GanarVida(int cantidad)
    {
        // Solo ganamos vida si no estamos ya al máximo.
        if (vidasRestantes < maximoVidas)
        {
            vidasRestantes += cantidad;
            VidasRestantes = vidasRestantes;
            ActualizarUICorazones();
        }
    }


    public void ActivarBoostVelocidad()
    {
        // Si ya teníamos un boost de velocidad, lo paramos.
        if (boostVelocidadActivo != null)
        {
            StopCoroutine(boostVelocidadActivo);
        }

        // Iniciamos la nueva coroutine y guardamos la referencia.
        boostVelocidadActivo = StartCoroutine(BoostVelocidadCoroutine());
    }

    public void ActivarBoostSalto()
    {
        // Si ya teníamos un boost de velocidad, lo paramos.
        if (boostSaltoActivo != null)
        {
            StopCoroutine(boostSaltoActivo);
        }

        // Iniciamos la nueva coroutine y guardamos la referencia.
        boostSaltoActivo = StartCoroutine(BoostSaltoCoroutine());
    }

    private IEnumerator BoostVelocidadCoroutine()
    {
        Debug.Log("¡Boost de velocidad ACTIVADO!");
        velocidad = velocidadBoost; // Aplicamos el boost.

        yield return new WaitForSeconds(duracionBoost); // Esperamos 2 segundos.

        Debug.Log("Boost de velocidad DESACTIVADO.");
        velocidad = velocidadOriginal; // Volvemos a la normalidad.
        boostVelocidadActivo = null; // Limpiamos la referencia.
    }

    private IEnumerator BoostSaltoCoroutine()
    {
        Debug.Log("¡Boost de salto ACTIVADO!");
        fuerzaSalto = fuerzaSaltoBoost; // Aplicamos el boost.

        yield return new WaitForSeconds(duracionBoost); // Esperamos 2 segundos.

        Debug.Log("Boost de salto DESACTIVADO.");
        fuerzaSalto = fuerzaSaltoOriginal; // Volvemos a la normalidad
        boostSaltoActivo = null; // Limpiamos la referencia.
    }

    void Moverse()
    {
        // Leemos el input y lo guardamos en la propiedad pública
        InputHorizontal = actionMoverse.ReadValue<Vector2>().x;
        // Guardamos la velocidad vertical
        VelocidadVertical = rigidbody2D.linearVelocity.y;
        // Guardamos los saltos restantes
        SaltosRestantes = saltosRestantes;

        // Movimiento izquierda y derecha.
        Vector2 movimiento = actionMoverse.ReadValue<Vector2>();
        rigidbody2D.linearVelocityX = movimiento.x * velocidad;

        // Salto.
        if (actionSaltar.WasPressedThisFrame())
        {
            if (saltosRestantes > 0)
            {
                saltosRestantes--; // Gastamos un salto

                // Aplicamos la velocidad de salto
                rigidbody2D.linearVelocityY = fuerzaSalto;
            }
        }
    }
}