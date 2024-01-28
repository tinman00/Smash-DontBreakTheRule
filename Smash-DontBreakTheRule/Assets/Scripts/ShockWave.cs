using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    [SerializeField] private float _shockWaveTime = 0.75f;

    private Coroutine _shockWaveCoroutine;

    private Material _material;

    private static int _waveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");

    private bool waveDone = false;

    void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    private void Update() {
        if (waveDone) {
            Destroy(gameObject);
        }
    }

    public void CallShockWave() {
        _shockWaveCoroutine = StartCoroutine(ShockWaveActive(-0.1f, 1f));
    }

    private IEnumerator ShockWaveActive(float startPos, float endPos) {
        _material.SetFloat(_waveDistanceFromCenter, startPos);

        float lerpedAmount = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < _shockWaveTime) {
            elapsedTime += Time.deltaTime;

            lerpedAmount = Mathf.Lerp(startPos, endPos, (elapsedTime / _shockWaveTime));
            _material.SetFloat(_waveDistanceFromCenter, lerpedAmount);

            yield return null;
        }
        waveDone = true;
        yield break;
    }
}
