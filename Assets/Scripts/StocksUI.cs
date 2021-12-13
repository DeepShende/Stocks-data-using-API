using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System;
using TMPro;
using UnityEngine.UI;
using System.Web;
using System.Linq;

public class StocksUI : MonoBehaviour
{
    //private static readonly APIHandlerScript _instance = new APIHandlerScript();
    public static StocksUI stockUIinstnace { get; private set; }   
    public TextMeshProUGUI SymbolText,CurrentModeText,LastDateText;
    private static string _symbol;
    public string symbol
    {
        get
        {
            return _symbol;
        }
        set
        {
            _symbol = value;
        }
    }
    public TMP_InputField EnteredText;
    public TMP_Dropdown dropdownMenu;
    public string errorMessage, LastRefreshDate,DisplayValue;
    public JSONNode StockInfo;
    static OperationsManager operationsManager;
    static GraphManager _graphManager;
    public delegate void SubmitAction();
    public static event SubmitAction OnSubmitClicked;
    public static event SubmitAction OnSubmitClickedSuccesful;
    public static event SubmitAction OnNoErrorPresent;

    private void Awake()
    {
        if(stockUIinstnace==null)
        {
            stockUIinstnace = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        var today = DateTime.Today;
        today = today.AddDays(1);
        string TodaysDate = today.ToString("yyyy-MM-dd");
        OnNoErrorPresent += OnDropDownValueChanged;
    }
    private void Start()
    {
        operationsManager = OperationsManager.GetOperationsManager();
        _graphManager = GraphManager.graphManager;
    }
    public void OnSubmitButtonPressed() 
    {
        symbol= EnteredText.text;
        symbol = symbol.ToUpper();
        APIRequestHandler apiCall = new APIRequestHandler();
        StartCoroutine(apiCall.GetRequest((NodeData)=>
        {
            StockInfo = NodeData;
            print(StockInfo["Meta Data"]["3. Last Refreshed"]);
        }
        ,"https://www.alphavantage.co", "function=&symbol=.BSE&outputsize=&apikey=", "", symbol, "", ""));
        StartCoroutine(WaitforSeconds(EventCallsMethod,3f));
        //StockInfo = await apiCall.GetRequestAsync("https://www.alphavantage.co", "function=&symbol=.BSE&outputsize=&apikey=", "", symbol, "", "");
        //EventCallsMethod();
    }
    IEnumerator WaitforSeconds(Action EventCalls,float Timer)
    {
        yield return new WaitForSeconds(Timer);
        print(StockInfo["Time Series (Daily)"]["2021-04-14"]["1. open"]);
        EventCalls();
    }
    void EventCallsMethod()
    {
        LastRefreshDate = StockInfo["Meta Data"]["3. Last Refreshed"];
        errorMessage = StockInfo["Error Message"];
        if (OnSubmitClicked != null)
        {
            OnSubmitClicked();
        }
        if (!IsErrorPresent())
        { 
            OnSubmitClickedSuccesful += _graphManager.GenerateGraph;
            CallEvents();
        }
        Debug.Log("Closing price on " + LastRefreshDate + " of " + symbol + " is " + DisplayValue);
    }
    public void OnDropDownValueChanged()
    {
        int index = dropdownMenu.value;
        switch(index)
        {
            case 1:
                //operationsManager.OnOpenSelected();
                OnSubmitClickedSuccesful += operationsManager.OnOpenSelected;
                print("subscribed to open.");
                break;
            case 2:
                //operationsManager.OnHighSelected();
                OnSubmitClickedSuccesful += operationsManager.OnHighSelected;
                print("subscribed to high.");
                break;
            case 3:
                //operationsManager.OnLowSelected();
                OnSubmitClickedSuccesful += operationsManager.OnLowSelected;
                break;
            case 4:
                //operationsManager.OnCloseSelected();
                OnSubmitClickedSuccesful += operationsManager.OnCloseSelected;
                break;
            case 5:
                //operationsManager.OnAdjustedCloseSelected();
                OnSubmitClickedSuccesful += operationsManager.OnAdjustedCloseSelected;
                break;
            case 6:
                //operationsManager.OnVolumeSelected();
                OnSubmitClickedSuccesful += operationsManager.OnVolumeSelected;
                break;
            case 7:
                //operationsManager.OnDividendAmountSelected();
                OnSubmitClickedSuccesful += operationsManager.OnDividendAmountSelected;
                break;
            case 8:
                //operationsManager.OnSplitCoeffcientSelected();
                OnSubmitClickedSuccesful += operationsManager.OnSplitCoeffcientSelected;
                break;
            default:
                //operationsManager.OnDefaultSelected();
                OnSubmitClickedSuccesful += operationsManager.OnDefaultSelected;
                break;

        }
    }
    public void CallEvents()
    {
        if (!IsErrorPresent())
        {
            OnNoErrorPresent();
            OnSubmitClickedSuccesful();
        }
    }
    public bool IsErrorPresent()
    {
        if (errorMessage != null || StockInfo == null)
        {
            SymbolText.text = "Unable to fetch data!";
            LastDateText.text = "Please check the symbol";
            CurrentModeText.text = " ";
            //print(apiHandlerScript.errorMessage);
            return true;
        }
        SymbolText.text = symbol;
        LastDateText.text = "Last refreshed on: " + LastRefreshDate;
        return false;
    }
}
