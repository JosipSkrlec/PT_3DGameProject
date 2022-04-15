using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _playerSpeed = 2.5f;
    [SerializeField] private float _playerDamage = 25.0f;
    [Space(5)]
    [SerializeField] private Transform _thisTR;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [Space(5)]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRadius;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float mH = Input.GetAxis("Horizontal");
        float mV = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.A))
        {
            _thisTR.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
            _animator.SetTrigger("Walking");
            //_rigidbody.AddForce();            
            _rigidbody.velocity = new Vector3(-(mH * _playerSpeed), 0.0f, 0.0f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _thisTR.eulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
            _animator.SetTrigger("Walking");
            //_rigidbody.AddForce();            
            _rigidbody.velocity = new Vector3(-(mH * _playerSpeed), 0.0f, 0.0f);
        }
        else
        {
            _animator.SetTrigger("Idle");
        }

        if (Input.GetKey(KeyCode.W))
        {
            //_thisTR.eulerAngles = new Vector3(0.0f, -180.0f, 0.0f);
            //_animator.SetTrigger("Walking");
            //_rigidbody.AddForce();            
            //_rigidbody.velocity = new Vector3(0.0f, 0.0f, -(mV * _playerSpeed));
           // transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + _playerSpeed * Time.deltaTime);
            transform.position += new Vector3(0.0f, 0.0f, -(_playerSpeed / 2 * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //_thisTR.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            //_animator.SetTrigger("Walking");
            //_rigidbody.AddForce();            
            //_rigidbody.velocity = new Vector3(0.0f, 0.0f, -(mV * _playerSpeed));
            transform.position += new Vector3(0.0f, 0.0f, _playerSpeed/2 * Time.deltaTime);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private void Attack()
    {
        int random = Random.Range(0, 2);
        switch (random)
        {
            case 0:
                _animator.SetTrigger("Punch");
                break;

            case 1:

                _animator.SetTrigger("LegKick");
                break;
        }

        //#region put it on Animation Event!
        Collider[] colliders = Physics.OverlapSphere(_attackPoint.position, _attackRadius, _layerMask);

        foreach (Collider coll in colliders)
        {
            if (coll != null)
            {
                Debug.Log(coll.gameObject.name);
                StartCoroutine(MoveAfterDelay(0.5f, coll.GetComponent<Rigidbody>()));
                //coll.GetComponent<Rigidbody>().AddForce(transform.forward * 150f);
            }
        }
        //#endregion // TODO
    }

    private IEnumerator MoveAfterDelay(float delay, Rigidbody enemyRB)
    {
        yield return new WaitForSeconds(delay);

        Enemy enemy = enemyRB.GetComponent<Enemy>();

        enemy.TakeDamage(_playerDamage);

        // TODO - show on UI!


        // TODO - do different, this is just testing for mass changed!
        //enemyRB.AddForce(transform.forward * 150f);
        //enemyRB.mass -= 0.25f;

        //if (enemyRB.mass <= 0.1)
        //{
        //    Debug.Log("Destroying " + enemyRB.gameObject.name);
        //    Destroy(enemyRB.gameObject);
        //}
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
        {
            return;
        }

        Gizmos.DrawSphere(_attackPoint.position, _attackRadius);
    }
}
