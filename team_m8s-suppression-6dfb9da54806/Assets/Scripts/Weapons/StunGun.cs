using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StunGun : MonoBehaviour {
    [SerializeField]
    private float offset;//tweaks weapon rotation
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject fireBall;
    [SerializeField]
    private Transform point;
    [SerializeField]
    private Image crosshairs;

    [SerializeField]
    private float magazineCapacity;
    [SerializeField]
    private float ammo;
    [SerializeField]
    private float reloadTime;
    private float reloadStart;
    private bool reloading;

    private float timeBtwShots;
    [SerializeField]
    private float startTimeBtwShots;

    [SerializeField]
    private Text ammoTracker;
    [SerializeField]
    private Text reloadTracker;

    private bool isFireBall = false;
    [SerializeField]
    private float coolDown;
   

    // Use this for initialization
    void Start()
    {

        ammo = magazineCapacity;
        reloading = false;
        reloadTracker.enabled = false;
        

        Cursor.visible = false;     // sets cursor visibilty to false
        crosshairs.transform.position = Input.mousePosition;
        ammoTracker.text = ammo + "/ " + magazineCapacity;      // updates ammo tracker GUI
    }

    // Update is called once per frame
    void Update ()
    {
        /*
        if (Input.GetKeyDown(KeyCode.R) && coolDown == 0)
        {
            isFireBall = true;
            ammo = 3;
            magazineCapacity = 3;
            if (ammo == 0 && isFireBall)
            {
                isFireBall = false;
                
            }
        }
        */

        crosshairs.transform.position = Input.mousePosition;        // sets crosshair position to the mouse position
        ammoTracker.text = ammo + "/ " + magazineCapacity;
        
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; //recieves direction
        float rotZ = Mathf.Atan2(-difference.x, difference.y) * Mathf.Rad2Deg; //get degrees of rotation
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);//preforms rotation
        

        if (timeBtwShots <= 0)
        {
            //If right click and time isn't stopped
            if (Input.GetMouseButtonDown(1) && Time.timeScale != 0f)
            {
                // if ammo is present, fire
                if (ammo > 0 && !reloading)
                {
                    ammo--; // decrements ammo count
                    if (isFireBall)
                    {
                        FindObjectOfType<AudioManager>().Play("flamestrike");
                        GameObject newFireBall = Instantiate(fireBall, point.position, Quaternion.identity);  //sets bullet to follow rotation
                        newFireBall.transform.localScale *= Mathf.Sign(transform.localScale.x);

                        // finds normalized vector using degrees
                        float dirX2 = Mathf.Cos((rotZ * Mathf.Deg2Rad) + (Mathf.PI / 2));
                        float dirY2 = Mathf.Sin((rotZ * Mathf.Deg2Rad) + (Mathf.PI / 2));

                        // makes new vector3 normalized vector
                        Vector3 direction2 = new Vector3(dirX2, dirY2, 0);

                        // grabs StunBolt component and sets direction of StunBolt
                        FireBall newFireBallScript = newFireBall.GetComponent<FireBall>();
                        newFireBallScript.SetDirection(direction2);

                        timeBtwShots = startTimeBtwShots;
                    }
                    else if (isFireBall == false)
                    {
                        FindObjectOfType<AudioManager>().Play("LaserGunShot");
                        GameObject newStunBolt = Instantiate(bullet, point.position, Quaternion.identity);  //sets bullet to follow rotation
                        newStunBolt.transform.localScale *= Mathf.Sign(transform.localScale.x);

                        // finds normalized vector using degrees
                        float dirX = Mathf.Cos((rotZ * Mathf.Deg2Rad) + (Mathf.PI / 2));
                        float dirY = Mathf.Sin((rotZ * Mathf.Deg2Rad) + (Mathf.PI / 2));

                        // makes new vector3 normalized vector
                        Vector3 direction = new Vector3(dirX, dirY, 0);

                        // grabs StunBolt component and sets direction of StunBolt
                        StunBolt newStunBoltScript = newStunBolt.GetComponent<StunBolt>();
                        newStunBoltScript.SetDirection(direction);

                        timeBtwShots = startTimeBtwShots;
                    }
                }
                // if not currently reloading, reload
                else if (!reloading)
                {
                    FindObjectOfType<AudioManager>().Play("LaserGunReload");
                    StartCoroutine(reload());
                }
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
            coolDown -= Time.deltaTime;
        }

        if (reloading)
        {
            float timeLeft = reloadTime - (Time.time - reloadStart);
            reloadTracker.text = "RELOADING..." + timeLeft.ToString("F1");
        }
    }

    private IEnumerator reload()
    {
        reloadStart = Time.time;

        reloading = true;
        reloadTracker.enabled = true;

        yield return new WaitForSeconds(reloadTime);

        ammo = magazineCapacity;

        reloading = false;
        reloadTracker.enabled = false;
    }
}
