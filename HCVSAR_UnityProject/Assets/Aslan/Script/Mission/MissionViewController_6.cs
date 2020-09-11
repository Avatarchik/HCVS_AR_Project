﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;

public class MissionViewController_6 : MonoBehaviour
{
    [SerializeField]
    private Sprite dog;
    [SerializeField]
    private Sprite npc;

    [SerializeField]
    EnterMissionView enterMissionView;
    [SerializeField]
    SituationMissionView situationMissionView;
    [SerializeField]
    DialogMissionView dialogMissionView;
    [SerializeField]
    QuestionMissionView questionMissionView;
    [SerializeField]
    EndMissionView endMissionView;
    [SerializeField]
    FingerClickEvent fingerClick;

    private int clickCount;

    // Message
    string situationMessage = StringAsset.MissionsSituation.Six.s1;
    string dogName = StringAsset.MissionsDialog.Person.dog;
    string npcName = StringAsset.MissionsDialog.Person.NPC_1;
    string npcMessage_1 = StringAsset.MissionsDialog.Six.d1;


    string[] historyMessage = { StringAsset.MissionsDialog.Three.history1, StringAsset.MissionsDialog.Three.history2, StringAsset.MissionsDialog.Three.history3 };

    private string qustion = StringAsset.MissionsQustion.Six.qustion;
    private string[] answers = { StringAsset.MissionsAnswer.Six.ans1, StringAsset.MissionsAnswer.Six.ans2,
                                 StringAsset.MissionsAnswer.Six.ans3, StringAsset.MissionsAnswer.Six.ans4};

    private string correctMessage_1 = StringAsset.MissionsDialog.Six.correct_1;
    private string correctMessage_2 = StringAsset.MissionsDialog.Six.correct_2;
    private string faultMessage_1 = StringAsset.MissionsDialog.Six.fault_1;
    private string faultMessage_2 = StringAsset.MissionsDialog.Six.fault_2;
    private string faultMessage_3 = StringAsset.MissionsSituation.Six.fault;
    private string endMessage = StringAsset.MissionsEnd.End.message;

    public void MissionStart(int missionNumber)
    {
        TypeFlag.InGameType.MissionType[] missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;
        MainView.Instance.studentScoreData.mission_id = missionArray[missionNumber].mission_id;

        enterMissionView.Show(true);
        enterMissionView.EnterMission(missionArray[missionNumber].mission_name, missionArray[missionNumber].mission_name);
        enterMissionView.OnEnable += StarEnable;
        enterMissionView.OnDisable += Disable;
    }

    // TODO: ibeacon find other mission after 10 second
    private void Disable()
    {
        enterMissionView.Show(false);
        enterMissionView.RemoveListeners();
        Debug.Log("other thing");
    }

    private void StarEnable()
    {
        enterMissionView.OnEnable -= StarEnable;
        enterMissionView.OnDisable -= Disable;
        enterMissionView.Show(false);

        situationMissionView.Show(true);
        situationMissionView.SituationView(situationMessage);

        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += ClickCount; // Add fingerClick event
    }

    void ClickCount()
    {
        clickCount++;
        Convercestion();
        Debug.Log("clickCount: " + clickCount);
    }

    void Convercestion()
    {
        if (clickCount == 1)
        {
            situationMissionView.Show(false);
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(npcName, npcMessage_1, npc);
        }

        if (clickCount == 2)
        {
            Debug.Log("Finish");
            Qusteion();
        }
    }

    private void Qusteion()
    {
        InitFingerClick();

        dialogMissionView.Show(false);
        questionMissionView.Show(true);

        questionMissionView.QuestionView(qustion, answers, 0);
        questionMissionView.buttonClick += OpenClickEvent;
    }

    private void OpenClickEvent()
    {
        questionMissionView.Show(false);
        dialogMissionView.Show(true);
        dialogMissionView.DialogView(dogName, correctMessage_1, dog);

        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += QuestionReult; // Add fingerClick event
    }

    private void QuestionReult()
    {
        clickCount++;

        int score = MainView.Instance.studentScoreData.score;
        int number;  

        if (score > 0)
        {
            number = 2;
            Debug.Log("clickCount: " + clickCount);
            if (clickCount == 1)
            {
                dialogMissionView.DialogView(npcName, correctMessage_2, npc);
            }
            if (clickCount >= number && clickCount < historyMessage.Length + number)
            {
                Debug.Log("clickCount3: " + clickCount);
                dialogMissionView.DialogView(dogName, historyMessage[clickCount - number], dog);
            }

            if (clickCount == historyMessage.Length + number)
            {
                Debug.Log("Finish");
                LeaveMission(score);
            }

        }
        else
        {
            number = 4;

            if (clickCount == 1)
            {
                dialogMissionView.DialogView(npcName, faultMessage_1, npc);
            }
            if (clickCount == 2)
            {
                dialogMissionView.DialogView(dogName, faultMessage_2, dog);
            }
            if (clickCount == 3)
            {
                dialogMissionView.Show(false);
                situationMissionView.Show(true);
                situationMissionView.SituationView(faultMessage_3);
            }
            if (clickCount >= number && clickCount < historyMessage.Length + number)
            {
                Debug.Log("clickCount4: " + clickCount);
                situationMissionView.Show(false);
                dialogMissionView.Show(true);                
                dialogMissionView.DialogView(dogName, historyMessage[clickCount - number], dog);
            }

            if (clickCount == historyMessage.Length + number)
            {
                Debug.Log("Finish");
                LeaveMission(score);
            }

        }
    }

    private void LeaveMission(int score)
    {
        dialogMissionView.Show(false);
        endMissionView.Show(true);
        endMissionView.EndMission(score, endMessage);
        endMissionView.OnEnable += LeaveEvent;
    }

    private void LeaveEvent()
    {
        endMissionView.Show(false);

        InitFingerClick();
        RemoveAllEvent();
        RemoveAllListeners();

        Debug.Log("Mission 6 Leave");
    }

    private void RemoveAllListeners()
    {
        endMissionView.RemoveListeners();
        questionMissionView.RemoveListeners();
        enterMissionView.RemoveListeners();
    }

    private void RemoveAllEvent()
    {
        fingerClick.Click -= ClickCount;
        enterMissionView.OnEnable -= StarEnable;
        enterMissionView.OnDisable -= Disable;
        endMissionView.OnEnable -= LeaveEvent;
        questionMissionView.buttonClick -= QuestionReult;
    }

    private void InitFingerClick()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= ClickCount;
        clickCount = -1; // initial
    }
}


