using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Character Player;

    [Header("Player Status Data")] 
    public StatusData playerAttackStatus;
    public StatusData playerDefenseStatus;
    public StatusData playerHpStatus;
    public StatusData playerCriticalStatus;
    public int playerGoldStatus;

    [Header("UI References")] 
    private UIStatus uiStatusManager; // UIStatusManager 스크립트 참조

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Player = new Character();
        Player.SetAttackStatus(playerAttackStatus);
        Player.SetDefenseStatus(playerDefenseStatus);
        Player.SetHpStatus(playerHpStatus);
        Player.SetCriticalStatus(playerCriticalStatus);
        Player.SetGold(playerGoldStatus);
    }
}