using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CoinsText : MonoBehaviour
{
    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        Wallet.Instance.OnValueChanged -= CoinsChanged;

        CoinsChanged();
    }

    private void OnDisable()
    {
        Wallet.Instance.OnValueChanged += CoinsChanged;
    }

    private void CoinsChanged()
    {
        _text.text = Wallet.Instance.Coins.ToString();
    }

    public void DisableComponent(float delay)
    {
        StartCoroutine(Disable(delay));
    }

    private IEnumerator Disable(float delay =0)
    {
        yield return new WaitForSeconds(delay);

        enabled = false;
    }
}
