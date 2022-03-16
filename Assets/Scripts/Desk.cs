using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Desk : MonoBehaviour
{
    public Vector3 offset;
    protected List<GameObject> cards;

    private void Start()
    {
        cards = new List<GameObject>();
    }

    public void AddCardOnTop(GameObject card)
    {
        GameObject parent = FindCardOnTop();
        card.transform.parent = parent.transform;
        cards.Add(card);

        var cardCom = card.GetComponent<Card>();
        if(parent.TryGetComponent<Card>(out Card parentCom))
        {
            cardCom.AddParent(parentCom);
        }
    }
    public void GenerateCard(Cards card, int orderInLayer)
    {
        Suits suit = (Suits)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Suits)).Length);

        Transform parent = FindCardOnTop().transform;
        var cardObj = Instantiate(GameAssets.instance.card, parent.position + offset, Quaternion.identity);
        AddCardOnTop(cardObj);

        var cardCom = cardObj.GetComponent<Card>();
        cardCom.ChangeCardAndSuit(card, suit);
        cardCom.spriteRenderer.sortingOrder = orderInLayer;
    }
    public GameObject FindCardOnTop()
    {
        if (cards.Count < 1)
        {
            return gameObject;
        }
        else
        {
            return cards[cards.Count - 1];
        }
    }
}
