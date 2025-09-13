using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    //private float rotateOffset = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootDelay = 0.1f;
    [SerializeField] private float bulletForce = 15f;
    private float nextShot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotateGun();
        Shoot();
    }

    void Shoot()
    {
        if(Input.GetMouseButtonDown(0) && Time.time > nextShot)
        {
            nextShot = Time.time + shootDelay;
            var bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if(bulletRb != null)
            {
                AudioManager.Instance.PlaySFX("Range");
                bulletRb.AddForce(firePos.up * bulletForce, ForceMode2D.Impulse);
            }
        }
    }

    void RotateGun()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || 
            Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            return;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 direc = mousePos - transform.position;
        float angle = Mathf.Atan2(direc.y, direc.x) * Mathf.Rad2Deg - 90f;
        firePos.rotation = Quaternion.Euler(0, 0, angle);
    }
}
