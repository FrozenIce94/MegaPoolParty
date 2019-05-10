using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bombe : MonoBehaviour
{

    public float timeToExplode = 5f;
    public float explosionDuration = 0.25f;
    public float explosionRadius = 25f;
    public GameObject explosionModel;
    public ParticleSystem explosionParticles;

    public BombermanPlayer playerInstance;

    private float explosionTimer;
    private bool exploded;

    // Start is called before the first frame update
    void Start()
    {
        explosionTimer = timeToExplode;
        explosionParticles.Stop();
        explosionModel.transform.localScale = Vector3.one * (explosionRadius * 0.5f);
        //explosionModel.SetActive(false);
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
        explosionParticles.transform.position = transform.position;
        explosionParticles.Play();
        yield return new WaitForSeconds(explosionDuration);
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
        Destroy(gameObject);
        playerInstance.bombAmount += 1;
    }
}
