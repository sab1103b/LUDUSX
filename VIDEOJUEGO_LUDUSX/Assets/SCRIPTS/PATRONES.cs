using UnityEngine;

public class PATRONES : MonoBehaviour
{
    public enum MovementPattern
    {
        Oscilante,
        PicadaCurva,
        Cazador,
        ZigZag,
        Rodeo
    }

    [Header("Player Reference (Assign XR Origin Here)")]
    public Transform player; // 🔥 ARRASTRA EL XR ORIGIN AQUÍ
    private Transform playerHead;

    [Header("Pattern")]
    public MovementPattern pattern;

    [Header("General Settings")]
    public float forwardSpeed = 4f;
    public float amplitude = 2f;
    public float frequency = 2f;
    public float smoothFactor = 5f;
    public float zigZagInterval = 1.5f;
    public float orbitDistance = 4f;
    public float orbitSpeed = 60f;

    [Header("Limits")]
    public float xLimit = 10f;
    public float yMaxLimit = 6f;
    public float groundLimit = 0.5f;   // límite inferior (suelo)
    public float zMinLimit = -50f;
    public float zMaxLimit = 5f;

    private float timer;
    private int zigZagDirection = 1;

    void Start()
    {
        if (player != null)
        {
            playerHead = player.GetComponentInChildren<Camera>().transform;
        }
    }

    void Update()
    {
        if (playerHead == null) return;

        timer += Time.deltaTime;

        switch (pattern)
        {
            case MovementPattern.Oscilante:
                transform.position = Oscilante();
                break;

            case MovementPattern.PicadaCurva:
                transform.position = PicadaCurva();
                break;

            case MovementPattern.Cazador:
                transform.position = Cazador();
                break;

            case MovementPattern.ZigZag:
                transform.position = ZigZag();
                break;

            case MovementPattern.Rodeo:
                transform.position = Rodeo();
                break;
        }

        ApplyLimits();
    }

    // ==========================
    // PATRONES
    // ==========================

    Vector3 Oscilante()
    {
        float x = playerHead.position.x + Mathf.Sin(timer * frequency) * amplitude;
        float z = transform.position.z - forwardSpeed * Time.deltaTime;

        return new Vector3(x, transform.position.y, z);
    }

    Vector3 PicadaCurva()
    {
        float x = playerHead.position.x + Mathf.Sin(timer * frequency) * amplitude;

        float y = transform.position.y -
                  Mathf.Abs(Mathf.Sin(timer * frequency)) *
                  amplitude * Time.deltaTime;

        float z = transform.position.z - forwardSpeed * Time.deltaTime;

        return new Vector3(x, y, z);
    }

    Vector3 Cazador()
    {
        Vector3 direction = (playerHead.position - transform.position).normalized;

        // Reducimos influencia vertical para evitar mareo
        direction.y *= 0.4f;

        return transform.position + direction * forwardSpeed * Time.deltaTime;
    }

    Vector3 ZigZag()
    {
        if (timer >= zigZagInterval)
        {
            zigZagDirection *= -1;
            timer = 0;
        }

        float x = playerHead.position.x + zigZagDirection * amplitude;
        float z = transform.position.z - forwardSpeed * Time.deltaTime;

        return new Vector3(x, transform.position.y, z);
    }

    Vector3 Rodeo()
    {
        float distance = Vector3.Distance(transform.position, playerHead.position);

        if (distance > orbitDistance)
        {
            Vector3 direction = (playerHead.position - transform.position).normalized;
            direction.y *= 0.4f;

            return transform.position + direction * forwardSpeed * Time.deltaTime;
        }
        else
        {
            Vector3 offset = transform.position - playerHead.position;
            offset = Quaternion.Euler(0, orbitSpeed * Time.deltaTime, 0) * offset;

            return playerHead.position + offset;
        }
    }

    // ==========================
    // LÍMITES
    // ==========================

    void ApplyLimits()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, -xLimit, xLimit);

        if (pos.y < groundLimit)
            pos.y = groundLimit;

        pos.y = Mathf.Clamp(pos.y, groundLimit, yMaxLimit);

        pos.z = Mathf.Clamp(pos.z, zMinLimit, zMaxLimit);

        transform.position = pos;
    }
}