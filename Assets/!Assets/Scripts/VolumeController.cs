using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class VolumeController : MonoBehaviour
{
    [SerializeField] private Volume _volume;

    private Vignette _vignette;

    private Sequence _vignetteSequenceAnimation;

    private void Start()
    {
        VolumeProfile volumeProfile = _volume.profile;

        if (!volumeProfile.TryGet(out _vignette)) {
            throw new System.NullReferenceException(nameof(_vignette));
        }

        // TODO - do good animation!
        _vignetteSequenceAnimation = DOTween.Sequence()
          .Append(DOTween.To(() => _volume.weight, x => _volume.weight = x, 0.75f, 0.25f))
          .AppendInterval(1.0f)
          .Append(DOTween.To(() => _volume.weight, x => _volume.weight = x, 0f, 0.25f))
          .AppendInterval(1.0f)
          .Append(DOTween.To(() => _volume.weight, x => _volume.weight = x, 0.75f, 0.25f))
          .AppendInterval(1.0f)
          .Append(DOTween.To(() => _volume.weight, x => _volume.weight = x, 0f, 0.25f))
          .Pause();

    }

    public void AnimateLowHealthIndicator()
    {
        if (_vignette != null)
        {
            //_vignette.intensity.Interp(0.0f, 0.5f, 0.25f)
            _vignetteSequenceAnimation.Play();
        }
    }

    //private IEnumerator AnimateRecursion()
    //{
    //    yield return new WaitForSeconds(1.0f);

    //    _vignette.intensity.value = 1.0f;
    //}
}
