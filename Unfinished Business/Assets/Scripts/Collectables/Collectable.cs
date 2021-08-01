using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    [Header("Management")]
    [SerializeField] string playerTag = "Player";

    [Header("Bounce")]
    [SerializeField] float maxBounceHeight = 0.25f;

    [Header("Animation")]
    [SerializeField] float despawnTime = 5f;
    [SerializeField]
    [Tooltip("This is how long the item blinks before despawning")]
    float graceTime = 2f;
    [SerializeField] int numBlinks = 3;
    [SerializeField] GameObject sprite;

    [Header("Tolerance")]
    [SerializeField]
    float pickupTolerance;
    float pickupTimer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag) && pickupTimer >= pickupTolerance)
        {
            PlayerHealth playerHP = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerHealth>();
            playerHP.Money += 1;
            DisableCollectable();
        }
    }

    // Called by CollectablePool to start the bouncing effect
    public void EnableCollectable (Vector2 position, float distance, float time)
    {
        pickupTolerance = time;
        sprite.SetActive(true);
        transform.position = position;
        gameObject.SetActive(true);
        Vector2 jumpDirection = PerspectiveUtilities.GetRandomVectorPerspectiveEllipse(randomMagnitude:false);
        jumpDirection *= distance;
        StartCoroutine(Jump(position + jumpDirection, time));
    }

    public void DisableCollectable ()
    {
        pickupTimer = 0;
        pickupTolerance = 0;
        gameObject.SetActive(false);
    }

    private IEnumerator Jump (Vector2 target, float time)
    {
        //Linearly interpolate to the target in the given amount of time

        float timePassed = 0;
        Vector2 start = transform.position;
        float jm = 1;
        float jt = 1;

        do
        {
            float yOffsetT = Mathf.Abs(target.x - transform.position.x) / Mathf.Abs(target.x - start.x);
            float yOffsetNormal = -Mathf.Pow(((2 * Mathf.Sqrt(jm)) / jt) * yOffsetT - Mathf.Sqrt(jm), 2) + jm;
            float yOffset = yOffsetNormal * maxBounceHeight;

            timePassed += Time.deltaTime;

            transform.position = Vector2.Lerp(start, target, timePassed / time);
            transform.position = new Vector2(transform.position.x, transform.position.y + yOffset);

            yield return null;
        } while (timePassed / time <= 1f);

        yield return new WaitForSeconds(despawnTime);
        float blinkTime = graceTime / numBlinks;
        for (int i = 0; i < numBlinks; i++)
        {
            sprite.SetActive(false);
            yield return new WaitForSeconds(blinkTime / 2f);
            sprite.SetActive(true);
            yield return new WaitForSeconds(blinkTime / 2f);
        }
        DisableCollectable();
    }

    private void Update()
    {
        if (pickupTimer < pickupTolerance)
            pickupTimer += Time.deltaTime;
    }
}
