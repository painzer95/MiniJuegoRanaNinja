using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.U2D;

public class Sonidos : MonoBehaviour
{
    JugadorMovimiento jugadorMovimiento;
    AudioSource audioSource;

    // Sonidos.
    [Header("Sonidos Base")]
    [SerializeField] AudioClip sonidoFondo;
    [SerializeField] AudioClip sonidoClick;
    [Header("Sonidos Animaciones")]
    [SerializeField] AudioClip sonidoCaminar;
    [SerializeField] AudioClip sonidoSaltar;
    [SerializeField] AudioClip sonidoSaltarDoble;
    [SerializeField] AudioClip sonidoCaer;
    [SerializeField] AudioClip sonidoDanho;
    [SerializeField] AudioClip sonidoMorir;
    [SerializeField] AudioClip sonidoRecompensa;
    [Header("Sonidos Finalizado")]
    [SerializeField] AudioClip sonidoNivelCompletado;

    private bool estaMuerto = false;
    private bool estaCayendo = false;
    private bool haSaltado_1 = false;
    private bool haSaltado_2 = false;

    void Start()
    {
        jugadorMovimiento = GetComponent<JugadorMovimiento>();

        // Audio.
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (jugadorMovimiento == null || !jugadorMovimiento.enabled) return;

        sonidos();
    }

    private void sonidos()
    {
        bool estaEnElSuelo = jugadorMovimiento.estaEnElSuelo();
        float movimiento = jugadorMovimiento.InputHorizontal;
        float velocidadVertical = jugadorMovimiento.VelocidadVertical;
        int saltosRestantes = jugadorMovimiento.SaltosRestantes;
        int vidasRestantes = jugadorMovimiento.VidasRestantes;

        if (vidasRestantes <= 0 && jugadorMovimiento.AcabaDeRecibirDano)
        {
            if (!estaMuerto)
            {
                PararSonidoCaminar();
                audioSource.PlayOneShot(sonidoMorir);
                estaMuerto = true;
                return;
            }
        }
        else if (vidasRestantes > 0 && jugadorMovimiento.AcabaDeRecibirDano)
        {
            audioSource.PlayOneShot(sonidoDanho);
        }

        if (estaMuerto || jugadorMovimiento.AcabaDeRecibirDano)
        {
            PararSonidoCaminar();
            return;
        }

        if (!estaMuerto && 
            !jugadorMovimiento.AcabaDeRecibirDano)
        {
            if (!estaEnElSuelo)
            {
                PararSonidoCaminar();

                if (velocidadVertical < -0.1f)
                {
                    if (!estaCayendo)
                    {
                        estaCayendo = true;
                    }
                }
                else if (velocidadVertical > 0.1f)
                {
                    estaCayendo = false;

                    if (saltosRestantes == 1 && !haSaltado_1)
                    {
                        audioSource.PlayOneShot(sonidoSaltar);
                        haSaltado_1 = true;
                    }
                    else if (saltosRestantes == 0 && !haSaltado_2)
                    {
                        audioSource.PlayOneShot(sonidoSaltarDoble);
                        haSaltado_2 = true;
                    }
                }
            }
            else
            {
                if (estaCayendo)
                {
                    audioSource.PlayOneShot(sonidoCaer);
                    estaCayendo = false;
                }

                haSaltado_1 = false;
                haSaltado_2 = false;

                if (movimiento > 0 || movimiento < 0)
                {
                    ReproducirSonidoCaminar();
                }
                else
                {
                    PararSonidoCaminar();
                }
            }
        }
    }

    void ReproducirSonidoCaminar()
    {
        if (audioSource.clip != sonidoCaminar || !audioSource.isPlaying)
        {
            audioSource.clip = sonidoCaminar;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void PararSonidoCaminar()
    {
        if (audioSource.clip == sonidoCaminar && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = null;
        }
    }

    public void ReproducirSonidoRecompensa()
    {
        if (sonidoRecompensa != null)
        {
            audioSource.PlayOneShot(sonidoRecompensa);
        }
    }
}
