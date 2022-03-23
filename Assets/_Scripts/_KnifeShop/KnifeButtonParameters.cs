using UnityEngine;

namespace ButtonParameters
{
    [CreateAssetMenu(fileName = "KnifeButtonParameters", menuName = "KnifeButtonParameters", order = 0)]
    public class KnifeButtonParameters : ScriptableObject
    {
        [Header("Colors")]
        [SerializeField] private Color purchasedKnifeColor;
        [SerializeField] private Color nonPurchasedKnifeColor;

        public Color PurchasedKnifeColor => purchasedKnifeColor;
        public Color NonPurchasedKnifeColor => nonPurchasedKnifeColor;

        [Header("Sprites")]
        [SerializeField] private Sprite selectedButtonSprite;
        [SerializeField] private Sprite unselectedButtonSprite;
        [SerializeField] private Sprite randomButtonSprite;

        public Sprite SelectedButtonSprite => selectedButtonSprite;
        public Sprite UnselectedButtonSprite => unselectedButtonSprite;
        public Sprite RandomButtonSprite => randomButtonSprite;
    }
}

