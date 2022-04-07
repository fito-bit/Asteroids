using NaughtyAttributes;
using Supyrb;
using UnityEngine;

public class UfoComponent : Enemy
{
    [SerializeField] private float speed;
    private GameObject playerObject;
    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(playerObject!=null)
            transform.position=Vector2.MoveTowards(
                transform.position,
                playerObject.transform.position, 
                speed);
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
        Signals.Get<AddScoreSignal>().Dispatch();
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(bulletTag))
        {
            other.gameObject.SetActive(false);
            Die();
        }
    }
}
