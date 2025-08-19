using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : GameEntity
{
    ParticleSystem _particle;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }
    private void OnParticleSystemStopped()
    {
        SimplePool.PushToPool(this);        
    }
    public void SetColor(Color color)
    {
        var main = _particle.main;
        main.startColor = color;
    }
}
