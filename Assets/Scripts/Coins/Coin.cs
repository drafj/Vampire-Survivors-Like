using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Coin : RecyclableObject
{
    private float timeTillDisapear = 5;
    private SpriteRenderer image;
    private Coroutine flickerCo;
    private Coroutine disableCo;
    private IStats stats;

    private void Awake()
    {
        stats = FindObjectsOfType<MonoBehaviour>().OfType<IStats>().FirstOrDefault();
        image = GetComponent<SpriteRenderer>();
    }

    public override void Init()
    {
        disableCo = StartCoroutine(DisapearCO());
        flickerCo = StartCoroutine(Flicker());
    }

    public override void Release()
    {
        base.Release();
        image.enabled = true;
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
        Recycle();
    }

    private IEnumerator Flicker()
    {
        yield return new WaitForSeconds(timeTillDisapear / 2);
        while (true)
        {
            image.enabled = false;
            yield return new WaitForSeconds(0.25f);
            image.enabled = true;
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
