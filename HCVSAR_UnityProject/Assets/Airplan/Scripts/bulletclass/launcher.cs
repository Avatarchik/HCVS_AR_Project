﻿using UnityEngine;
using System.Collections;
public enum ModeState
{
    Create = 0,
    Ring = 1,
    Swirl = 2,
    Line = 3
}

public class launcher : MonoBehaviour {
    EnemyAI enemyAI;

    public Transform target;//目標
    public Transform child;
    [Header("子彈物件")]
    public GameObject B_bullet;
    [Header("射擊模式")]
    public ModeState Mode = ModeState.Ring;    
    [Header("相關參數")]
    public float B_angle=360;//發射擴散角度
	public int B_ring_quantity = 12;//單次發測量
	public int B_loop_Qt = 6;//總發射次數
    public int bout = 10; //執行次數
    public float B_CD = 1;//CD時間
    public float L_CD = 0; //CD
    public float delay = 0;//等待時間
    int loopS = 0;
    float x;
    

    [Header("怪物路徑")]
    public bool replace = false;
    public Vector3 initialPosition; // initial position on sphere
    public Vector3 initialDirection = new Vector3(1f, 0, 0);    // when on the sphere, first moving direction
    public AnimationCurve rotateCurve; // rotate curve


    float Tt;
	float qr;
    Player pl;

	void Start () {

        child = new GameObject("axis").GetComponent<Transform>();      
        child.position = transform.position;
        child.rotation = transform.rotation;
        child.parent = transform;

        enemyAI = GetComponent<EnemyAI>();
        /*switch (Mode)
        {
            case ModeState.Create:
                enemyAI = GetComponent<EnemyAI>();
                break;
            case ModeState.Ring:
                enemyAI = GetComponent<EnemyAI>();
                break;
            case ModeState.Swirl:
                enemyAI = GetComponent<EnemyAI>();
                break;
            case ModeState.Line:
                enemyAI = GetComponentInParent<EnemyAI>();
                break;

        }*/


        qr = B_angle / B_ring_quantity;

        Tt = Time.time + delay;
        pl = Object.FindObjectOfType<Player>();
    }

	void Update () {

        switch (Mode)
        {
            case ModeState.Create:
                Create();
                break;
            case ModeState.Ring:
                
                Ring();
                break;
            case ModeState.Swirl:
               
                Swril();
                break;
            case ModeState.Line:
                
                Line();
                break;

        }

    }

	public void Fire(Vector3 pos , Quaternion rota){
        if (B_loop_Qt > 0 && Time.time > Tt)
        {
            GameObject bullet1 = Instantiate (B_bullet, pos, rota) as GameObject;
            bullet1.transform.parent = Game.moveall;
            if (replace == true)
            {
                EnemyWanderer w;
                w = bullet1.GetComponent<EnemyWanderer>();
                w.initialPosition=initialPosition;
                w.initialDirection = initialDirection;
                w.rotateCurve = rotateCurve;
            }
            B_loop_Qt--;
            Tt = Time.time + B_CD;
        }
    }

    public void Create()
    {
             
            Fire(transform.position, transform.rotation);
      
    }

    public void Ring()
    {
        if (enemyAI.onSphere == true  && Time.time > Tt)
        {

            child.rotation = transform.rotation;
            child.Rotate(new Vector3(0, -B_angle / 2 + qr / 2, 0));

            for (int i = 0; i < B_ring_quantity; i++)
            {
                GameObject bullet1 = Instantiate(B_bullet, transform.position, child.rotation) as GameObject;
                bullet1.transform.parent = Game.moveall;
                child.Rotate(new Vector3(0, qr, 0));
            }

            B_loop_Qt--;
            loopS++;
            if (loopS >= bout)
            {
                Tt = Time.time + L_CD;
                loopS = 0;
               
            }
            else
            {
                Tt = Time.time + B_CD;
            }

            child.rotation = transform.rotation;

        }
    }
    
    public void Swril()
    {
        //child.rotation = transform.rotation;
        //child.Rotate(new Vector3(0, -B_angle / 2 + qr / 2, 0));
        if ( enemyAI.onSphere == true  && Time.time > Tt)
        {
            GameObject bullet1 = Instantiate(B_bullet, transform.position, child.rotation) as GameObject;
            bullet1.transform.parent = Game.moveall;            
            child.Rotate(new Vector3(0, qr, 0));
         
            B_loop_Qt--;
            loopS++;

            if (loopS >= bout)
            {
                Tt = Time.time + L_CD;
                loopS = 0;
                child.Rotate(new Vector3(0, -B_angle / 2 + qr / 2, 0));
            }
            else
            {
                Tt = Time.time + B_CD;
            }
        }
    }

    public void Line()
    {

        if (enemyAI.onSphere == true  && Time.time > Tt)
        {
            GameObject bullet1 = Instantiate(B_bullet, transform.position, child.rotation) as GameObject;
            bullet1.transform.parent = Game.moveall;
            B_loop_Qt--;
            loopS++;

            if (loopS >= bout)
            {
                Tt = Time.time + L_CD;
                loopS = 0;
            }
            else
            {
                Tt = Time.time + B_CD;
            }
        }
    }



}
