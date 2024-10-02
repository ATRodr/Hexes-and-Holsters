using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotate : MonoBehaviour
{
    public GameObject Down;
    public GameObject DownRight;
    public GameObject DownLeft;
    public GameObject Left;
    public GameObject Right;
    public GameObject Up;
    public GameObject UpLeft;
    public GameObject UpRight;


    private void FixedUpdate()
    {
        Vector3 diffrence = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diffrence.Normalize();

        float rotationZ = Mathf.Atan2(diffrence.y, diffrence.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        if (transform.eulerAngles.z >= 31 && transform.eulerAngles.z <=60)
            {
                Down.SetActive(false);
                DownRight.SetActive(false);
                DownLeft.SetActive(false);
                Left.SetActive(false);
                Right.SetActive(false);
                Up.SetActive(false);
                UpRight.SetActive(true);
                UpLeft.SetActive(false);
            }
            if (transform.eulerAngles.z >= 0 && transform.eulerAngles.z <= 30)
            {
                Down.SetActive(false);
                DownRight.SetActive(false);
                DownLeft.SetActive(false);
                Left.SetActive(false);
                Right.SetActive(true);
                Up.SetActive(false);
                UpRight.SetActive(false);
                UpLeft.SetActive(false);
            }
            if (transform.eulerAngles.z >= 331 && transform.eulerAngles.z <= 360)
            {
                Down.SetActive(false);
                DownRight.SetActive(false);
                DownLeft.SetActive(false);
                Left.SetActive(false);
                Right.SetActive(true);
                Up.SetActive(false);
                UpRight.SetActive(false);
                UpLeft.SetActive(false);
            }
            if (transform.eulerAngles.z >= 301 && transform.eulerAngles.z <= 330)
            {
                Down.SetActive(false);
                DownRight.SetActive(true);
                DownLeft.SetActive(false);
                Left.SetActive(false);
                Right.SetActive(false);
                Up.SetActive(false);
                UpRight.SetActive(false);
                UpLeft.SetActive(false);
            }
            if (transform.eulerAngles.z >= 61 && transform.eulerAngles.z <= 120)
            {
                Down.SetActive(false);
                DownRight.SetActive(false);
                DownLeft.SetActive(false);
                Left.SetActive(false);
                Right.SetActive(false);
                Up.SetActive(true);
                UpRight.SetActive(false);
                UpLeft.SetActive(false);
            }
            if (transform.eulerAngles.z >= 241 && transform.eulerAngles.z <= 300)
            {
                Down.SetActive(true);
                DownRight.SetActive(false);
                DownLeft.SetActive(false);
                Left.SetActive(false);
                Right.SetActive(false);
                Up.SetActive(false);
                UpRight.SetActive(false);
                UpLeft.SetActive(false);
            }
            if (transform.eulerAngles.z >= 211 && transform.eulerAngles.z <= 240)
            {
                Down.SetActive(false);
                DownRight.SetActive(false);
                DownLeft.SetActive(true);
                Left.SetActive(false);
                Right.SetActive(false);
                Up.SetActive(false);
                UpRight.SetActive(false);
                UpLeft.SetActive(false);
            }
            if (transform.eulerAngles.z >= 151 && transform.eulerAngles.z <= 210)
            {
                Down.SetActive(false);
                DownRight.SetActive(false);
                DownLeft.SetActive(false);
                Left.SetActive(true);
                Right.SetActive(false);
                Up.SetActive(false);
                UpRight.SetActive(false);
                UpLeft.SetActive(false);
            }
            if (transform.eulerAngles.z >= 121 && transform.eulerAngles.z <= 150)
            {
                Down.SetActive(false);
                DownRight.SetActive(false);
                DownLeft.SetActive(false);
                Left.SetActive(false);
                Right.SetActive(false);
                Up.SetActive(false);
                UpRight.SetActive(false);
                UpLeft.SetActive(true);
            }
       
    }

}