using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PP
{
    public class VolumeController : MonoBehaviour
    {
        private Volume _volume;
        private FilmGrain _filmGrain;
        private ColorAdjustments _colorAdjustments;
        private ChromaticAberration _chromaticAberration;
        private PaniniProjection _paniniProjection;
        private Vignette _vignette;
        
        private void Start()
        {
            _volume = GetComponent<Volume>();
            
            _volume.profile.TryGet(out _filmGrain);
            _volume.profile.TryGet(out _colorAdjustments);
            _volume.profile.TryGet(out _chromaticAberration);
            _volume.profile.TryGet(out _paniniProjection);
            _volume.profile.TryGet(out _vignette);
            
        }
    }
}
