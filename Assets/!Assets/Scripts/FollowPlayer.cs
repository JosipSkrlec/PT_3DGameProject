using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private float _lerpSpeed = 0.25f;
    [SerializeField] private Transform _player;

    private Transform thisTR;


    private void Awake()
    {
        thisTR = this.GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        thisTR.position = new Vector3(Mathf.Lerp(this.transform.position.x, _player.transform.position.x, _lerpSpeed),thisTR.position.y, thisTR.position.z);
    }
}
