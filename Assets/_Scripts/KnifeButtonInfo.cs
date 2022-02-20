using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeButtonInfo : MonoBehaviour
{
    public int id, publicCost, indexInNonPurchased = -1;
    public SafeInt cost;
    public bool isLocked;

    private int huy;


    private void Awake()
    {
        cost = publicCost;
    }
}
