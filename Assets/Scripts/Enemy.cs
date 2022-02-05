using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 0f;
    [SerializeField] float respawnTime = 5f;
    [SerializeField] float minSpawnDistance = -10f;
    [SerializeField] float maxSpawnDistance = 10f;
    [SerializeField] float timeToIncreaseSpeed = 10f;
    [SerializeField] float speedIncrease = 0.1f;
    [SerializeField] ParticleSystem destroyParticles = null;

    float timer;
    Transform player;
    MeshRenderer meshRenderer;
    Collider meshCollider;
    AudioSource audioSource;
    PlayerFollower playerFollower;
    PlayerStats playerStats;

    public bool IsEnemyEnabled { get => meshRenderer.enabled; }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
        audioSource = GetComponent<AudioSource>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<Collider>();
        playerFollower = Camera.main.GetComponent<PlayerFollower>();

        Invoke("Spawn", UnityEngine.Random.Range(1, respawnTime));

        playerStats.onDead += DisableMovement;
    }

    private void Update()
    {
        FollowPlayer();
        IncreaseDifficulty();
    }

    private void FollowPlayer()
    {
        if (!meshRenderer.enabled) { return; }

        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void IncreaseDifficulty()
    {
        timer += Time.deltaTime;
        if (timer >= timeToIncreaseSpeed)
        {
            speed += speedIncrease;
            timer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") || other.CompareTag("Player"))
        {
            if (other.transform != player) { other.gameObject.SetActive(false); };
            PlayDestroyEffect();
        }
    }

    public void PlayDestroyEffect()
    {
        if (!meshRenderer.enabled) { return; }

        audioSource.Play();
        destroyParticles.Play();
        SetEnemyEnabled(false);
        playerFollower.IncreaseShakeTimer();

        Invoke("Spawn", UnityEngine.Random.Range(1, respawnTime));
    }

    private void Spawn()
    {
        transform.position = CalculateSpawnPoint();
        SetEnemyEnabled(true);
    }

    private void SetEnemyEnabled(bool isEnabled)
    {
        meshRenderer.enabled = isEnabled;
        meshCollider.enabled = isEnabled;
    }

    private Vector3 CalculateSpawnPoint()
    {
        Vector3 respawnPoint = player.position;
        respawnPoint.x += UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);
        respawnPoint.z += UnityEngine.Random.Range(maxSpawnDistance / 2, maxSpawnDistance);
        respawnPoint.y = 1;
        return respawnPoint;
    }

    private void DisableMovement()
    {
        CancelInvoke();
        enabled = false;
    }
}