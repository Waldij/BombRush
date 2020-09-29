using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float _shakingForce;
    private Vector3 _lastPosition;
    private Vector3 _lastRotation;
    [Tooltip("Exponent for calculating the shake factor. Useful for creating different effect fade outs")]
    public float TraumaExponent = 1;
    [Tooltip("Maximum angle that the gameobject can shake. In euler angles.")]
    public Vector3 MaximumAngularShake = Vector3.one * 5;
    [Tooltip("Maximum translation that the gameobject can receive when applying the shake effect.")]
    public Vector3 MaximumTranslationShake = Vector3.one * .75f;

    private void Update()
    {
        //if (Input.GetKey(KeyCode.LeftControl)) { Shake(0.5f); }

        float shake = Mathf.Pow(_shakingForce, TraumaExponent);

        if (shake > 0)
        {
            var previousRotation = _lastRotation;
            var previousPosition = _lastPosition;

            _lastPosition = new Vector3(
                MaximumTranslationShake.x * (Mathf.PerlinNoise(0, Time.unscaledTime * 25) * 2 - 1),
                MaximumTranslationShake.y * (Mathf.PerlinNoise(1, Time.unscaledTime * 25) * 2 - 1),
                MaximumTranslationShake.z * (Mathf.PerlinNoise(2, Time.unscaledTime * 25) * 2 - 1)
            ) * shake;

            _lastRotation = new Vector3(
                MaximumAngularShake.x * (Mathf.PerlinNoise(3, Time.unscaledTime * 25) * 2 - 1),
                MaximumAngularShake.y * (Mathf.PerlinNoise(4, Time.unscaledTime * 25) * 2 - 1),
                MaximumAngularShake.z * (Mathf.PerlinNoise(5, Time.unscaledTime * 25) * 2 - 1)
            ) * shake;

            transform.localPosition += _lastPosition - previousPosition;
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + _lastRotation - previousRotation);
            _shakingForce = Mathf.Clamp01(_shakingForce - Time.unscaledDeltaTime);
        }
        else
        {
            if (_lastPosition == Vector3.zero && _lastRotation == Vector3.zero) return;

            transform.localPosition -= _lastPosition;
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles - _lastRotation);
            _lastPosition = Vector3.zero;
            _lastRotation = Vector3.zero;
        }
    }

    /// <summary>
    ///  Applies a stress value to the current object.
    /// </summary>
    /// <param name="shakeForce">[0,1] Amount of stress to apply to the object</param>
    public void Shake(float shakeForce)
    {
        _shakingForce = Mathf.Clamp01(_shakingForce + shakeForce);
    }
}
