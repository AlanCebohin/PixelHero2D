using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private Rigidbody2D arrowRB;
    [SerializeField] private float arrowSpeed;
    private GameObject levelLimit;

    [SerializeField] private GameObject arrowImpact;
    [SerializeField] private EnemyController enemyController;
    private Transform arrowTransform;

    private void Awake()
    {
        arrowRB = GetComponent<Rigidbody2D>();
        arrowTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        levelLimit = GameObject.Find("LevelLimit");
        arrowRB.velocity = arrowTransform.right * arrowSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != levelLimit.name && collision.gameObject.name != "Player" && collision.tag != "Enemy")
        {
            Instantiate(arrowImpact, arrowTransform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            enemyController.DestroyEnemy(collision.transform.position);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}