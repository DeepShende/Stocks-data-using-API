using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public static GraphManager graphManager;
    private static List<GameObject> graphBars;
    public GameObject GraphBarPrefab;
    DateTime LastRefreshDate;
    StocksUI _stocksUIinstance;
    private static JSONNode _stockInfo;
    public float factorY,factorZ;
    public static string[] DatesString=new string[10];
    public static float[] ClosingValues=new float[10];
    public static int[] VolumeValues=new int[10];
    static DateTime iteratorDate;
    static string CurrDate;
    private void Awake()
    {
        if(graphManager == null)
        {
            graphManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        print("GRAPH manager is awake");
        StocksUI.OnSubmitClicked += DeletePrevoiusGraph;
        _stocksUIinstance = StocksUI.stockUIinstnace;
    }
    public void GenerateGraph()
    {
        
        _stockInfo = _stocksUIinstance.StockInfo;
        graphBars = new List<GameObject>();
        getDataForLastRefreshDay();
        CalculateFactor();
        PopulateGraph();
        StocksUI.OnSubmitClickedSuccesful -= GenerateGraph;
    }
    void getDataForLastRefreshDay()
    {
        string LastRefreshDateString=_stockInfo["Meta Data"]["3. Last Refreshed"];
        CultureInfo provider = CultureInfo.InvariantCulture;
        LastRefreshDate=DateTime.ParseExact(LastRefreshDateString, "yyyy-MM-dd", provider);
    }
    void PopulateGraph()
    {
        float offsetY=0;
        //float offsetZ = 0;
        for(int i=0;i<10;i++)
        {
            var temp=Instantiate(GraphBarPrefab, new Vector3(offsetY, 6f, 0f), Quaternion.identity);
            var tempScript=temp.GetComponent<GraphBarController>();
            print("From populate graph" + ClosingValues[i] + " " + DatesString[i]);
            tempScript.BarValue = ClosingValues[i];
            tempScript.date = DatesString[i];
            tempScript.VolumeValue = VolumeValues[i];
            tempScript.StartScaling();
            graphBars.Add(temp);
            offsetY += 55f;
            //offsetZ -= 10f;
        }
        print(graphBars.Count);
    }
    void CalculateFactor()
    {
        float maxClosing = 0;
        int maxVolume = 0;
        iteratorDate = LastRefreshDate;
        CurrDate = iteratorDate.ToString("yyyy-MM-dd");
        string ClosingPrice = _stockInfo["Time Series (Daily)"][iteratorDate.ToString("yyyy-MM-dd")]["4. close"];
        string Volume = _stockInfo["Time Series (Daily)"][iteratorDate.ToString("yyyy-MM-dd")]["6. volume"];
        print("closing Price is " + ClosingPrice);
        float tmpClosing = float.Parse(ClosingPrice);
        int tmpVolume = int.Parse(Volume);
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("Added "+CurrDate+" at index " +  i);
            tmpClosing = float.Parse(_stockInfo["Time Series (Daily)"][CurrDate]["4. close"]);
            tmpVolume = int.Parse(_stockInfo["Time Series (Daily)"][CurrDate]["6. volume"]);
            AddDataToArray(CurrDate,tmpClosing,tmpVolume, i);
            if (tmpClosing > maxClosing)
            {
                maxClosing = tmpClosing;
            }
            if (tmpVolume > maxVolume)
            {
                maxVolume = tmpVolume;
            }
            GetNextDate();
        }
        factorY = maxClosing / 100;
        factorZ = maxVolume / 100;
        print("Factor is "+factorY+" and Z is "+ factorZ);
    }
    void GetNextDate()
    {
        iteratorDate = iteratorDate.Subtract(TimeSpan.FromDays(1));
        CurrDate = iteratorDate.ToString("yyyy-MM-dd");
        while (_stockInfo["Time Series (Daily)"][CurrDate]["4. close"] == null)
        {
            iteratorDate = iteratorDate.Subtract(TimeSpan.FromDays(1));
            CurrDate = iteratorDate.ToString("yyyy-MM-dd");
        }
    }
    void AddDataToArray(string Date,float ClosingValue,int Volume,int index)
    {
        int storeAtIndex = 9 - index;
        DatesString[storeAtIndex] = Date;
        ClosingValues[storeAtIndex] = ClosingValue;
        VolumeValues[storeAtIndex] = Volume;
    }
    public void DeletePrevoiusGraph()
    {
        if(graphBars!=null)
        {
            foreach(GameObject g in graphBars)
            {
                Destroy(g);
            }
            print("deleted all graphs");
        }
    }
    private void OnDisable()
    {
        StocksUI.OnSubmitClicked -= DeletePrevoiusGraph;
    }

}
