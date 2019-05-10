using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bombe : MonoBehaviour
{

    public float timeToExplode = 5f;
    public float explosionDuration = 0.25f;
    public float explosionRadius = 25f;
    public GameObject[] explosionModels;

    private float explosionTimer;
    private bool exploded;

    // Start is called before the first frame update
    void Start()
    {
        explosionTimer = timeToExplode;
        foreach (var explosionModel in explosionModels)
        {
            explosionModel.transform.localScale = Vector3.one * explosionRadius;
            explosionModel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        explosionTimer -= Time.deltaTime;
        if (explosionTimer <= 0f && !exploded)
        {
            exploded = true;

            var hitObjects = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (var collider in hitObjects)
            {
                var player = collider.GetComponent<BombermanPlayer>();
                if (player != null)
                {
                    player.Hit();
                }

            }
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        foreach (var explosionModel in explosionModels)
        {
            explosionModel.SetActive(true);
        }
        yield return new WaitForSeconds(explosionDuration);
        Destroy(gameObject);
    }
}
