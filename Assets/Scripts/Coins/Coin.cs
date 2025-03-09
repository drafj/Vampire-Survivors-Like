using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    private float timeTillDisapear = 5;
    private MeshRenderer meshRenderer;
    private Coroutine flickerCo;
    private Coroutine disableCo;
    private IStats stats;

    private void Awake()
    {
        stats = FindObjectsOfType<MonoBehaviour>().OfType<IStats>().FirstOrDefault();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        disableCo = StartCoroutine(DisapearCO());
        flickerCo = StartCoroutine(Flicker());
    }

    private void OnDisable()
    {
        meshRenderer.enabled = true;
        StopCoroutine(disableCo);
        StopCoroutine(flickerCo);
    }

    private IEnumerator DisapearCO()
    {
        yield return new WaitForSeconds(timeTillDisapear);
        Disapear();
    }

    private void Disapear()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator Flicker()
    {
        yield return new WaitForSeconds(timeTillDisapear / 2);
        while (true)
        {
            meshRenderer.enabled = false;
            yield return new WaitForSeconds(0.25f);
            meshRenderer.enabled = true;
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Saw") || collision.gameObject.CompareTag("Player"))
        {
            Disapear();
            stats.AddCoins();
        }
    }
}
