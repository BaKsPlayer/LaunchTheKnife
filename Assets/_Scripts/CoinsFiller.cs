using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Писал этот код по уроку (https://www.youtube.com/watch?v=ObcS4M6ILi4)
public class CoinsFiller : MonoBehaviour
{
    public float duration;

    Text _text;
    
    public IEnumerator FillCoins(float startValue, float endValue, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        _text = GetComponent<Text>();

        float timer = 0;

        float nextValue;

        while (timer < duration)
        {
            nextValue = Mathf.Lerp(startValue, endValue, timer / duration);

            _text.text = ((int)nextValue).ToString();

            timer += Time.deltaTime;

            yield return null;
        }

        _text.text = endValue.ToString();
    }
}
