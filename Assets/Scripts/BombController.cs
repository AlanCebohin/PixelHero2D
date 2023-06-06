using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [Header("Bomb settings")]
    [SerializeField] private float bombTimeCounter;
    [SerializeField] private float destroyWaitingTime;
    private Transform transformBomb;
    [SerializeField] private float bombExplosionRadius;
    [SerializeField] private LayerMask isDestroyable;
    private Animator animator;
    private bool isExploding;
    private int IdIsExploding;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        IdIsExploding = Animator.StringToHash("isExploding");
        transformBomb = GetComponent<Transform>();
    }

    private void Update()
    {
        bombTimeCounter -= Time.deltaTime;

        destroyWaitingTime -= Time.deltaTime;

        if (bombTimeCounter <= 0 && !isExploding)
        {
            BombExplode();
        }
        if (destroyWaitingTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void BombExplode()
    {
        isExploding = true;
        animator.SetBool(IdIsExploding, isExploding);

        Collider2D[] destroyedObjects = Physics2D.OverlapCircleAll(transformBomb.position, bombExplosionRadius, isDestroyable);

        if (destroyedObjects.Length > 0)
        {
            foreach (var collision in destroyedObjects)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
