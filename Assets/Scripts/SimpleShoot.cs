using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location References")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destroy the casing object")][SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")][SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")][SerializeField] private float ejectPower = 150f;
    [Tooltip("Damage dealt by the gun")][SerializeField] private float damage = 100f;
    [Tooltip("Maximum distance for the gun to be effective")][SerializeField] private float range = 100f;
    [Tooltip("Layer mask to filter what the gun can hit")][SerializeField] private LayerMask hitLayers;

    private Camera fpsCamera;
    private ScoreManager scoreManager;

    void Start()
    {
        fpsCamera = GetComponentInParent<Camera>();
        scoreManager = FindObjectOfType<ScoreManager>();

        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // If you want a different input, change it here
        if (Input.GetButtonDown("Fire1"))
        {
            // Calls animation on the gun that has the relevant animation events that will fire
            gunAnimator.SetTrigger("Fire");
        }
    }

    // This function creates the bullet behavior
    void Shoot()
    {
        if (muzzleFlashPrefab)
        {
            // Create the muzzle flash
            GameObject tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            // Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        // Cancels if there's no bullet prefab
        if (!bulletPrefab)
            return;

        // Create a bullet and add force on it in direction of the barrel
        Rigidbody bulletRb = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.AddForce(barrelLocation.forward * shotPower);
        }
        else
        {
            Debug.LogWarning("Bullet prefab does not have a Rigidbody component.");
        }

        // Handle bullet hit detection and damage
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range, hitLayers))
        {
            EnemyHP_EnemyDeath target = hit.transform.GetComponent<EnemyHP_EnemyDeath>();
            if (target != null)
            {
                target.TakeDamage(damage);

                // Assign points based on hit location
                string hitPart = hit.collider.tag;
                int points = 0;

                switch (hitPart)
                {
                    case "EnemyHad":
                        points = target.pointsForHeadshot;
                        break;
                    case "EnemyBody":
                        points = target.pointsForBodyshot;
                        break;
                    case "EnemyArm":
                        points = target.pointsForLimbshot;
                        break;
                    case "EnemyLeg":
                        points = target.pointsForLimbshot;
                        break;
                    default:
                        points = target.pointsForBodyshot; // Default to body points if no specific tag is found
                        break;
                }

                if (scoreManager != null)
                {
                    scoreManager.AddPoints(points);
                }
            }
        }
    }

    // This function creates a casing at the ejection slot
    void CasingRelease()
    {
        // Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
            return;

        // Create the casing
        GameObject tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation);

        // Add force on casing to push it out
        Rigidbody casingRb = tempCasing.GetComponent<Rigidbody>();
        if (casingRb != null)
        {
            casingRb.AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower),
                                       casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f,
                                       1f);

            // Add torque to make casing spin in random direction
            casingRb.AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

            // Destroy casing after X seconds
            Destroy(tempCasing, destroyTimer);
        }
        else
        {
            Debug.LogWarning("Casing prefab does not have a Rigidbody component.");
        }
    }
}