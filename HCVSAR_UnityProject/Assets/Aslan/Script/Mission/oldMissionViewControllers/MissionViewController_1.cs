﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;

public class MissionViewController_1 : MonoBehaviour
{
    public Button success;
    public Button fail;
    public GameObject mailInfo;
    public CanvasGroup mainCanvas;

    [HideInInspector]
    public bool isSuccess;
    [SerializeField]
    private Sprite dog;
    [SerializeField]
    private Sprite police;

    [SerializeField]
    private DialogMissionView dialogMissionView;
    [SerializeField]
    private EndMissionView endMissionView;
    [SerializeField]
    private BagPanel bagPanel;
    [SerializeField]
    private FingerClickEvent fingerClick;

    private int clickCount;

    // Message
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string policeName = StringAsset.MissionsDialog.Person.NPC_6;
    private string dogMessage1 = StringAsset.MissionsDialog.One.d1;
    private string correctMessage_1 = StringAsset.MissionsDialog.One.correct_1;
    private string faultMessage_1 = StringAsset.MissionsDialog.One.fault_1;
    private string faultMessage_2 = StringAsset.MissionsDialog.One.fault_2;

    private string[] historyMessage = { StringAsset.MissionsDialog.One.history1, StringAsset.MissionsDialog.One.history2, StringAsset.MissionsDialog.One.history3 };
    private string endMessage = StringAsset.MissionsEnd.End.message;

    private void Start()
    {
        success.onClick.AddListener(SuccessClick);
        fail.onClick.AddListener(FailClick);
    }

    private void SuccessClick()
    {
        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += Count; // Add fingerClick event
        isSuccess = true;

        bagPanel.Show(false);
        dialogMissionView.Show(true);
        dialogMissionView.DialogView(dogName, dogMessage1, dog);
    }

    private void FailClick()
    {
        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += Count; // Add fingerClick event
        isSuccess = false;

        bagPanel.Show(false);
        dialogMissionView.Show(true);
        dialogMissionView.DialogView(dogName, dogMessage1, dog);
    }

    private void Count()
    {
        clickCount++;
        mainCanvas.interactable = false;

        if (clickCount >= 0)
        {
            if (isSuccess) { GameSuccess(); }
            if (!isSuccess) { GameFail(); }
        }
    }

    private void GameSuccess()
    {
        if (clickCount == 1)
        {
            dialogMissionView.DialogView(policeName, correctMessage_1, police);
        }

        if (clickCount >= 2)
        {
            StarHistory(2);
        }
    }

    private void GameFail()
    {
        if (clickCount == 1)
        {
            dialogMissionView.DialogView(policeName, faultMessage_1, police);
        }

        if (clickCount == 2)
        {
            dialogMissionView.DialogView(dogName, faultMessage_2, dog);
        }

        if (clickCount >= 3)
        {
            StarHistory(3);
        }
    }

    private void StarHistory(int number)
    {
        if (clickCount >= number && clickCount < historyMessage.Length + number)
        {
            dialogMissionView.DialogView(dogName, historyMessage[clickCount - number], dog);
        }

        if (clickCount == historyMessage.Length + number)
        {
            LeaveMission(isSuccess);
        }
    }

    private void LeaveMission(bool success)
    {
        int score = success ? 5 : 0;

        mailInfo.SetActive(false);
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

        mainCanvas.interactable = true;
        Debug.Log("Mission 1 Leave");
    }

    private void RemoveAllListeners()
    {
        endMissionView.RemoveListeners();
    }

    private void RemoveAllEvent()
    {
        fingerClick.Click -= Count;
        endMissionView.OnEnable -= LeaveEvent;
    }

    private void InitFingerClick()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= Count;
        clickCount = 0; // initial
    }
}
