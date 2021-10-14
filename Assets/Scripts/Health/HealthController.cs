using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    public float Health = 100;
    [SerializeField]
    private bool ControlHealthBar = false;
    [SerializeField]
    private GameObject HealthBarScale;
    [SerializeField]
    private string HitboxTarget;
    private Vector3 Scale;
    private bool CanDamaged = true;

    void Start()
    {
        if (ControlHealthBar)
        {
            Scale = HealthBarScale.transform.localScale;
        }
    }

    void Update()
    {
        if (ControlHealthBar)
        {
            Scale.x = (Health * 100 / 100) / 100;
            HealthBarScale.transform.localScale = Scale;
        }
    }

    public void TakeDamage(float damage)
    {
        if (Health > 0)
        {
            if (Health - damage >= 0)
            {
                Health -= damage;
            }
            else
            {
                Health = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == HitboxTarget && CanDamaged)
        {
            
            Health -= collision.gameObject.GetComponent<DamageController>().ATK_Damage;
            CanDamaged = false;
            Invoke("ReturnCanDamaged", 0.4f);
        }
    }

    private void ReturnCanDamaged()
    {
        CanDamaged = true;
    }

}
