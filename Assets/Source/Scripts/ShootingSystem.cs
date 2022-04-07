using Supyrb;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    [SerializeField] private Transform spawnBulletPos;
    [SerializeField] private Pool bulletPool;
    [SerializeField] private Pool laserPool;
    private BulletComponent bulletComponent;

    private void Start()
    {
        Signals.Get<ShootSignal>().AddListener(Shoot);
        Signals.Get<ApplyLaserSignal>().AddListener(LaserShoot);
    }

    void Shoot()
    {
        GetAmmunition(bulletPool);
    }

    void GetAmmunition(Pool pool)
    {
        bulletComponent= pool.GetObject().GetComponent<BulletComponent>();
        bulletComponent.gameObject.SetActive(true);
        bulletComponent.transform.position = spawnBulletPos.position;
        if(bulletComponent!=null)
            bulletComponent.Project(transform.up);
    }

    void LaserShoot(Transform player)
    {
        GetAmmunition(laserPool);
        bulletComponent.transform.rotation = player.rotation;
    }
}
