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
    [Tooltip("Maximum number of bullets allowed in the scene")][SerializeField] private int maxBullets = 10;

    private Queue<GameObject> bulletPool;
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

        
        bulletPool = new Queue<GameObject>();
        for (int i = 0; i < maxBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    void Update()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
            
            gunAnimator.SetTrigger("Fire");
        }
    }

    
    void Shoot()
    {
        if (muzzleFlashPrefab)
        {
            
            GameObject tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            
            Destroy(tempFlash, destroyTimer);
        }

        
        if (!bulletPrefab)
            return;

        
        GameObject bullet = bulletPool.Dequeue();
        bullet.transform.position = barrelLocation.position;
        bullet.transform.rotation = barrelLocation.rotation;
        bullet.SetActive(true);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = Vector3.zero; 
            bulletRb.angularVelocity = Vector3.zero; 
            bulletRb.AddForce(barrelLocation.forward * shotPower);
        }
        else
        {
            Debug.LogWarning("Bullet prefab does not have a Rigidbody component.");
        }

        
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range, hitLayers))
        {
            EnemyHP_EnemyDeath target = hit.transform.GetComponent<EnemyHP_EnemyDeath>();
            if (target != null)
            {
                target.TakeDamage(damage);

                
                string hitPart = hit.collider.tag;
                int points = 0;

                switch (hitPart)
                {
                    case "EnemyHead":
                        points = target.pointsForHeadshot;
                        Debug.Log("Headshot");
                        break;
                    case "EnemyBody":
                        points = target.pointsForBodyshot;
                        Debug.Log("Body");
                        break;
                    case "EnemyArm":
                        points = target.pointsForLimbshot;
                        Debug.Log("Limb");
                        break;
                    case "EnemyLeg":
                        points = target.pointsForLimbshot;
                        Debug.Log("LimbLeg");
                        break;
                    default:
                        points = target.pointsForBodyshot;
                        Debug.Log("LOL");
                        break;
                }

                if (scoreManager != null)
                {
                    scoreManager.AddPoints(points);
                }
            }
        }

        
        StartCoroutine(DeactivateBulletAfterTime(bullet, destroyTimer));
    }

    private IEnumerator DeactivateBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }

    
    void CasingRelease()
    {
        
        if (!casingExitLocation || !casingPrefab)
            return;

        
        GameObject tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation);

        
        Rigidbody casingRb = tempCasing.GetComponent<Rigidbody>();
        if (casingRb != null)
        {
            casingRb.AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower),
                                       casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f,
                                       1f);

            
            casingRb.AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

            
            Destroy(tempCasing, destroyTimer);
        }
        else
        {
            Debug.LogWarning("Casing prefab does not have a Rigidbody component.");
        }
    }
}