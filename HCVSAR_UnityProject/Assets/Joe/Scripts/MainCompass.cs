﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCompass : MonoBehaviour
{
    public static MainCompass main;
    public Transform camera;
    float dushu = 0;
    float tempdushu = 0;
    // Start is called before the first frame update
    public void Awake()
    {
        main = this;
    }

    void Start()
    {
        Input.location.Start();
        Input.compass.enabled = true;
        if (MissionsController.Instance.isARsupport) {
            //gameObject.SetActive(false);
        }
        else
        {
            //Input.location.Start();
            //Input.compass.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = camera.position+new Vector3(0,-2,0);

        //Input.location.Start();

        dushu = Input.compass.trueHeading;


        if (Mathf.Abs(tempdushu - dushu) > 3)
        {
            tempdushu = dushu;
            transform.eulerAngles = new Vector3(0,-dushu, 0);
        }
    }
}
