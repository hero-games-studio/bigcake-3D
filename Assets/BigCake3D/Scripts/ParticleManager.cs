using UnityEngine;

public class ParticleManager : MonoSingleton<ParticleManager>
{
    #region Variables
    [Header("Firework Particles")]
    [SerializeField]
    private ParticleSystem[] fireworks = null;

    #endregion

    #region Builtin Methods
    #endregion

    #region Custom Methods
    public void PlayFireworks()
    {
        foreach (ParticleSystem particle in fireworks)
        {
            particle.Play();
        }
    }

    public void StopFireworks()
    {
        foreach (ParticleSystem particle in fireworks)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
    #endregion
}
