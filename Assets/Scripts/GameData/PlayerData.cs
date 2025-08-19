using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    //Scene
    public string _nextScene;

    //Gold
    public int _gold;

    //PurchasedItem
    public List<eHairs> _purchasedHairs = new List<eHairs>();
    public List<ePants> _purchasedPants = new List<ePants>();
    public List<eWeapons> _purchasedWeapons = new List<eWeapons>();
    public List<eShields> _purchasedShields = new List<eShields>();

    //EquippedItems
    public eSkins _skin;
    public ePants _pant;
    public eWeapons _weapon;
    public eHairs _hair;
    public eShields _shield;
}
