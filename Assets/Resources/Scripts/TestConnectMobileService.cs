using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RestSharp;
using Pathfinding.Serialization.JsonFx;
using Unity3dAzure.AppServices;
using chemistrecipe.model;
using System.Net;
using System;

namespace chemistrecipe
{
    public class TestConnectMobileService : MonoBehaviour
    {

        private MobileServiceClient _client;
        private MobileServiceTable<TodoItem> _table;

        void Start()
        {
            _client = new MobileServiceClient("https://chemistrecipe-mobile-backend.azurewebsites.net");
            _table = _client.GetTable<TodoItem>("TodoItem");

            ReadItems();
            
            _client.ExecuteAsync<List<DocumentTodoItem>>(new ZumoRequest(_client, "api/DocumentTodoItem", Method.GET), (response, handle) => OnReadDocumentsComplete(response));
        }

        private void OnReadDocumentsComplete(IRestResponse<List<DocumentTodoItem>> response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Debug.Log("OnGreetingCompleted data: " + response.Content);
                List<DocumentTodoItem> items = response.Data;
                Debug.Log("Document Todo items count: " + items.Count);

                items.ForEach((item) => {
                    Debug.Log(item.Name + " " + item.Description + " " + item.Completed);
                });
            }
        }

        private void ReadItems()
        {
            _table.Read<TodoItem>(OnReadItemsCompleted);
        }

        private void OnReadItemsCompleted(IRestResponse<List<TodoItem>> response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Debug.Log("OnReadItemsCompleted data: " + response.Content);
                List<TodoItem> items = response.Data;
                Debug.Log("Todo items count: " + items.Count);
            }
            else
            {
                ResponseError err = JsonReader.Deserialize<ResponseError>(response.Content);
                Debug.Log("Error " + err.code.ToString() + " " + err.error + " Uri: " + response.ResponseUri);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}