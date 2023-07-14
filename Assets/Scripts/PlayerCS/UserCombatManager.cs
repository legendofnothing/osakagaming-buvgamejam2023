using Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCombatManager : MonoBehaviour
{
      
    [SerializeField] private float maxRecoveryTime;
    private float recoveryTime;

    [SerializeField] private Transform ShootPoint;
    [SerializeField] private GameObject bulletPF;
    [SerializeField] private float shootDelay;
    private bool canShoot;

    private void Start()
    {
        canShoot = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
        Instantiate(bulletPF, ShootPoint.position, Quaternion.identity);

        while (shootDelayCount < shootDelay)
        {
            shootDelayCount += Time.deltaTime;
            yield return null;
        }

        canShoot = true;
    }

}
