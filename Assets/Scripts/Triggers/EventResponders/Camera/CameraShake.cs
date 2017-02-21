using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

[Trigger(Description = "An effect that shakes the camera like in an earthquake.")]
public class CameraShake : EventResponder
{
    private enum CameraShakeMode
    {
        Start,
        Stop,
    }

#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("Whether or not shaking is to be started or stopped.")]
    private CameraShakeMode _mode;

    [SerializeField]
    [Infobox("How hard the camera will shake.")]
    private float _magnitude;

    [SerializeField]
    [Infobox("How long the camera will shake for. If less than 0, the camera will shake until stopped.")]
    private float _duration;

    [SerializeField]
    [Infobox("Decay rate of camera shake as half-life of magnitude.")]
    private float _decay;

    [SerializeField]
    [Infobox("The camera to shake.")]
    private Camera _camera;
#pragma warning restore 0067, 0649

    private static bool _isRunning = false;
    private static bool _abort = false;

    public override void OnEvent(ExecutionContext context)
    {
        switch (_mode)
        {
            case CameraShakeMode.Start:
                if (!_isRunning)
                {
                    StartCoroutine(ShakeCamera(_duration, _decay, _magnitude, _camera));
                }
                break;
            case CameraShakeMode.Stop:
                _abort = true;
                break;

        }
    }
	
    static IEnumerator ShakeCamera(float duration, float decay, float magnitude, Camera camera)
    {
        _isRunning = true;
        float startingTime = Time.time;
        float elapsed = 0.0f;

        Vector3 originalCamPos = camera.transform.localPosition;

        while (elapsed < duration || duration < 0)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = Mathf.Exp(-decay * (Time.time - startingTime));

            // map value to [-1, 1]
            float x = UnityEngine.Random.value * 2.0f - 1.0f;
            float y = UnityEngine.Random.value * 2.0f - 1.0f;
            x *= magnitude * damper;
            y *= magnitude * damper;

            camera.transform.localPosition = new Vector3(x, y, originalCamPos.z);

            if (_abort)
            {
                camera.transform.localPosition = originalCamPos;
                _abort = false;
                _isRunning = false;
                yield break;
            }
            yield return null;
        }

        camera.transform.localPosition = originalCamPos;
        _isRunning = false;
    }
}

