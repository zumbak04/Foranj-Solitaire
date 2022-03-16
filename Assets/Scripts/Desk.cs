using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Desk : MonoBehaviour
{
    private Vector3 offset;
    private List<GameObject> cards;

    private void Start()
    {
        cards = new List<GameObject>();
    }

    public void GenerateCard(Cards card, int orderInLayer)
    {
        Suits suit = (Suits)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Suits)).Length);

        var cardObj = Instantiate(GameAssets.instance.card, gameObject.transform.position + offset, Quaternion.identity);
        offset.y -= 0.1f;
        cardObj.transform.parent = gameObject.transform;
        cards.Add(cardObj);

        var cardCom = cardObj.GetComponent<Card>();
        cardCom.ChangeCardAndSuit(card, suit);
        cardCom.spriteRenderer.sortingOrder = orderInLayer;
    }
}
