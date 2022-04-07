using System;
using System.Collections;
using NaughtyAttributes;
using Supyrb;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    [SerializeField] private float torqueSpeed;
    [SerializeField] private float forcePower;
    [SerializeField] [Tag] private string boundsTag;
    [SerializeField] [Tag] private string asteroidTag;
    [SerializeField] [Tag] private string ufoTag;
    [SerializeField] private GameObject laser;
    [SerializeField] private float laserChargeTime;
    [SerializeField] private int maxLaserCharges=3;
    [SerializeField] private float boundaryDistance = 1.1f;
    private int currentCharges=0;
    private UpdateLaserBarSignal updateLaserBarSignal;
    private SetShipDataSignal setShipDataSignal;
    private Rigidbody2D rb;
    private float direction;
    private bool thrusting;

    private void Awake()
    {
        setShipDataSignal = Signals.Get<SetShipDataSignal>();
        updateLaserBarSignal = Signals.Get<UpdateLaserBarSignal>();
        Signals.Get<RestartSignal>().AddListener(Restart);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(Charge());
    }

    IEnumerator Charge()
    {
        float time = laserChargeTime;
        while (time > 0)
        {
            updateLaserBarSignal.Dispatch(time,laserChargeTime,currentCharges);
            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        
        if (currentCharges < maxLaserCharges)
        {
            currentCharges++;
            StartCoroutine(Charge());
        }
    }

    void Restart()
    {
        currentCharges = 0;
        transform.position = Vector3.zero;
        transform.rotation = new Quaternion(0,0,0,0);
        this.gameObject.SetActive(true);
        StartCoroutine(Charge());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(ufoTag) || other.gameObject.CompareTag(asteroidTag))
        {
            other.gameObject.SetActive(false);
            StopAllCoroutines();
            Signals.Get<LoseSignal>().Dispatch();
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(boundsTag))
        {
            Vector3 boundPos = other.gameObject.transform.position;
            Vector3 playerPos = transform.position;
            Vector3 offset=Vector3.one;
            if (Math.Abs(playerPos.y - boundPos.y) <= boundaryDistance&& boundPos.y!=0)
            {
                offset = Vector3.up*(transform.position.y) * 2;
            }else if (Math.Abs(playerPos.x - boundPos.x) <= boundaryDistance&& boundPos.x!=0)
            {
                offset = Vector3.right*(transform.position.x) * 2;
            }
            transform.position -= offset;
        }
    }

    void Update()
    {
        thrusting=Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        if (Input.GetKeyDown(KeyCode.F)&& currentCharges>0)
        {
            if (currentCharges == maxLaserCharges)
            {
                StartCoroutine(Charge());
            }
            currentCharges--;
            Signals.Get<ApplyLaserSignal>().Dispatch(transform);
        }else if (Input.GetKeyDown(KeyCode.Space))
        {
            Signals.Get<ShootSignal>().Dispatch();
        }
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            direction=Input.GetAxis("Horizontal");
        }
        else
        {
            direction = 0;
        }
    }

    private void FixedUpdate()
    {
        setShipDataSignal.Dispatch(transform.position,transform.rotation.eulerAngles,rb.velocity);
        if (thrusting)
        {
            rb.AddForce(forcePower*transform.up);
        }
        if (direction != 0)
        {
            rb.AddTorque(direction*-torqueSpeed);
        }
    }
}
