using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab = null;
    [SerializeField] Transform muzzle = null;
    [SerializeField] float bulletSpeed = 3;

    ObjectPooler pooler;

    private void Awake()
    {
        pooler = GetComponent<ObjectPooler>();
    }

    //Called in TankController
    public void Shoot()
    {
        GameObject bullet = pooler.GetObject();

        if (bullet == null) { return; }

        bullet.transform.rotation = Quaternion.Euler(CalculateBulletInitialRotation());
        bullet.transform.position = muzzle.position;
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody>().velocity = muzzle.up * bulletSpeed;
    }

    private Vector3 CalculateBulletInitialRotation()
    {
        Vector3 bulletRotation = bulletPrefab.transform.eulerAngles;
        bulletRotation.y = muzzle.eulerAngles.y;

        return bulletRotation;
    }
}
