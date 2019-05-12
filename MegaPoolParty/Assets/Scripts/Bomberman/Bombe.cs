using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bombe : MonoBehaviour
{

    public float timeToExplode = 5f;
    public float explosionDuration = 0.25f;
    public float explosionRadius = 25f;
    public float fallForce = 300f;
    public GameObject explosionModel;
    public GameObject bombModel;
    public ParticleSystem explosionParticles;
    public AudioSource explosionSound;

    public BombermanPlayer playerInstance;

    private float explosionTimer;
    private bool exploded;


    private List<Collider> ignoredCollisions = new List<Collider>();
    // Start is called before the first frame update
    void Start()
    {
        explosionTimer = timeToExplode;
        explosionParticles.Stop();
        explosionModel.transform.localScale = Vector3.one * (explosionRadius * 0.5f);

    }


    // Update is called once per frame
    void Update()
    {
        explosionTimer -= Time.deltaTime;
        if (explosionTimer <= 0f && !exploded)
        {
            exploded = true;


            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        bombModel.SetActive(false);
        explosionParticles.transform.position = transform.position;
        explosionParticles.Play();
        yield return new WaitForSeconds(explosionDuration);
        StartCoroutine(ExplosionSound());
        var hitObjects = Physics.OverlapSphere(explosionParticles.transform.position, explosionRadius * 0.6f);
        foreach (var collider in hitObjects)
        {
            var firstHit = false;
            var player = collider.GetComponent<BombermanPlayer>();
            if (player != null && !firstHit)
            {
                Debug.Log($"Der {(player.IsStudent ? "Lehrer" : "Schüler")} hat gewonnen");
                player.Hit();
                firstHit = true;
            }
        }
        playerInstance.IncreaseBombs();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Regal")
        {
            var ownCollider = GetComponent<Collider>();
            Physics.IgnoreCollision(ownCollider, collision.collider);
            ignoredCollisions.Add(collision.collider);
        }
    }

    private IEnumerator ExplosionSound()
    {
        explosionSound.Play();
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

}
