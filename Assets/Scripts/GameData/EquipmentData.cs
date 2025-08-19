using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Equipments", menuName = "Equipment/EquipmentData", order = 1)]
public class EquipmentData : ScriptableObject
{
    public Material[] skinData;
    public Material[] pantData;
    public Transform[] hairData;
    public Transform[] shielData;
    public WeaponController[] weaponData;
}
public enum eSkins
{
    White = 0,
    Red = 1,
    Green = 2,
    Blue = 3,
    Yellow = 4,
    Orange = 5,
    Purple = 6,
    Pink = 7,
    Black = 8
}
public enum ePants
{
    None = 0,
    America = 1,
    Batman = 2,
    Dots = 3,
    Onion = 4,
    Panther = 5,
    Pokemon = 6,
    PurpleVenis = 7,
    Rainbow = 8,
    Skull = 9
}
public enum eWeapons
{
    None = -1,
    Axe = 0,
    Boomerang = 1,
    Candy = 2,
    Hammer = 3,
    Knife = 4,
    SphereCandy = 5,
    Uzi = 6,
    ZShape = 7,
}
public enum eHairs
{
    None = -1,
    Arrow = 0,
    Cap = 1,
    CaptainHat = 2,
    Cowboy = 3,
    Crown = 4,
    Headphone = 5,
    Horn = 6,
    RabbitEar = 7,
    StrawHat = 8
}
public enum eShields
{
    None = -1,
    Normal = 0,
    CaptainAmerica = 1
}
public enum eSkinSets
{

}
