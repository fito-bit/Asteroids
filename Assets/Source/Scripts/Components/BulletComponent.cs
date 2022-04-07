using System.Collections;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    [SerializeField] private float forcePower;
    [SerializeField] private float lifeTime;
    private Rigidbody2D rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(lifeTime);
        this.gameObject.SetActive(false);
    }
    
    public void Project(Vector2 direction)
    {
        rb.AddForce(direction*forcePower);
    }
}
