using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCore : MonoBehaviour
{
    public Transform ExplosionEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter()
    {
        Destroy(gameObject);
        
        GameObject explosionEffect = Instantiate(ExplosionEffect.gameObject, transform.position, Quaternion.identity);

        Destroy(explosionEffect, 3);
    }
}
