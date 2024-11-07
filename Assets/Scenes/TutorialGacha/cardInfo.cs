using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity{Legend, Epic, Rare, Uncommon, Common};
[CreateAssetMenu(fileName = "new card", menuName = "Character")]
public class cardInfo : ScriptableObject
{
    public Sprite image;
    public string name;
    public Rarity rarity;
}
