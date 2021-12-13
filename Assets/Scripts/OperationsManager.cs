using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SimpleJSON;

public class OperationsManager
{
    private static readonly OperationsManager _instance = new OperationsManager();
    StocksUI _stocksUIinstance=StocksUI.stockUIinstnace;
    JSONNode _stockInfo;
    string _errorMessage, _lastRefreshDate, _displayValue;


    private enum Modes
    {
        Select,
        open,
        high,
        low,
        close,
        adjusted_close,
        volume,
        dividend_amount,
        split_coefficient
    }
    private OperationsManager()
    {
        StocksUI.OnSubmitClicked += setValuesFromAPIHandler;
        StocksUI.OnSubmitClicked += PopulateList;
    }
    public static OperationsManager GetOperationsManager()
    {
        return _instance;
    }
    public void setValuesFromAPIHandler()
    {
        _stockInfo = _stocksUIinstance.StockInfo;
        _errorMessage = _stocksUIinstance.errorMessage;
        _lastRefreshDate = _stocksUIinstance.LastRefreshDate;
        _displayValue = _stocksUIinstance.DisplayValue;
    }
    public void PopulateList()
    {
        List<string> ModesList = new List<string>(Enum.GetNames(typeof(Modes)).ToList());
        _stocksUIinstance.dropdownMenu.AddOptions(ModesList);
        StocksUI.OnSubmitClicked -= PopulateList;
    }
    public void OnDefaultSelected()
    {
        if (_errorMessage == null)
        {
            _displayValue = "Please select mode!";
            SetDisplayPrice();
            StocksUI.OnSubmitClickedSuccesful -= OnDefaultSelected;
        }
    }
    public void OnOpenSelected()
    {
        if (_errorMessage == null)
        {
            _displayValue = _stockInfo["Time Series (Daily)"][_lastRefreshDate]["1. open"];
            SetDisplayPrice();
            StocksUI.OnSubmitClickedSuccesful -= OnOpenSelected;
        }
    }
    public void OnHighSelected()
    {
        if (_errorMessage == null)
        {
            _displayValue = _stockInfo["Time Series (Daily)"][_lastRefreshDate]["2. high"];
            SetDisplayPrice();
            StocksUI.OnSubmitClickedSuccesful -= OnHighSelected;
    }
    }
    public void OnLowSelected()
    {
        if (_errorMessage == null)
        {
            _displayValue = _stockInfo["Time Series (Daily)"][_lastRefreshDate]["3. low"];
            SetDisplayPrice();
            StocksUI.OnSubmitClickedSuccesful -= OnLowSelected;
        }
    }
    public void OnCloseSelected()
    {
        if (_errorMessage == null)
        {
            _displayValue = _stockInfo["Time Series (Daily)"][_lastRefreshDate]["4. close"];
            SetDisplayPrice();
            StocksUI.OnSubmitClickedSuccesful -= OnCloseSelected;
        }
    }
    public void OnAdjustedCloseSelected()
    {
        if (_errorMessage == null)
        {
            _displayValue = _stockInfo["Time Series (Daily)"][_lastRefreshDate]["5. adjusted close"];
            SetDisplayPrice();
            StocksUI.OnSubmitClickedSuccesful -= OnAdjustedCloseSelected;
        }
    }
    public void OnVolumeSelected()
    {
        if (_errorMessage == null)
        {
            _displayValue = _stockInfo["Time Series (Daily)"][_lastRefreshDate]["6. volume"];
            SetDisplayPrice();
            StocksUI.OnSubmitClickedSuccesful -= OnVolumeSelected;
        }
    }
    public void OnDividendAmountSelected()
    {
        if (_errorMessage == null)
        {
            _displayValue = _stockInfo["Time Series (Daily)"][_lastRefreshDate]["7. dividend amount"];
            SetDisplayPrice();
            StocksUI.OnSubmitClickedSuccesful -= OnDividendAmountSelected;
        }
    }
    public void OnSplitCoeffcientSelected()
    {
        if (_errorMessage == null)
        {
            _displayValue = _stockInfo["Time Series (Daily)"][_lastRefreshDate]["8. split coefficient"];
            SetDisplayPrice();
            StocksUI.OnSubmitClickedSuccesful -= OnSplitCoeffcientSelected;
        }
    }
    public void SetDisplayPrice()
    {        
        _stocksUIinstance.CurrentModeText.text = _displayValue;
    }

}
