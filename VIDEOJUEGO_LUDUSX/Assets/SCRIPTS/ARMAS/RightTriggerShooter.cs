using UnityEngine;
using UnityEngine.InputSystem;

public class RightTriggerShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzleTransform;
    public float bulletSpeed = 20f;
    public float fireRate = 0.25f;

    private InputAction triggerAction;
    private float nextFireTime = 0f;

    private void Awake()
    {
        triggerAction = new InputAction(
            name: "RightTrigger",
            type: InputActionType.Button,
            binding: "<XRController>{RightHand}/triggerPressed"
        );
    }

    private void OnEnable()
    {
        triggerAction.Enable();
    }

    private void OnDisable()
    {
        triggerAction.Disable();
    }

    private void Update()
    {
        if (triggerAction == null)
            return;

        if (triggerAction.IsPressed() && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("No hay bulletPrefab asignado");
            return;
        }

        if (muzzleTransform == null)
        {
            Debug.LogWarning("No hay muzzleTransform asignado");
            return;
        }

        Debug.Log("DISPARO");

        GameObject bullet = Instantiate(
            bulletPrefab,
            muzzleTransform.position,
            muzzleTransform.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = muzzleTransform.forward * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("La bala no tiene Rigidbody");
        }
    }
}