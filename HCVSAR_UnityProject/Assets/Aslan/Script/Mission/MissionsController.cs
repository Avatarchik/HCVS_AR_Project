﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expect.View;

public class MissionsController : Singleton<MissionsController>
{
    protected MissionsController() { } // guarantee this will be always a singleton only - can't use the constructor!

    public MissionViewController_0 mission_0;
    public MissionViewController_2 mission_2;
    public MissionViewController_3 mission_3;
    public MissionViewController_4 mission_4;
    public MissionViewController_5 mission_5;
    public MissionViewController_6 mission_6;
    public MissionViewController_7 mission_7;
    public MissionViewController_8 mission_8;
    public MissionViewController_9 mission_9;

    EnterMissionView enterMissionView;
    QuestionMissionView questionMissionView;
    EndMissionView endMissionView;

    private void Awake()
    {
        InitController();
    }

    private void InitController()
    {
        mission_0 = mission_0.GetComponent<MissionViewController_0>();
        mission_2 = mission_2.GetComponent<MissionViewController_2>();
        mission_3 = mission_3.GetComponent<MissionViewController_3>();
        mission_4 = mission_4.GetComponent<MissionViewController_4>();
        mission_5 = mission_5.GetComponent<MissionViewController_5>();
        mission_6 = mission_6.GetComponent<MissionViewController_6>();
        mission_7 = mission_7.GetComponent<MissionViewController_7>();
        mission_8 = mission_8.GetComponent<MissionViewController_8>();
        mission_9 = mission_9.GetComponent<MissionViewController_9>();
    }

    // MARK: If Dont use Delete
    public void RemoveAllListeners()
    {
        endMissionView.RemoveListeners();
        questionMissionView.RemoveListeners();
        enterMissionView.RemoveListeners();
    }
}