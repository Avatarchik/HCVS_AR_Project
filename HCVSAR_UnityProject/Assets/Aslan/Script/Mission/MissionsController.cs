﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;
using UnityEngine.XR.ARFoundation;

public class MissionsController : Singleton<MissionsController>
{
    protected MissionsController() { } // guarantee this will be always a singleton only - can't use the constructor!

    public GameObject[] MissionsObj;
    public ViewController[] viewControllers;
    public Sprite[] sprites;

    public EnterMissionView enterMissionView;
    public ARSession ARSession;
    public GyroControler gyroControler;

    public GameObject MainCameraObj;
    public GameObject ARCameraObj;
    public GameObject ARObj;

    [HideInInspector]
    public bool isARsupport;
    [HideInInspector]
    public Camera MainCamera;
    [HideInInspector]
    public Camera ARcamera;
    public Text text;
    [HideInInspector]
    public bool isEnter;
    
    private void Awake()
    {
        MainCamera = MainCameraObj.GetComponent<Camera>();
        ARcamera = ARCameraObj.transform.GetChild(0).GetComponent<Camera>();

        StartCoroutine(CheckSupport());        
    }

    private void Start()
    {
        InitController();
        ReSetMissions();
    }

    private void SwitchMainCamera(bool isSupport)
    {
        ARCameraObj.SetActive(isSupport);
        ARObj.SetActive(isSupport);
        MainCameraObj.SetActive(!isSupport);
    }

    private void InitController()
    {
        for (int i = 0; i < MissionsObj.Length; i++)
        {
            viewControllers[i] = MissionsObj[i].GetComponent<ViewController>();
        }
    }

    public void ReSetMissions()
    {
        isEnter = false;
        ARSession.enabled = false;
        foreach (GameObject g in MissionsObj) { g.SetActive(false); }
        MissionsObj[1].SetActive(true);
    }

    public void Missions(int number)
    {
        if (isARsupport) { ARSession.enabled = true; }

        enterMissionView.message.text = StringAsset.EnterMission.enterMission;
        MissionStart(number);
        enterMissionView.image.sprite = sprites[number];

        if (number != 3)
        { 
            enterMissionView.EnterButton.onClick.AddListener(() => EnterGame(number));
            enterMissionView.LeaveButton.onClick.AddListener(() => LeaveGame(number));
        }

        if(number == 9)
        {
            enterMissionView.message.text = StringAsset.EnterMission.enterEndMission;
        }
    }

    public void EnterGame(int number)
    {
        MissionsObj[number].SetActive(true);
        viewControllers[number].Enable();
        viewControllers[number].isEnter = true;
        JoeGM.joeGM.StartMission(number);

        isEnter = true;

        CloseEnterView();
    }

    public void LeaveGame(int number)
    {
        ARSession.enabled = false;
        viewControllers[number].isEnter = false;
        JoeGM.joeGM.LeaveMission();
        ReSetMissions();
        CloseEnterView();
    }

    public void MissionStart(int missionNumber)
    {
        TypeFlag.InGameType.MissionType[] missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;
        MainView.Instance.studentScoreData.mission_id = missionArray[missionNumber].mission_id;
        MainView.Instance.missionNumber = missionNumber;
        
        if (missionNumber == 3)
        {
            isEnter = true;
            EnterGame(missionNumber);
        }

        if (missionNumber != 3)
        {
            enterMissionView.Show(true);
            enterMissionView.EnterMission(missionArray[missionNumber].mission_name, missionArray[missionNumber].mission_name);
        }

    }

    // MARK: If Dont use Delete
    public void CloseEnterView()
    {
        enterMissionView.Show(false);
        enterMissionView.RemoveListeners();
    }


    private IEnumerator CheckSupport()
    {
        Debug.Log("Checking for AR support...");

        yield return ARSession.CheckAvailability();

        text.text = "support";
        //isARsupport = true;

        switch (ARSession.state)
        {
            case ARSessionState.Unsupported:
                isARsupport = false;
                text.text = "Device Unsupported AR";
                break;

            case ARSessionState.None:
                isARsupport = false;
                text.text = "Device None";
                break;

            case ARSessionState.NeedsInstall:
                isARsupport = false;
                text.text = "Device Installing";
                break;

            case ARSessionState.Ready:
                isARsupport = true;
                text.text = "Device Ready";
                break;

            default:
                isARsupport = false;
                break;
        }

        isARsupport = true;
        SwitchMainCamera(isARsupport);
    }
}
