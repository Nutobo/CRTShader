using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class CRTGear : MonoBehaviour {

    [SerializeField]
    CRT crt;

    void Start() {
        
        var resetTimer = Observable.FromCoroutine<float>(RandomInterval);
        
        var totalTime = 0.0f;
        var time = 0.0f;
        var noisePower = 0.0f;
        var baseNoisePower = 0.0f;
        var noisyTime = 0.0f;
        var offset = Vector2.zero;
        var baseOffset = Vector2.zero;

        resetTimer.Subscribe(t =>
        {
            totalTime = t;
            time = 0;
            noisePower = Random.Range(0.0f, 1.0f);
            baseNoisePower = Mathf.Clamp01(Random.Range(-0.01f, 0.01f));
            noisyTime = Random.Range(0.0f, 0.5f);
            crt.SinNoiseWidth = Random.Range(0.0f, 30.0f);
            offset = Vector2.right * Random.Range(-5.0f, 5) + Vector2.up * Random.Range(-5.0f, 5);
            baseOffset = (Vector2.right * Random.Range(-1.0f, 1) + Vector2.up * Random.Range(-1.0f, 1)) * 0.1f;
        });

        this.UpdateAsObservable().Subscribe(_ =>
        {
            float t = time / totalTime;
            float nt = Mathf.Clamp01(t / noisyTime);
            float np = baseNoisePower + noisePower * (1 - nt);
            crt.NoiseX = np * 0.5f;
            crt.RGBNoise = np * 0.5f;
            crt.SinNoiseScale = np * 0.5f;
            crt.SinNoiseOffset += Time.deltaTime * 5;
            crt.Offset = baseOffset + offset * np;
            time += Time.deltaTime;
        });
        
        // resetTimer.SelectMany(_ => shortNoise).Subscribe(_ => { });
    }

    IEnumerator RandomInterval(IObserver<float> observer)
    {
        while (true)
        {
            float wait = Random.Range(0.2f, 2);
            observer.OnNext(wait);
            yield return new WaitForSeconds(wait);
        }

    }
	
}
