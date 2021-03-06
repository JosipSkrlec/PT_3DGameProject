using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int _playerLifes = 3;
    [SerializeField] private float _playerSpeed = 2.5f;
    [SerializeField] private float _playerDamage = 25.0f;
    [SerializeField] private float _playerHealth = 200;
    [SerializeField] private float _attackRadius = 0.55f;
    [SerializeField] private float _jumpForce = 4.0f;
    [Space(5)]
    [Header("References")]
    [SerializeField] private Transform _thisTR;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private PostProcessingController _volumeController;
    [Space(5)]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _attackPoint;

    private float _playerMaxHealth;

    // Start is called before the first frame update
    void Start()
    {
        _playerMaxHealth = _playerHealth;
        PlayerUIController.Instance.UpdateLife(_playerLifes);
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
            transform.position += new Vector3(0.0f, 0.0f, _playerSpeed / 2 * Time.deltaTime);

        }

        // TODO - disable multi attack in the same time !.. do cooldown!
        if (Input.GetKeyDown(KeyCode.C))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Todo - remove double jump,fix !!
            Debug.Log("JUMP!");
            
            //_rigidbody.velocity = new Vector3(0.0f, _jumpForce, 0.0f);

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
                // TODO - add TakeDamage to animation as Event!
                StartCoroutine(TakeDamageAfterDelay(0.5f, coll.GetComponent<Rigidbody>()));
                //coll.GetComponent<Rigidbody>().AddForce(transform.forward * 150f);
            }
        }
        //#endregion // TODO
    }

    // this delay is temporary here just to test it !
    private IEnumerator TakeDamageAfterDelay(float delay, Rigidbody enemyRB)
    {
        yield return new WaitForSeconds(delay);

        EnemyController enemy = enemyRB.GetComponent<EnemyController>();

        enemy.TakeDamageToEnemy(_playerDamage);

        // TODO - do Score system !
        ScoreController.Instance.AppendScore((int)Random.Range(100.0f, 200.0f));

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

    public void TakeDamageToPlayer(float damage)
    {
        _playerHealth -= damage;

        PlayerUIController.Instance.UpdateHealth(_playerMaxHealth, _playerHealth);


        if (_playerHealth <= 0.0f)
        {
            _playerLifes -= 1;

            if (_playerLifes <= 0)
            {
                Debug.Log("Player if death!");

            }
            else
            {
                Debug.Log("Take life to plyer!");
                PlayerUIController.Instance.UpdateLife(_playerLifes);
                _playerHealth = _playerMaxHealth;

            }

        }
        else if (_playerHealth <= 50.0f)
        {
            _volumeController.AnimateLowHealthIndicator();
        }
    }

    public void HealThePlayer(float healAmount)
    {
        _playerHealth += healAmount;

        if (_playerHealth > _playerMaxHealth)
        {
            _playerHealth = _playerMaxHealth;
        }


        PlayerUIController.Instance.UpdateHealth(_playerMaxHealth, _playerHealth);
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
