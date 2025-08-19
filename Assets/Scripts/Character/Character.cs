using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Character : GameEntity
{
    [Header("CHARACTER")]
    public int _ranking;
    public string _name;
    public int _goldReward = 0;
    public Color _color;
    public Transform _ringTF;
    public SkinnedMeshRenderer _skinRenderer;
    public SkinnedMeshRenderer _pantRenderer;
    public Transform _hairHolderTF;
    public Transform _weaponHolderTF;
    public Transform _shieldHolderTF;
    public Transform _wingHolderTF;
    public Transform _tailHolderTF;
    public Transform _characterModelTF;
    public EquipmentData _equipmentData;
    public PlayerData _playerData;
    public AttackAreaController _attackAreaController;
    public Collider _collider;
    public Action<Collider> OnCharacterDead;

    protected Character targetLocked;
    protected WeaponController weaponController;
    protected AudioSource _audioSource;
    protected PoolType projectilePool;

    private List<Character> targetList = new List<Character>();
    private Dictionary<eWeapons ,WeaponController> equippedWeapons = new Dictionary<eWeapons, WeaponController>();
    private Dictionary<eHairs, Transform> equippedHairs = new Dictionary<eHairs, Transform>();
    private Dictionary<eShields, Transform> equppedShields = new Dictionary<eShields, Transform>();

    private eSkins characterSkin;
    private eHairs characterHair;
    private eWeapons characterWeapon;
    private eShields characterShield;
    private eSkinSets characterSkinSets;

    [Header("ANIMATOR")]
    public Animator animator;

    [Header("ATTACK")]
    public Transform muzzleTF;
    public float attackSpeed;
    public Vector3 projectileSize;

    [Header("CANVAS")]
    public Canvas _characterCanvas;
    public Image _rankBoard;
    public TextMeshProUGUI _nameText;
    public TextMeshProUGUI _rankText;

    [Header("SOUND")]
    public AudioClip _hitClip;
    public AudioClip _sizeUpClip;


    public void SizeUp()
    {
        SizeUpSound();
        _characterModelTF.localScale += new Vector3(.1f, .1f, .1f);
    }
    public void ProjectSizeUp()
    {
        projectileSize += new Vector3(.1f, .1f, .1f);
    }
    public void CanvasUp()
    {
        _characterCanvas.transform.localPosition += new Vector3(0f, .25f, 0f);
    }
    public void AddTarget(Collider collider)
    {
        Character c = ComponentSaver.GetCharacter(collider);
        if (targetList.Contains(c)) return;
        targetList.Add(c);
        c.OnCharacterDead += RemoveTarget;
        if (!targetLocked)
        {
            targetLocked = c;
            targetLocked._ringTF.gameObject.SetActive(true);
        }
    }
    public void RemoveTarget(Collider collider)
    {
        Character c = ComponentSaver.GetCharacter(collider);
        if (!targetList.Contains(c)) return;
        targetList.Remove(c);
        c.OnCharacterDead -= RemoveTarget;
        if (targetLocked == c)
        {
            targetLocked._ringTF.gameObject.SetActive(false);
            targetLocked = null;
        }
    }
    public Character GetTarget()
    {
        Character target = null;
        if (targetList.Count > 0)
        {
            target = targetList[UnityEngine.Random.Range(0, targetList.Count)];
            target?._ringTF.gameObject.SetActive(true);
        }
        return target;
    }
    public void SetSkin(eSkins skin)
    {
        _skinRenderer.material = _equipmentData.skinData[(int)skin];
    }
    public void SetPant(ePants pant)
    {
        _pantRenderer.material = _equipmentData.pantData[(int)pant];
    }
    public void SetHair(eHairs hair)
    {
        if (hair != eHairs.None)
        {
            if (equippedHairs.ContainsKey(characterHair) && equippedHairs[characterHair].gameObject.activeSelf)
            {
                equippedHairs[characterHair].gameObject.SetActive(false);
            }
            if (!equippedHairs.ContainsKey(hair))
            {
                Transform equipHair = Instantiate(_equipmentData.hairData[(int)hair], _hairHolderTF);
                equipHair.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                equippedHairs[hair] = equipHair;
            }
            characterHair = hair;
            equippedHairs[characterHair].gameObject.SetActive(true);
        }
    }
    public void SetWeapon(eWeapons weapon)
    {
        if (weapon != eWeapons.None)
        {
            if (equippedWeapons.ContainsKey(characterWeapon) && equippedWeapons[characterWeapon].gameObject.activeSelf)
            {
                equippedWeapons[characterWeapon].gameObject.SetActive(false);
            }
            if (!equippedWeapons.ContainsKey(weapon))
            {
                WeaponController equipWeapon = Instantiate(_equipmentData.weaponData[(int)weapon], _weaponHolderTF);
                equipWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                equippedWeapons[weapon] = equipWeapon;
            }
            characterWeapon = weapon;
            equippedWeapons[characterWeapon].gameObject.SetActive(true);
            weaponController = equippedWeapons[characterWeapon];
            weaponController.WeaponOwner(this, _collider);
        }
    }
    public void SetShield(eShields shield)
    {
        if (shield != eShields.None)
        {
            if (equppedShields.ContainsKey(characterShield) && equppedShields[characterShield].gameObject.activeSelf)
            {
                equppedShields[characterShield].gameObject.SetActive(false);
            }
            if (!equppedShields.ContainsKey(shield))
            {
                Transform equipShield = Instantiate(_equipmentData.shielData[(int)shield], _shieldHolderTF);
                equipShield.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                equppedShields[shield] = equipShield;
            }
            characterShield = shield;
            equppedShields[characterShield].gameObject.SetActive(true);
        }
    }
    public void SetSkinSet(eSkinSets skinset)
    {

    }
    public void SetNameTextColor(Color color)
    {
        _nameText.color = color;
    }
    public void SetNameText(string text)
    {
        _name = text;
        _nameText.text = text;
    }
    public void SetRankBoardColor(Color color)
    {
        _rankBoard.color = color;
    }
    public void SetRankText(int rank)
    {
        _ranking += rank;
        _rankText.text = _ranking.ToString();
    }
    public void HitSound()
    {
        _audioSource.clip = _hitClip; 
        _audioSource.Play();
    }
    public void SizeUpSound()
    {
        _audioSource.clip = _sizeUpClip;
        _audioSource.Play();
    }
}

