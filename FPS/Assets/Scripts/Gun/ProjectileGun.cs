using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    public int damage = 10;

    //bullet
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    //gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //Graphics
    public GameObject muzzleFlash;
    public TextMeshPro ammunitionDisplay;

    //Bug fixing :)
    public bool allowInvoke = true;


    // Start is called before the first frame update
    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        //Set ammo display, if it exists
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
    }

    void MyInput()
    {
        //Check if allowed to hold button
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        //Reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        //Shooting
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullet shot to 0
            bulletsShot = 0;

            Shoot();
        }
    }

    void Shoot()
    {
        readyToShoot = false;

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Ray through middle of screen
        RaycastHit hit;

        //Check id ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;

            if (hit.collider.gameObject.GetComponent<TargetHealth>() != null)
            {
                hit.collider.gameObject.GetComponent<TargetHealth>().TakeDamage(damage);
            }
        }
            
        else
            targetPoint = ray.GetPoint(75); //A point far away from player

        //Calculate direction
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //CalculateNewDirection with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        //Instantiate bullet
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //add pread to last distance
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        //Instantiate muzzle flash, if you have one
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);


        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function(if not already invoked)
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
