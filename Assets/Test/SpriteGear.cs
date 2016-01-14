using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class SpriteGear : MonoBehaviour {

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    Sprite eyesOpenedSprite;

    [SerializeField]
    Sprite[] eyesClosedSprite;

    [SerializeField]
    Sprite funSprite;

    void Start ()
    {
        var resetTimer = Observable.FromCoroutine<float>(RandomInterval);

        var time = 0.0f;

        bool fun = false;
        resetTimer.Subscribe(_ =>
        {
            time = 0;
            fun = Random.Range(0.0f, 1) < 0.2;
        });

        this.UpdateAsObservable().Subscribe(_ =>
        {
            if (fun)
            {
                spriteRenderer.sprite = funSprite;
            }
            else
            {
                float et = time / 0.1f;
                if (et < 1)
                {
                    int id = (int)(Mathf.Clamp01(1 - Mathf.Abs(et * 2 - 1)) * eyesClosedSprite.Length);
                    spriteRenderer.sprite = eyesClosedSprite[id];
                }
                else
                {
                    spriteRenderer.sprite = eyesOpenedSprite;
                }
            }

            time += Time.deltaTime;
        });
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
