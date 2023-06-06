using System.Collections;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [Header("Extras/ Skills")]
    public SpriteRenderer sprite;
    private GameObject player;

    [Header("Item Fade Settings")]
    [SerializeField] private float fadeDurationTime = -0.05f;
    [SerializeField] private float fadeSpeed = 0.05f;

    [Header("Item Translation Settings")]
    [SerializeField] private float translationUnits = 0.1f;
    [SerializeField] private float translationSpeed = 1f;
    private Transform itemTransform;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
        itemTransform = GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetTracker();
        }
    }

    private void SetTracker()
    {
        StartCoroutine("FadeOutItem");

        StartCoroutine("MoveItemUp");
    }

    private IEnumerator FadeOutItem()
    {
        for (float i = 1; i >= fadeDurationTime; i -= fadeSpeed)
        {
            Color c = sprite.material.color;
            c.a = i;
            sprite.material.color = c;

            yield return new WaitForSeconds(fadeDurationTime);
        }

        Destroy(gameObject);
    }

    private IEnumerator MoveItemUp()
    {
        Vector3 destroyItemPosition = new Vector2(itemTransform.position.x, itemTransform.position.y + 2);

        for (float i = 0; i <= 2; i += translationUnits)
        {
            if (itemTransform.position != destroyItemPosition)
            {
                itemTransform.position = new Vector2(itemTransform.position.x, itemTransform.position.y + (translationUnits * translationSpeed));
            }

            yield return new WaitForSeconds(0.02f);
        }
    }
}
