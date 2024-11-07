using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Cards : MonoBehaviour
{
    public cardInfo card;
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI name;

    private void LateUpdate() {
        if(card != null)
        {
            img.sprite = card.image;
            name.text = card.name;
        }
    }
}
