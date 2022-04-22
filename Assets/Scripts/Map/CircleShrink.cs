using System.Collections;
using Mirror;
using Player;
using UnityEngine;

public class CircleShrink : NetworkBehaviour
{
    bool isOutside = false;
    [SerializeField]
    float minSize, shrinkMultiplier, damage = 1;
    bool isShrinked = false, takeDamage = true;
    PlayerManager playerToDamage = null;

    void Update()
    {
        if (isOutside)
        {
            if (playerToDamage != null && takeDamage)
            {
                StartCoroutine(delay());
                Damage(playerToDamage);
            }
        }
        if (!isShrinked)
            StartCoroutine(Shrink());
    }

    IEnumerator delay()
    {
        takeDamage = false;
        yield return new WaitForSeconds(1);
        takeDamage = true;
    }

    void Damage(PlayerManager player)
    {
        player.TakeDamage(damage);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isOutside = false;
            var player = col.gameObject.GetComponent<PlayerManager>();
            if (player.isLocalPlayer)
                playerToDamage = player;
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
