using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxEffectManager : Singleton<FxEffectManager>
{
    private List<ParticleSystem> fxEffects = new List<ParticleSystem>();
    
    public void PlayEffect(string particleAddress, Vector3 pos)
    {
        ParticleSystem fxEffect = Resources.Load<ParticleSystem>(particleAddress);
        PlayEffect(fxEffect, pos);
    }

    public void PlayEffect(ParticleSystem particle, Vector3 pos)
    {
        if (particle == null)
            return;

        string particleName = particle.name;
        foreach (ParticleSystem fxEffect in fxEffects)
        {
            if(fxEffect.name.Contains(particleName) && !fxEffect.isPlaying)
            {
                fxEffect.transform.position = pos;
                fxEffect.Play();
                return;
            }
        }

        InstanceParticle(particle, pos);
    }
    private void InstanceParticle(ParticleSystem particle, Vector3 pos)
    {
        ParticleSystem effect = Instantiate(particle, transform);
        fxEffects.Add(effect);
        effect.transform.position = pos;
        effect.Play();
    }
}
