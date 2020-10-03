﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expect.StaticAsset;

public class Mission8 : ViewController
{
    [SerializeField]
    private Sprite dog;

    // Message
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string dogMessage1 = StringAsset.MissionsDialog.Eight.d1;
    private string dogMessage2 = StringAsset.MissionsDialog.Eight.d2;
    //private string[] historyMessage = { StringAsset.MissionsDialog.Eight.history1, StringAsset.MissionsDialog.Eight.history2,
                                       // StringAsset.MissionsDialog.Eight.history3, StringAsset.MissionsDialog.Eight.history4 };

    [HideInInspector]
    public bool isEnterMission;
    public GameObject hideBG;
    public GameObject video;
    public GameObject toolView;

    public override void Enable()
    {
        base.Enable();

        isEnterMission = true;
        hideBG.SetActive(false);
        video.SetActive(true);
        JoeMain.Main.StarAndPlay360Video(4);// Start360Video(4);
    }

    public override void NextAction()
    {
        Debug.Log("8 Finish");

        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += ClickCount; // Add fingerClick event
    }

    void ClickCount()
    {
        clickCount++;

        if (clickCount >= 0)
        {
            EndMessage();
        }

        Debug.Log("8 clickCount: " + clickCount);
    }

    private void EndMessage()
    {

        if (clickCount == 1)
        {
            video.SetActive(false);
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(dogName, dogMessage1, dog);
        }

        if (clickCount == 2)
        {
            dialogMissionView.Show(false);
            toolView.SetActive(true);
        }

        if (clickCount == 3)
        {
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(dogName, dogMessage2, dog);
        }

        if (clickCount == 4)
        {
            LeaveEvent();
        }
    }

    private void LeaveEvent()
    {
        dialogMissionView.Show(false);
        JoeMain.Main.Stop360Video();

        InitFingerClick();
        RemoveAllEvent();

        hideBG.SetActive(true);

        MissionsController.Instance.ReSetMissions();
    }

    private void RemoveAllEvent()
    {
        fingerClick.Click -= ClickCount;
    }

    private void InitFingerClick()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= ClickCount;
        clickCount = 0; // initial
    }
}
