using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : GameEntity
{
    [SerializeField] private float _speed;
    [SerializeField] private float _movTime;
    Vector3 _direction;
    Collider _collider;
    Character _attacker;
    AudioSource _audioSource;
    bool isMoving;
    float movTimer;

    string PLAYER = "Player";
    string OBSTACLE = "Obstacle";
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        Move();
    }
    private void OnEnable()
    {
        _audioSource.Play();
    }
    private void OnDisable()
    {
        _audioSource.Stop();
    }
    private void Move()
    {
        if (isMoving)
        {
            transform.position += _direction * _speed * Time.deltaTime;
            movTimer += Time.deltaTime;
            if (movTimer >= _movTime)
            {
                isMoving = false;
                SimplePool.PushToPool(this);
            }    
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != _collider)
        {
            if (!other.CompareTag(OBSTACLE))
            {
                Vector3 collision = other.ClosestPoint(transform.position);
                ComponentSaver.GetIDamage(other).Damage(_attacker, collision);
                if (_attacker.CompareTag(PLAYER))
                {
                    _attacker._goldReward += 5;
                }
                _attacker.SetRankText(1);
            }
            SimplePool.PushToPool(this);
        }
    }
    public void Throw(Vector3 direction, Collider collider, Character attacker)
    {
        _direction = direction;
        _collider = collider;
        _attacker = attacker;
        isMoving = true;
        movTimer = 0;
    }
}
