using UnityEngine;

namespace ButtonParameters
{
    public class KnifeButtonParameters : MonoBehaviour
    {
        [SerializeField] private Color purchasedKnifeColor;
        [SerializeField] private Color nonPurchasedKnifeColor;

        public Color PurchasedKnifeColor => purchasedKnifeColor;
        public Color NonPurchasedKnifeColor => nonPurchasedKnifeColor;

        [SerializeField] private Sprite selectedButtonSprite;
        [SerializeField] private Sprite unselectedButtonSprite;
        [SerializeField] private Sprite randomButtonSprite;

        public Sprite SelectedButtonSprite => selectedButtonSprite;
        public Sprite UnselectedButtonSprite => unselectedButtonSprite;
        public Sprite RandomButtonSprite => randomButtonSprite;
    }
}

