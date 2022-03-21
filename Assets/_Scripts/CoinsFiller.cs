using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//Писал этот код по уроку (https://www.youtube.com/watch?v=ObcS4M6ILi4)
public class CoinsFiller : MonoBehaviour
{
    [SerializeField] private float duration;

    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    public void Fill(int startValue, int endValue, float delay = 0)
    {
        StartCoroutine(FillCoins(startValue, endValue, delay));
    }

    private IEnumerator FillCoins(float startValue, float endValue, float delay)
    {
        yield return new WaitForSeconds(delay);

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
