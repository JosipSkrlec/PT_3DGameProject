using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Healing : MonoBehaviour
{
    [SerializeField] private float _healAmount;
    [Space(5)]
    [SerializeField] private VisualEffect _healingEffect;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerController>().HealThePlayer(_healAmount);
            // TODO - play healing sound!
            Destroy(this.gameObject);
        }
    }
}
