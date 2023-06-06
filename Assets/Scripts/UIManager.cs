using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ItemsManager itemsManager;

    private void OnGUI()
    {
        GUILayout.Label("Heart Shinning Left: " + itemsManager.heartShinningLeft);
        GUILayout.Label("Coin Spinning Left: " + itemsManager.coinSpinningLeft);
        GUILayout.Label("Coin Shinning Left: " + itemsManager.coinShinningLeft);
    }
}
