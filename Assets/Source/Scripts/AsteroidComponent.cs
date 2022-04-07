using System.Collections;
using NaughtyAttributes;
using Supyrb;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidComponent : Enemy
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float splitSize;
    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;
    [SerializeField] private float forceStrength=50;
    [SerializeField] private float lifeTime;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float actualSize;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(bulletTag))
        {
            other.gameObject.SetActive(false);
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(laserTag))
        {
            Die();
        }
    }

    protected void Die()
    {
        if (actualSize >= splitSize)
        {
            Split();
        }
        Signals.Get<AddScoreSignal>().Dispatch();
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void Split()
    {
        Signals.Get<SplitAsteroidSignal>().Dispatch(transform,spriteRenderer.sprite);
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(lifeTime);
        this.gameObject.SetActive(false);
    }

    public void SetAsteroid(Vector3 spawnDirection)
    {
        ApplyParameters(
            Random.Range(minSize, maxSize),
            sprites[Random.Range(0, sprites.Length)],
            spawnDirection*forceStrength);
    }
    
    public void SetAsteroid(float size,Sprite sprite)
    {
        ApplyParameters(size,sprite,Random.insideUnitCircle.normalized*forceStrength*10);
    }

    void ApplyParameters(float size,Sprite sprite,Vector3 force)
    {
        actualSize = size;
        transform.localScale = Vector3.one*actualSize;
        rb.mass = actualSize;
        transform.eulerAngles = Vector3.forward*Random.Range(0, 360);
        spriteRenderer.sprite = sprite;
        rb.AddForce(force);
    }
}
