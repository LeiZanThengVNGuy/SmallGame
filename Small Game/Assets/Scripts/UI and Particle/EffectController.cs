using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class EffectController : MonoBehaviour
{
    Volume volume;
    Vignette vignette;
    Bloom bloom;
    [HideInInspector] public bool IsBlind = false;
    private void Start() {
        volume = gameObject.GetComponent<Volume>();
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<Bloom>(out bloom);
    }
    private void Update() {
        if(IsBlind)
        {
            vignette.active = true;
        }
        if(!IsBlind)
        {
            vignette.active = false;
        }
        if(GameScreen.AllowBloom)
        {
            bloom.active = true;
        }
        if(!GameScreen.AllowBloom)
        {
            bloom.active = false;
        }
    }
}
