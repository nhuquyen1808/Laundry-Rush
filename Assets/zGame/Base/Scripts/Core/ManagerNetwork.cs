// using System;
// using System.Collections.Generic;
// using BestHTTP;

// // by nt.Dev93
// namespace ntDev
// {
//     public static class ManagerNetwork
//     {
//         public static string URL_API = "";
//         public static string Token = "";

//         static List<string> listPrevent = new List<string>();

//         public static void Get(string url, Action<string> onSuccess = null, Action<string> onFail = null, List<string> listField = null, List<string> listValue = null)
//         {
//             if (listPrevent.Contains(url))
//             {
//                 return;
//             }
//             listPrevent.Add(url);

//             string str = URL_API + url;
//             Ez.Log("Request: " + str);

//             HTTPRequest request = new HTTPRequest(new System.Uri(str), methodType: HTTPMethods.Get, (request, response) =>
//             {
//                 listPrevent.Remove(url);
//                 if (response == null)
//                 {
//                     onFail?.Invoke("Fuck Server " + str + url);
//                 }
//                 else if (response.IsSuccess)
//                 {
//                     Ez.Log("Response: " + response.DataAsText);
//                     onSuccess?.Invoke(response.DataAsText);
//                 }
//                 else
//                 {
//                     Ez.Log("Error code: " + response.StatusCode);
//                     Ez.Log("Data: " + response.DataAsText);
//                     onFail?.Invoke(response.DataAsText);
//                 }
//             });

//             for (int i = 0; listField != null && i < listField.Count && i < listValue.Count; ++i)
//             {
//                 Ez.Log("Field: " + listField[i] + " : " + listValue[i]);
//                 request.AddField(listField[i], listValue[i]);
//             }
//             if (Token != "") request.SetHeader("Authorization", "Bearer " + Token);
//             request.Send();
//         }

//         public static void Post(string url, Action<string> onSuccess = null, Action<string> onFail = null, List<string> listField = null, List<string> listValue = null)
//         {
//             if (listPrevent.Contains(url))
//             {
//                 return;
//             }
//             listPrevent.Add(url);

//             string str = URL_API + url;
//             Ez.Log("Request: " + str);

//             HTTPRequest request = new HTTPRequest(new System.Uri(str), methodType: HTTPMethods.Post, (request, response) =>
//             {
//                 listPrevent.Remove(url);
//                 if (response.IsSuccess)
//                 {
//                     Ez.Log("Response: " + response.DataAsText);
//                     onSuccess?.Invoke(response.DataAsText);
//                 }
//                 else
//                 {
//                     Ez.Log("Error code: " + response.StatusCode);
//                     Ez.Log("Data: " + response.DataAsText);
//                     onFail?.Invoke(response.DataAsText);
//                 }
//             });

//             for (int i = 0; listField != null && i < listField.Count && i < listValue.Count; ++i)
//             {
//                 Ez.Log("Field: " + listField[i] + " : " + listValue[i]);
//                 request.AddField(listField[i], listValue[i]);
//             }
//             if (Token != "") request.SetHeader("Authorization", "Bearer " + Token);
//             request.Send();
//         }
//     }
// }