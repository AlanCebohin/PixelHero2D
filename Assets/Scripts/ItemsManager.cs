using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    [SerializeField] private PlayerExtrasTracker playerExtrasTracker;

    private string itemType;

    [HideInInspector] public int heartShinningLeft = 5;
    [HideInInspector] public int coinSpinningLeft = 6;
    [HideInInspector] public int coinShinningLeft = 10;

    private void Start()
    {
        playerExtrasTracker = GetComponent<PlayerExtrasTracker>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        itemType = other.tag;
        switch (itemType)
        {
            case "HeartShinning":
                CanUnlockExtra("HeartShinning");
                break;
            case "CoinSpinning":
                CanUnlockExtra("CoinSpinning");
                break;
            case "CoinShinning":
                CanUnlockExtra("CoinShinning");
                break;
        }
    }

    private void CanUnlockExtra(string item)
    {
        switch (item)
        {
            case "HeartShinning":
                if (heartShinningLeft > 0) heartShinningLeft--;
                if (heartShinningLeft == 0) playerExtrasTracker.CanDoubleJump = true;
                break;
            case "CoinSpinning":
                if (coinSpinningLeft > 0) coinSpinningLeft--;
                if (coinSpinningLeft == 0) playerExtrasTracker.CanDash = true;
                break;
            case "CoinShinning":
                if (coinShinningLeft > 0) coinShinningLeft--;
                if (coinShinningLeft == 0)
                {
                    playerExtrasTracker.CanEnterBallMode = true;
                    playerExtrasTracker.CanDropBomb = true;
                }
                break;
        }
    }
}
