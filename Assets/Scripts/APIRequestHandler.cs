using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class APIRequestHandler
{
    private readonly string DefaultParamValue1 = "TIME_SERIES_DAILY_ADJUSTED";
    private readonly string DefaultParamValue2 = "";
    private readonly string DefaultParamValue3 = "compact";
    private readonly string DefaultParamValue4 = "5E8JJEWBIBM3511E";
    private string Query;
    public JSONNode APINode;
    private string[] ParametersArray;
    public delegate void ParameterCheck();
    private string PValue1, PValue2, PValue3, PValue4;

    public IEnumerator GetRequest(Action<JSONNode> actionGetJSON,string BU,string QF, string param1, string param2, string param3, string param4)
    {
        SetValuesForParameters(param1, param2, param3, param4);
        string query=QuerySetter(QF);
        Debug.Log(BU);
        string CU = BU + query;
        Debug.Log(CU);
        UnityWebRequest APIWebRequest = UnityWebRequest.Get(CU);
        yield return APIWebRequest.SendWebRequest();
 

        if(APIWebRequest.isNetworkError || APIWebRequest.isHttpError)
        {
            Debug.Log("Error fetching API.");
            yield break;
        }
        Debug.Log("fetched API!");
        APINode = JSON.Parse(APIWebRequest.downloadHandler.text);
        actionGetJSON(APINode);
    }
    /*public async Task<JSONNode> GetRequestAsync(string BU, string QF, string param1, string param2, string param3, string param4)
    {
        SetValuesForParameters(param1, param2, param3, param4);
        string query = QuerySetter(QF);
        Debug.Log(BU);
        string CU = BU + query;
        Debug.Log(CU);
        UnityWebRequest APIWebRequest = UnityWebRequest.Get(CU);
        /*var operation = await APIWebRequest.SendWebRequest();
        //operation.wait();

        //while(!operation.isDone)
            await Task.Yield();


        if (APIWebRequest.isNetworkError || APIWebRequest.isHttpError)
        {
            Debug.Log("Error fetching API.");
            return null;
        }
        Debug.Log("fetched API!");
        APINode = JSON.Parse(APIWebRequest.downloadHandler.text);
        return APINode;
    }*/
    public string QuerySetter(string query)
    {
        Query = "/query?";
        SplitQuery(query);
        Query += CombineQuery();
        Debug.Log(Query);
        return Query;
    }
    void SetValuesForParameters(string paramValue1, string paramValue2, string paramValue3, string paramValue4)
    {
        PValue1 = string.IsNullOrEmpty(paramValue1) ? DefaultParamValue1 : paramValue1;
        PValue2 = string.IsNullOrEmpty(paramValue2) ? DefaultParamValue2 : paramValue2;
        PValue3 = string.IsNullOrEmpty(paramValue3) ? DefaultParamValue3 : paramValue3;
        PValue4 = string.IsNullOrEmpty(paramValue4) ? DefaultParamValue4 : paramValue4;
    }
    public void SplitQuery(string query)
    {
        int i = 0;
        ParametersArray = query.Split('=');
        foreach (string q in ParametersArray)
        {
            Debug.Log("The parameter at index " + i + " is " + q + "");
            i++;
        }
    }
    public string CombineQuery()
    {
        string[] varNames = new string[]{ PValue1, PValue2, PValue3, PValue4 };
        for(int i=0;i<ParametersArray.Length-1;i++)
        {
            ParametersArray[i] += "="+varNames[i];
        }

        return string.Join("", ParametersArray);
    }
    public JSONNode GetJSON()
    {
        if(APINode==null)
        {
            Debug.Log("It's Empty!!");
        }
        else
        {
            Debug.Log("returning non empty node!");
        }
        return APINode;
    }
}
