﻿using BestHTTP.SocketIO;
using Expect.StaticAsset;
using Hsinpa.Socket;
using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;

namespace Hsinpa.Controller
{
    public class MonitorCtrl : ObserverPattern.Observer
    {
        [SerializeField]
        private MonitorPanelView _monitorView;

        private TypeFlag.SocketDataType.ClassroomDatabaseType selectedRoomData;
        private List<TypeFlag.SocketDataType.StudentDatabaseType> allStudentData;
        private SocketIOManager _socketIOManager;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case GeneralFlag.ObeserverEvent.ShowMonitorUI:
                    selectedRoomData = (TypeFlag.SocketDataType.ClassroomDatabaseType)p_objects[0];

                    _monitorView.Show(true);

                    PrepareStudentData(selectedRoomData);
                    break;
            }
        }

        public void SetUp(SocketIOManager socketIOManager)
        {
            _socketIOManager = socketIOManager;
            _socketIOManager.socket.On(TypeFlag.SocketEvent.UserJoined, OnUserJoinEvent);
            _socketIOManager.socket.On(TypeFlag.SocketEvent.UserLeaved, OnUesrLeaveEvent);
            _socketIOManager.socket.On(TypeFlag.SocketEvent.RefreshUserStatus, OnRefreshUserStatusEvent);
            _socketIOManager.socket.On(TypeFlag.SocketEvent.StartGame, OnGameStartSocketEvent);

            _monitorView.SetUp(OnGameStartBtnClickEvent, OnTerminateBtnClickEvent, OnMoreInfoBtnClickEvent, OnStudentObjClick);
        }

        private void PrepareStudentData(TypeFlag.SocketDataType.ClassroomDatabaseType selectedRoomData) {
            string getStudentURI = string.Format(StringAsset.API.GetAllStudentByID, selectedRoomData.class_id, selectedRoomData.year);

            _monitorView.ResetContent();

            StartCoroutine(
            APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(getStudentURI), UnityWebRequest.kHttpVerbGET, null, (string json) => {
                if (string.IsNullOrEmpty(json))
                {
                    return;
                }

                var tempStudentData = JsonHelper.FromJson<TypeFlag.SocketDataType.StudentDatabaseType>(json);

                if (tempStudentData != null)
                {
                    allStudentData = tempStudentData.ToList();

                    string fullRoomName = string.Format("{0}年, {1}", selectedRoomData.year, selectedRoomData.class_name);
                    _monitorView.SetContent(fullRoomName, allStudentData);

                    _socketIOManager.Emit(TypeFlag.SocketEvent.RefreshUserStatus);
                }
                
            }, null));
        }

        private void OnGameStartBtnClickEvent(Button btn) {
            btn.interactable = false;

            _socketIOManager.socket.Emit(TypeFlag.SocketEvent.StartGame, string.Format("{{\"room_id\" : \"{0}\"}}", selectedRoomData.class_id));
        }

        private void OnTerminateBtnClickEvent(Button btn)
        {
            btn.interactable = false;
        }

        private void OnMoreInfoBtnClickEvent(Button btn)
        {

        }

        private void OnStudentObjClick(MonitorItemPrefabView studentItem) {

            MainApp.Instance.Notify(GeneralFlag.ObeserverEvent.ShowUserInfo, studentItem.studentDatabaseType, studentItem.isOnline);

        }

        #region Socket Section
        private void OnUserJoinEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
        {
            if (args.Length > 0)
            {
                var userComp = JsonUtility.FromJson<TypeFlag.SocketDataType.UserComponentType>(args[0].ToString());
                _monitorView.SetUserConnectionType(true, userComp.user_id);
            }
        }

        private void OnUesrLeaveEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
        {
            if (args.Length > 0) {
                var userComp = JsonUtility.FromJson<TypeFlag.SocketDataType.UserComponentType>(args[0].ToString());
                _monitorView.SetUserConnectionType(false, userComp.user_id);
            }
        }

        private void OnRefreshUserStatusEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
        {
            if (args.Length > 0)
            {
                var userComps = JsonHelper.FromJson<TypeFlag.SocketDataType.UserComponentType>(args[0].ToString());
                _monitorView.SyncUserStateByArray(userComps);
            }
        }

        private void OnGameStartSocketEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
        {
            if (args.Length > 0)
            {
                Debug.Log(args[0].ToString());

                var roomComps = JsonUtility.FromJson<TypeFlag.SocketDataType.RoomComponentType>(args[0].ToString());

                _monitorView.SetTimer(roomComps.end_time);
            }
        }
        #endregion

    }
}