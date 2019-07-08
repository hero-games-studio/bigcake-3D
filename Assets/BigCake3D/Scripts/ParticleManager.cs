using UnityEngine;

public class ParticleManager : MonoSingleton<ParticleManager>
{
    #region Variables
    [Header("Firework Particles")]
    [SerializeField]
    private ParticleSystem[] fireworks = null;

    #endregion
    #region Custom Methods

    /*
     * METOD ADI :  PlayFireworks
     * AÇIKLAMA  :  Fireworks particle effect'ini başlatır.
     */
    public void PlayFireworks()
    {
        foreach (ParticleSystem particle in fireworks)
        {
            particle.Play();
        }
    }

    /*
     * METOD ADI :  StopFireworks
     * AÇIKLAMA  :  Fireworks particle effect'ini durdurur.
     */
    public void StopFireworks()
    {
        foreach (ParticleSystem particle in fireworks)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
    #endregion
}
