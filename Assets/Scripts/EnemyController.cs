using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Movement Settings")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float range = 2.5f;
    [SerializeField] private GameObject batHit;
    private float startingPointY;
    private int dir = 1;

    private void Start()
    {
        startingPointY = transform.position.y;
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime * dir);
        if (transform.position.y < startingPointY || transform.position.y > startingPointY + range)
            dir *= -1;
    }

    public void DestroyEnemy(Vector3 position)
    {
        Instantiate(batHit, position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(0);
        }
    }
}
