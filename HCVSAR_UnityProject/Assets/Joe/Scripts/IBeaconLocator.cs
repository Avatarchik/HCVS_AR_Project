﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using System.Runtime.InteropServices;

public class IBeaconLocator : MonoBehaviour
{
    JoeGM GM;
    // Start is called before the first frame update

    public InputField[] z;
    
    Point point;
    Point p1;
    Point p2;
    Point p3;
    public Transform ct;
    public InputField[] pos;
    public Text[] poss;
    public Transform box;
    public bool st;
    void Start()
    {
        if (MissionsController.Instance.isARsupport)
        {
            //gameObject.SetActive(false);
        }
        else
        {
            JoeGM.beaconUPD += posUPD;
        }
      
        GM = JoeGM.joeGM;
        p1 = new Point();
        p2 = new Point();
        p3 = new Point();
        point = new Point();

    }

    public void addbox()
    {
        Instantiate(box, box.transform.position, box.rotation);
    }

    public void UI_st()
    {
        st = true;
    }
    public Text dd;
    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void posUPD()
    {
        //Debug.Log(example.mybeacons.Count);
        poss[0].text = box.position.x.ToString();
        poss[1].text = box.position.y.ToString();
        poss[2].text = box.position.z.ToString();
        try
        {

        }
        catch
        {

        }

        if (st)
        {


            p1.X = float.Parse(pos[0].text);
            p1.Y = float.Parse(pos[1].text);
            //p1.Distance = 1;
            try
            {
                p1.Distance = GM.IBeaconDistances[int.Parse(z[0].text)];
            }
            catch
            {
                p1.Distance = 5;
            }



            p2.X = float.Parse(pos[2].text);
            p2.Y = float.Parse(pos[3].text);
            //p2.Distance = 1;
            try
            {
                p2.Distance = GM.IBeaconDistances[int.Parse(z[1].text)];
            }
            catch
            {
                p2.Distance = 5;
            }



            p3.X = float.Parse(pos[4].text);
            p3.Y = float.Parse(pos[5].text);
            //p3.Distance = 1;
            try
            {
                p3.Distance = GM.IBeaconDistances[int.Parse(z[2].text)];
            }
            catch
            {
                p3.Distance = 5;
            }



            point = GetPiontByThree(p1, p2, p3);

            ct.position = new Vector3(point.X, ct.position.y, point.Y);


        }
    }

    public Transform p1t;
    public Transform p2t;
    public Transform p3t;
    public Transform target;
    public Transform target2;
    public void testttt()
    {
        p1.X = p1t.position.x;
        p1.Y = p1t.position.z;
        p1.Distance = Vector3.Distance(target.position, p1t.position);
        p2.X = p2t.position.x;
        p2.Y = p2t.position.z;
        p2.Distance = Vector3.Distance(target.position, p2t.position);
        p3.X = p3t.position.x;
        p3.Y = p3t.position.z;
        p3.Distance = Vector3.Distance(target.position, p3t.position);
        point = GetPiontByThree(p1, p2, p3);
        target2.position = new Vector3(point.X, ct.position.y, point.Y);
    }

    public void setprint()
    {

    }

    //public class

    static void Main(string[] args)
    {
        Point p1 = new Point() { X = 0, Y = 2, Distance = Math.Sqrt(5) };
        Point p2 = new Point() { X = 2, Y = 2, Distance = Math.Sqrt(5) };
        Point p3 = new Point() { X = 1, Y = 0, Distance = Math.Sqrt(0) };
        var p = GetPiontByThree(p1, p2, p3);
        Console.WriteLine("Point x:{0}", p.X);
        Console.WriteLine("Point y:{0}", p.Y);
        Console.ReadKey();
    }


    private static Point GetPiontByThree(Point p1, Point p2, Point p3)
    {

        var A = p1.X - p3.X;
        var B = p1.Y - p3.Y;
        var C = Math.Pow(p1.X, 2) - Math.Pow(p3.X, 2) + Math.Pow(p1.Y, 2) - Math.Pow(p3.Y, 2) + Math.Pow(p3.Distance, 2) - Math.Pow(p1.Distance, 2);
        var D = p2.X - p3.X;
        var E = p2.Y - p3.Y;
        var F = Math.Pow(p2.X, 2) - Math.Pow(p3.X, 2) + Math.Pow(p2.Y, 2) - Math.Pow(p3.Y, 2) + Math.Pow(p3.Distance, 2) - Math.Pow(p2.Distance, 2);

        var x = (B * F - E * C) / (2 * B * D - 2 * A * E);
        var y = (A * F - D * C) / (2 * A * E - 2 * B * D);

        Point P = new Point() { X = (float)x, Y = (float)y, Distance = 0 };
        return P;
    }

    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }

        public double Distance { get; set; }
    }
}
