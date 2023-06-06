using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    private PlayerController playerController;
    private Transform cameraTransform, playerTransform;
    private BoxCollider2D levelLimit;
    private float cameraSizeVertical, cameraSizeHorizontal;

    void Start()
    {
        cameraTransform = GetComponent<Transform>();
        playerController = FindObjectOfType<PlayerController>();
        playerTransform = playerController.transform;
        levelLimit = GameObject.Find("LevelLimit").GetComponent<BoxCollider2D>();
        cameraSizeVertical = Camera.main.orthographicSize;
        cameraSizeHorizontal = cameraSizeVertical * Camera.main.aspect;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerController != null)
            cameraTransform.position = new Vector3(Mathf.Clamp(playerTransform.position.x,
                                                               levelLimit.bounds.min.x + cameraSizeHorizontal,
                                                               levelLimit.bounds.max.x - cameraSizeHorizontal),
                                                   Mathf.Clamp(playerTransform.position.y,
                                                               levelLimit.bounds.min.y + cameraSizeVertical,
            levelLimit.bounds.max.y - cameraSizeVertical),
            cameraTransform.position.z)
        ;
    }
}