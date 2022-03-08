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
        Wallet.Instance.OnValueChanged += CoinsChanged;

        CoinsChanged();
    }

    private void OnDisable()
    {
        Wallet.Instance.OnValueChanged -= CoinsChanged;
    }

    private void CoinsChanged()
    {
        _text.text = Wallet.Instance.Coins.ToString();
    }

    
}
