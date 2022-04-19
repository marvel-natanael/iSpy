using System.Collections;
using Mirror;
using Player;
using UnityEngine;

public class CircleShrink : NetworkBehaviour
{
    bool isOutside = false;
    [SerializeField]
    float minSize, shrinkMultiplier, damage = 1, timer;
    bool isShrinked = false;
    PlayerManager playerToDamage = null;

    void Update()
    {
        timer = 0;
        if (isOutside)
        {
            if (playerToDamage != null && Time.time >= timer)
            {
                Damage(playerToDamage);
                timer = Time.time + 1.0f;
            }
        }
        else
        {
            timer = 0;
        }
        if (!isShrinked)
            StartCoroutine(Shrink());
    }

    void Damage(PlayerManager player)
    {
        player.TakeDamage(damage);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOutside = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isOutside = true;
            var player = col.gameObject.GetComponent<PlayerManager>();
            if (player.isLocalPlayer)
                playerToDamage = player;
        }
    }


    IEnumerator Shrink()
    {
        Vector3 minScale = new Vector3(minSize, minSize);
        Vector3 startScale = transform.localScale;
        float timer = 0f;
        while (transform.localScale.x > minSize)
        {
            transform.localScale = Vector3.Lerp(startScale, minScale, timer / shrinkMultiplier / 50f);
            timer += Time.deltaTime;
            yield return null;
        }
        isShrinked = true;
    }
}
