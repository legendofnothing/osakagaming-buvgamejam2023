using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;


    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * _bulletSpeed, Space.Self);
    }
}
