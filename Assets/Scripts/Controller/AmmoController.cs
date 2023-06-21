using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : Spawnable
{
   
    public int AttackPow { get; set; }

    public IHealth TargetIhealth;

    private float rotateSpeed = 200f;

    private Rigidbody2D rb;

    public Vector3 TargetPos; 

    public float speed;

    private bool targedDestroyed;


    public void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        EventManager.TargerDestroyed.AddListener(TargerDestroyed);
        targedDestroyed = false;
    }

    private void TargerDestroyed(Vector3 target)
    {
        if (TargetPos == target)
            targedDestroyed = true;
    }

    void FixedUpdate()
    {

        Vector2 direction = (Vector2)TargetPos - rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;

        rb.velocity = transform.up * speed;

        if (Vector3.Distance(TargetPos, transform.position)<1.2f)
        {
            PoolManager.Instance.ReturnObject(gameObject, PoolObjectType);
            if(!targedDestroyed)
            GiveDamage(AttackPow, TargetIhealth);

            EventManager.MissileHit.Invoke(gameObject);
        }
    }


    public void GiveDamage(int damage, IHealth target) => target.TakeDamage(damage);



}
