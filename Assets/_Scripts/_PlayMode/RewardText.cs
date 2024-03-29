using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RewardText : MonoBehaviour
{
    [SerializeField] private Text m_Text;
    [SerializeField] private Target target;
    [SerializeField] private GameKnife gameKnife;

    private Animation m_Animation;

    private void Awake()
    {
        m_Animation = GetComponent<Animation>();
        target.OnHit += Show;

        gameObject.SetActive(false);
    }

    private void Show()
    {
        m_Text.text = $"+{(int)(gameKnife.CoinsPerHit * gameKnife.CoinsMultiplyer)} ";

        gameObject.SetActive(true);
        m_Animation.Play();

        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(m_Animation.clip.length);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        target.OnHit -= Show;
    }
}
