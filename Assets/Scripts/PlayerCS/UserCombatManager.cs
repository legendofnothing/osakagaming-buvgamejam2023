using Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCombatManager : MonoBehaviour
{
      
    [SerializeField] private float maxRecoveryTime;
    private float recoveryTime;

    [SerializeField] private Transform _shootPoint;
    [SerializeField] private GameObject bulletPF;
    [SerializeField] private float shootDelay;
    private bool canShoot;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        canShoot = true;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(!canShoot) { return; }

            StartCoroutine(Shoot());
        }
    }

    void ShootBullet()
    {
        //GetComponent<EntityBase>().TakeDamage(10);
    }

    IEnumerator Shoot()
    {
        float shootDelayCount = 0;
        canShoot = false;
        //Instantiate(bulletPF, _shootPoint.position, _shootPoint.rotation);
        ObjectPool.SpawnObject(bulletPF, _shootPoint.position, _shootPoint.rotation);

        Vector2 knockBackDir = new Vector2(Mathf.Sin((-_shootPoint.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad), Mathf.Cos((-_shootPoint.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad));
        rb.AddForce(knockBackDir * 9000 * Time.deltaTime, ForceMode2D.Impulse);

        while (shootDelayCount < shootDelay)
        {
            shootDelayCount += Time.deltaTime;
            yield return null;
        }

        canShoot = true;
    }

}
