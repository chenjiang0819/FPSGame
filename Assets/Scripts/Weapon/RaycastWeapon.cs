using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform raycastOrigin;
    [SerializeField] Animator rigController;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject muzzleFlash;

    [Header("Weapon Properties")]
    [SerializeField] int fireRate = 25;
    [SerializeField] float bulletSpeed = 1000.0f;
    [SerializeField] float verticalRecoil = 1f;
    [SerializeField] float horizontalRecoil = 1f;
    [SerializeField] float minDamage = 5f;
    [SerializeField] float maxDamage = 10f;
    [SerializeField] float criticalRate = .3f;
    [SerializeField] AudioClip gunShot;

    PlayerController controller;
    Transform mainCam;
    WeaponRecoil cameraRecoil;

    bool isFiring = false;
    Ray ray;
    RaycastHit hit;
    float accumulatedTime;
    AudioSource audioSource;

    private void Start()
    {
        mainCam = SceneManager.Instance.mainCam.transform;
        cameraRecoil = SceneManager.Instance.cameraSpring.GetComponent<WeaponRecoil>();
        controller = SceneManager.Instance.player.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isFiring && controller.IsFiring)
            StartFiring();

        if (isFiring && !controller.IsFiring)
            StopFiring();

        if (isFiring)
            UpdateFiring();
    }

    public void StartFiring()
    {
        isFiring = true;
        accumulatedTime = 0f;
    }

    private void UpdateFiring()
    {
        accumulatedTime += Time.deltaTime;
        float fireInterval = 1.0f / fireRate;

        while (accumulatedTime >= 0f)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    private void FireBullet()
    {
        // audioSource.Stop();
        audioSource.PlayOneShot(gunShot);
        GameObject muzzleFlashVFX = Instantiate(muzzleFlash, raycastOrigin.position, raycastOrigin.rotation) as GameObject;
        muzzleFlashVFX.transform.localScale = new Vector3(.5f, .5f, .5f);
        muzzleFlashVFX.transform.parent = raycastOrigin.transform;

        ray.origin = mainCam.position;
        ray.direction = mainCam.forward;

        if (!Physics.Raycast(ray, out hit))
            hit.point = ray.origin + ray.direction * 1000f;
        else
            print(hit.collider.gameObject);

        Vector3 velocity = (hit.point - raycastOrigin.position).normalized * bulletSpeed;

        GameObject bullet = Instantiate(bulletPrefab, raycastOrigin.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetProperties(minDamage, maxDamage, criticalRate, true);
        bullet.GetComponent<Rigidbody>().AddForce(velocity);

        Recoil();
    }

    private void Recoil()
    {
        rigController.Play("RifleRecoil", 0, 0.0f);

        cameraRecoil.IsRecoiling = true;
        cameraRecoil.vertical = Random.Range(verticalRecoil / 2, verticalRecoil);
        cameraRecoil.horizontal = Random.Range(-horizontalRecoil, horizontalRecoil);
        cameraRecoil.time = 1.0f / fireRate * 0.8f;
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class RaycastWeapon : MonoBehaviour
// {
//     class Bullet
//     {
//         public float time;
//         public Vector3 initialPosition;
//         public Vector3 initialVelocity;
//     }

//     [Header("References")]
//     [SerializeField] Transform raycastOrigin;
//     [SerializeField] Transform mainCam;
//     [SerializeField] WeaponRecoil cameraRecoil;
//     [SerializeField] Animator rigController;
//     [SerializeField] LayerMask layers;

//     [Header("Weapon Properties")]
//     [SerializeField] int fireRate = 25;
//     [SerializeField] float bulletSpeed = 1000.0f;
//     [SerializeField] float bulletDrop = 0.0f;
//     [SerializeField] float maxLifeTime = 3.0f;
//     [SerializeField] ParticleSystem bulletImpact;
//     [SerializeField] float verticalRecoil = 1f;
//     [SerializeField] float horizontalRecoil = 1f;
//     [SerializeField] float minDamage = 5f;
//     [SerializeField] float maxDamage = 10f;
//     [SerializeField] float criticalRate = .3f;

//     public TrailRenderer trail;

//     PlayerController controller;
//     bool isFiring = false;
//     Ray ray;
//     RaycastHit hit;
//     float accumulatedTime;
//     List<Bullet> bullets = new List<Bullet>();
//     public AudioClip gunShot;
//     AudioSource audioSource;

//     Vector3 GetPosition(Bullet bullet)
//     {
//         Vector3 gravity = Vector3.down * bulletDrop;
//         return (bullet.initialPosition) +
//                (bullet.initialVelocity * bullet.time) +
//                (.5f * gravity * bullet.time * bullet.time);
//     }

//     Bullet CreateBullet(Vector3 position, Vector3 velocity)
//     {
//         Bullet bullet = new Bullet();
//         bullet.initialPosition = position;
//         bullet.initialVelocity = velocity;
//         bullet.time = 0.0f;
//         return bullet;
//     }

//     private void Start()
//     {
//         controller = GetComponentInParent<PlayerController>();
//         audioSource = GetComponent<AudioSource>();
//     }

//     private void Update()
//     {
//         if (!isFiring && controller.IsFiring)
//             StartFiring();

//         if (isFiring && !controller.IsFiring)
//             StopFiring();

//         if (isFiring)
//             UpdateFiring();

//         UpdateBullet();
//     }

//     public void StartFiring()
//     {
//         isFiring = true;
//         accumulatedTime = 0f;
//     }

//     private void UpdateFiring()
//     {
//         accumulatedTime += Time.deltaTime;
//         float fireInterval = 1.0f / fireRate;

//         while (accumulatedTime >= 0f)
//         {
//             FireBullet();
//             accumulatedTime -= fireInterval;
//         }
//     }

//     private void FireBullet()
//     {
//         audioSource.Stop();
//         audioSource.PlayOneShot(gunShot);

//         ray.origin = mainCam.position;
//         ray.direction = mainCam.forward;

//         var tracer = Instantiate(trail, raycastOrigin.position, Quaternion.identity);
//         tracer.AddPosition(raycastOrigin.position);

//         if (!Physics.Raycast(ray, out hit))
//         {
//             hit.point = ray.origin + ray.direction * 1000f;
//             tracer.transform.position = hit.point;
//         }

//         Vector3 velocity = (hit.point - raycastOrigin.position).normalized * bulletSpeed;
//         var bullet = CreateBullet(raycastOrigin.position, velocity);
//         bullets.Add(bullet);

//         Recoil();
//     }

//     private void Recoil()
//     {
//         rigController.Play("RifleRecoil", 0, 0.0f);

//         cameraRecoil.IsRecoiling = true;
//         cameraRecoil.vertical = Random.Range(verticalRecoil / 2, verticalRecoil);
//         cameraRecoil.horizontal = Random.Range(-horizontalRecoil, horizontalRecoil);
//         cameraRecoil.time = 1.0f / fireRate * 0.8f;
//     }

//     private void UpdateBullet()
//     {
//         SimulateBullets(Time.deltaTime);
//         DestoryBullets();
//     }

//     private void DestoryBullets()
//     {
//         bullets.RemoveAll(bullet => bullet.time >= maxLifeTime);
//     }

//     private void SimulateBullets(float deltaTime)
//     {
//         bullets.ForEach(bullet =>
//         {
//             Vector3 p0 = GetPosition(bullet);
//             bullet.time += deltaTime;
//             Vector3 p1 = GetPosition(bullet);
//             RaycastSegment(p0, p1, bullet);
//         });
//     }

//     private void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
//     {
//         Vector3 direction = end - start;
//         float distance = direction.magnitude;
//         ray.origin = start;
//         ray.direction = direction;

//         if (Physics.Raycast(ray, out hit, distance, layers))
//         {
//             // bulletImpact.transform.position = hit.point;
//             // bulletImpact.transform.forward = hit.normal;
//             // bulletImpact.Emit(1);
//             // bullet.time = maxLifeTime;
//             Instantiate(bulletImpact, hit.point, Quaternion.identity);

//             var health = hit.collider.GetComponentInParent<HealthSystem>();
//             if (health) health.TakeDamage(minDamage, maxDamage, criticalRate, hit.collider.tag);
//         }
//     }

//     public void StopFiring()
//     {
//         isFiring = false;
//     }
// }
