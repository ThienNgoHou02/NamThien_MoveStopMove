using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    [Header("Level Component")]
    [SerializeField] private Transform charactersOnLevelTF;
    [SerializeField] private Transform[] spnPoints;
    [SerializeField] private int maxAlives;
    [SerializeField] private string nextZone;

    [Header("Character")]
    [SerializeField] private PlayerController player;
    [SerializeField] private BotController bot;
    [SerializeField] private int numOfBots;

    [Header("Data")]
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private EquipmentData _equipmentData;

    CanvasGameplay game;
    Action OnVictory;
    
    public List<Character> OnStage = new List<Character>();

    int index = 0;
    int gold;
    int deadAmount;
    string killer;
    bool _endGame;
    enum CharacterNames
    {
        Arvelion = 0,
        Kaelthar = 1,
        Liora = 2,
        Draven = 3,
        Sylmaris = 4,
        Thalorien = 5,
        Zeyra = 6,
        Orvan = 7,
        Myrath = 8,
        Veloria = 9
    }
    private void Awake()
    {
        Instance = this;
        game = UIManager.Instance.OpenUI<CanvasGameplay>();
        game.SetAliveAmountText(maxAlives);
        
    }
    private void Start()
    {
        deadAmount = 0;
        SetupCharacter();
    }
    private void SetupCharacter()
    {
        PlayerController p = Instantiate(player, charactersOnLevelTF);
        OnVictory += p.Victory;
        p.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        p.JoyStick = game.Joystick;
        p.SetSkin(_playerData._skin);
        p.SetHair(_playerData._hair);
        p.SetPant(_playerData._pant);
        p.SetWeapon(_playerData._weapon);

        OnStage.Add(p);

        CameraController.instance.Follow = p.transform;

        for (int i = 0; i < numOfBots; i++)
        {
            int hair = UnityEngine.Random.Range(0, 8);
            int skin = UnityEngine.Random.Range(1, 8);
            int pant = UnityEngine.Random.Range(1, 9);
            int weapon = UnityEngine.Random.Range(1, 8);
            BotController b = Instantiate(bot, charactersOnLevelTF);
            b.OnDead += AliveMinus;
            b.transform.SetLocalPositionAndRotation(spnPoints[i].position, Quaternion.identity);
            b.SetHair((eHairs)hair);
            b.SetSkin((eSkins)skin);
            b.SetPant((ePants)pant);
            b.SetWeapon((eWeapons)weapon);

            Color color = _equipmentData.skinData[skin].color;
            b.SetNameTextColor(color);
            b.SetNameText(((CharacterNames)i).ToString());
            b.SetRankBoardColor(color);
            b._color = color;

            OnStage.Add(b);
        }
    }
    public void CharacterRevive(Character character)
    {
        OnStage.Add(character);
        character.transform.position = spnPoints[index].position;
        index++;
        if (index >= spnPoints.Length)
            index = 0;
    }
    public void AliveMinus(Character character)
    {
        deadAmount += 1;
        game.SetAliveAmountText(maxAlives - deadAmount);
        if (OnStage.Contains(character))
        {
            OnStage.Remove(character);
        }
        if (OnStage.Count <= 1 && deadAmount + OnStage.Count >= maxAlives)
        {
            Victory();
        }
    }
    public void Victory()
    {
        if (!_endGame)
        {
            _endGame = true;
            OnVictory.Invoke();
            this.gold = OnStage[0]._goldReward;
            _playerData._gold += gold;
            _playerData._nextScene = nextZone;
            game.Close(0);
            UIManager.Instance.OpenUI<CanvasVictory>();
        }
    }
    public void Defead(string killer, int gold)
    {
        if (!_endGame)
        {
            _endGame = true;
            this.killer = killer;
            this.gold = gold;
            _playerData._gold += gold;
            game.Close(0);
            UIManager.Instance.OpenUI<CanvasRevive>();
        }
    }
    public int Rank
    {
        get { return maxAlives; }
    }
    public string Killer
    {
        get { return killer; }
    }
    public int Gold
    {
        get { return gold; }
    }
    public bool CanRevive()
    {
        return deadAmount + OnStage.Count < maxAlives;
    }
}
