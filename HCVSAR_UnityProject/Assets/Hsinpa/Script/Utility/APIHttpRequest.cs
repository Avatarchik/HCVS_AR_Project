﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp;
using UnityEngine.Networking;
using System.Text;

public class APIHttpRequest : MonoBehaviour
{

    public static void Curl(string url, HTTPMethods httpMethods, string rawJsonObject, System.Action<bool, string> callback) {
        var r = new BestHTTP.HTTPRequest(new System.Uri(url), httpMethods, (request, response) =>
        {
            if (callback == null) return;

            if (request.State == HTTPRequestStates.Finished)
            {
                callback(true, response.DataAsText);
            }
            else {
                Debug.LogError("Request Finished with Error! " + (request.Exception != null ? (request.Exception.Message + "\n" + request.Exception.StackTrace) : "No Exception"));
                callback(false, "");
            }
        });
        
        if (!string.IsNullOrEmpty(rawJsonObject)) {
            r.SetHeader("Content-Type", "application/json; charset=UTF-8");
            r.RawData = System.Text.Encoding.UTF8.GetBytes(rawJsonObject);
        }

        //r.Timeout = System.TimeSpan.FromSeconds(4);
        //r.ConnectTimeout = System.TimeSpan.FromSeconds(4);
        r.DisableCache = true;
        
        r.Send();
    }



    public static IEnumerator NativeCurl(string url, string httpMethods, string rawJsonObject, System.Action<bool, string> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.timeout = 4;
            webRequest.method = httpMethods;

            if (rawJsonObject != null) {
                webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(rawJsonObject));
                webRequest.uploadHandler.contentType = "application/json";
            }

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            callback(!webRequest.isNetworkError, webRequest.downloadHandler.text);
        }
    }

}