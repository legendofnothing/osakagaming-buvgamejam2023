using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public LayerMask enemyLayer;
    [SerializeField] private float _bulletSpeed;
    bool hasReturned;

    private void OnEnable()
    {
        hasReturned = false;
        StartCoroutine(DelayDisableObject(4));
    }

    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * _bulletSpeed, Space.Self);
    }

    IEnumerator DelayDisableObject(float delayTime)
    {
        float delayCount = 0;
        while (delayCount < delayTime)
        {
            delayCount += Time.deltaTime;
            yield return null;
        }

        if(!hasReturned)
        {
            ObjectPool.ReturnObjectToPool(gameObject);
            hasReturned = true;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (CheckLayerMask.IsInLayerMask(collision.gameObject, enemyLayer))
        {
            StartCoroutine(DelayDisableObject(0.0001f));
        }
    }
}
