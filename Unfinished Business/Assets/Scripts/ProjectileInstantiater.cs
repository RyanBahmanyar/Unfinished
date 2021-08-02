using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInstantiater : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private PlayerHealth health;

    public void SpawnProjectile()
    {
        if (health.Money > 1)
        {
            health.Money -= 1;

            GameObject instance = Instantiate(projectile, transform.position, Quaternion.Euler(Vector3.zero));
            PlayerProjectile controller = instance.GetComponent<PlayerProjectile>();

            Vector3 direction = transform.right;

            if (!playerController.FacingRight())
            {
                direction *= -1;
            }

            controller.SetDirection(direction);
        }
    }
}
