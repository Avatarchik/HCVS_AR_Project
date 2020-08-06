﻿using Expect.StaticAsset;
using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View
{
    public class UserInfoModal : Modal
    {
        [SerializeField]
        private Button CloseButton;

        [SerializeField]
        private Text TotalScoreText;

        [SerializeField]
        private Text UserInfoText;

        [SerializeField]
        private Transform AchievementHolder;

        [SerializeField]
        private GameObject AchievementPrefab;

        [SerializeField]
        private MissionItemSObj missionItemSObj;

        private TypeFlag.UserType userType;

        private void Awake()
        {
            CloseButton.onClick.AddListener(() =>
            {
                Modals.instance.Close();
            });
        }

        public void SetUserInfo(TypeFlag.SocketDataType.StudentDatabaseType studentObj, bool hasConnection) {
            ResetContent();

            UserInfoText.text = GetUserInfoText(studentObj, hasConnection);
        }

        public void SetContent(TypeFlag.SocketDataType.UserScoreType[] scoreArray, TypeFlag.UserType userType) {
            this.userType = userType;
            TotalScoreText.text = CalculateAccompishPercent(scoreArray);

            GenerateScoreBoard(scoreArray);
        }

        private string GetUserInfoText(TypeFlag.SocketDataType.StudentDatabaseType studentObj, bool hasConnection) {

            string formString = string.Format(StringAsset.UserInfo.HeaderUserInfo, studentObj.student_name, studentObj.id,

                (hasConnection) ? StringAsset.UserInfo.OnlineColor : StringAsset.UserInfo.OfflineColor,
                (hasConnection) ? StringAsset.UserInfo.Online : StringAsset.UserInfo.Offline);

            return formString;
        }

        private int CalculateScore(TypeFlag.SocketDataType.UserScoreType[] scoreArray) {
            return scoreArray.Sum(x => x.score);
        }

        private string CalculateAccompishPercent(TypeFlag.SocketDataType.UserScoreType[] scoreArray)
        {
            float a_percent = (((float)scoreArray.Length) / missionItemSObj.missionArray.Length) * 100;

            return a_percent + "%";
        }

        private void GenerateScoreBoard(TypeFlag.SocketDataType.UserScoreType[] scoreArray) {

            if (missionItemSObj.missionArray == null) return;

            var scoreList = scoreArray.ToList();
            int missionLength = missionItemSObj.missionArray.Length;

            Utility.UtilityMethod.ClearChildObject(AchievementHolder);

            for (int i = 0; i < missionLength; i++)
            {
                AchievementItemView a_item = Utility.UtilityMethod.CreateObjectToParent(AchievementHolder, AchievementPrefab).GetComponent<AchievementItemView>();

                a_item.hashed = (userType != TypeFlag.UserType.Teacher);

                //Check if this task is been accompished
                var userScoreIndex = scoreList.FindIndex(x => x.mission_id == missionItemSObj.missionArray[i].mission_id);

                a_item.SetTitle(missionItemSObj.missionArray[i].mission_name, (userScoreIndex >= 0));
            }
        }

        private void ResetContent() {
            UserInfoText.text = "";
            TotalScoreText.text = "0%";

            Utility.UtilityMethod.ClearChildObject(AchievementHolder);
        }

    }
}