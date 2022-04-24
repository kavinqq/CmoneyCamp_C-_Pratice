using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BackEndWeek1
{
    /// <summary>
    /// 資料索引
    /// </summary>
    public enum DataIndex
    {
        DealDate = 0,
        StockID = 1,
        StockName = 2,
        SecBrokerID = 3,
        SecBrokerName = 4,
        Price = 5,
        BuyQty = 6,
        SellQty = 7,
    }

    /// <summary>
    /// 股票資料csv 要讀取的欄位
    /// </summary>
    public class StockData
    {      
        // 內部類別 每一筆交易 紀錄的格式
        private class Data
        {                    
            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="data"> 傳入的一行資料</param>
            public Data (string[] data)
            {
                //根據 資料索引 把存進來的資料 依照索引放好
                DealDate = data[(int)DataIndex.DealDate];

                StockID = data[(int)DataIndex.StockID];

                StockName = data[(int)DataIndex.StockName];

                SecBrokerID = data[(int)DataIndex.SecBrokerID];

                SecBrokerName = data[(int)DataIndex.SecBrokerName];

                Price = decimal.Parse(data[(int)DataIndex.Price]);

                BuyQty = int.Parse(data[(int)DataIndex.BuyQty]);

                SellQty = int.Parse(data[(int)DataIndex.SellQty]);             
            }

            /// <summary>
            /// 交易日期
            /// </summary>
            public string DealDate { get; set; }

            /// <summary>
            /// 股票代號
            /// </summary>
            public string StockID { get; set; }

            /// <summary>
            /// 股票名稱
            /// </summary>
            public string StockName { get; set; }

            /// <summary>
            /// 卷商代號
            /// </summary>
            public string SecBrokerID { get; set; }

            /// <summary>
            /// 卷商名稱
            /// </summary>
            public string SecBrokerName { get; set; }

            /// <summary>
            /// 股價
            /// </summary>
            public decimal Price { get; set; }

            /// <summary>
            /// 買進數量
            /// </summary>
            public int BuyQty { get; set; }

            /// <summary>
            /// 賣出數量
            /// </summary>
            public int SellQty { get; set; }


            /// <summary>
            /// 輸出字串(csv格式)
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(DealDate).Append(",");
                sb.Append(StockID).Append(",");
                sb.Append(StockName).Append(",");
                sb.Append(SecBrokerID).Append(",");
                sb.Append(SecBrokerName).Append(",");
                sb.Append(Price).Append(",");
                sb.Append(BuyQty).Append(",");
                sb.Append(SellQty);

                return sb.ToString();
            }
        }

        // 存有所有交易的List (就是把data.csv放進來)
        private List<Data> allData;        

        /// <summary>
        /// 建構子
        /// </summary>
        public StockData()
        {
            allData = new List<Data>();            
        }       

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="data">一筆交易資料</param>
        public void Add(string[] data)
        {
            allData.Add(new Data(data));            
        }

        /// <summary>
        /// 印出所有資訊
        /// </summary>
        /// <returns></returns>
        public void PrintAllStock()
        {       
            // 用using是因為 可以在區塊結束時 自動呼叫 Dispose() 去釋放非控資源(記憶體/物件/檔案...)
            using (var streamWriter = new StreamWriter("列出股票代號.csv", false, System.Text.Encoding.UTF8))
            {
                // 字典物件 => 我用他的ContainsKey 幫我過濾重複鍵
                Dictionary<string, string> nonRepeatStock = new Dictionary<string, string>();

                // 寫入標題列
                streamWriter.WriteLine("StocksID,StockName");
                streamWriter.WriteLine("All,All");

                // 因為這張表 primary key是股票ID,所以我以股票ID為主
                for (int i = 0; i < allData.Count; i++)
                {
                    // 如果還沒有紀錄該ID
                    if (!nonRepeatStock.ContainsKey(allData[i].StockID))
                    {
                        // 由於不確定C# List[i] 是否每次都是BigO(1) 就先紀錄下來
                        var stockID = allData[i].StockID;
                        var stockName = allData[i].StockName;
                        
                        // 字典加入這筆資料
                        nonRepeatStock.Add(stockID, stockName);

                        // csv寫入這筆資料
                        streamWriter.WriteLine($"{stockID},{stockName}");
                    }
                }              
              
                // 印出有幾個不重複的股票代號
                Console.WriteLine($"共有{nonRepeatStock.Count}個股票代號");
            }            
        }

        /// <summary>
        /// 查詢股票
        /// </summary>
        /// <param name="range">要找的範圍</param>
        public void Search(string range)
        {
            Dictionary<string, List<string>> resultDic = new Dictionary<string, List<string>>();            

            // 建立新的writer 輸出股票查詢1結果.csv
            using (var streamWriter = new StreamWriter("股票查詢1結果.csv", false, System.Text.Encoding.UTF8))
            {      
                // 如果範圍是All
                if (range.Equals("All"))
                {    
                    //遍尋依次所有的資料
                    for (int i = 0; i < allData.Count; i++)
                    {
                        var data = allData[i];

                        // 如果還沒有紀錄該ID
                        if (!resultDic.ContainsKey(data.StockID))
                        {          
                            // 加入該AD 並new List物件
                            resultDic.Add(data.StockID, new List<string>());                            

                            // 把資料字串放進去List
                            resultDic[data.StockID].Add(data.ToString());
                        }
                        else
                        {
                            // 把資料字串直接放進去List
                            resultDic[data.StockID].Add(data.ToString());
                        }
                    }
                } 
                else
                {                   
                    // 先幫字典建立好目錄
                    foreach(string keyID in range.Split(","))
                    {
                        resultDic.Add(keyID, new List<string>());
                    }

                    // 遍尋所有的資料
                    for(int i = 0; i < allData.Count; i++)
                    {                        
                        var data = allData[i];

                        // 如果該資料的ID已經有存在key裡面 就把資料加進去
                        if (resultDic.ContainsKey(data.StockID))
                        {
                            resultDic[data.StockID].Add(data.ToString());
                        }
                    }
                }

                // csv的標題
                streamWriter.WriteLine("DealDate,StocksID,StockName,SecBrokerID,SecBrokerName,Price,ByuQty,SellQty");
                
                // 跑一次字典 把 對應key的所有字串寫入csv
                foreach(string key in resultDic.Keys)
                {
                    // 對應key的 List<string> 內的每一筆資料 => csv的一行
                    foreach (string data in resultDic[key])
                    {                        
                        streamWriter.WriteLine(data);
                    }
                }
            }

            //建立新的writer 輸出股票查詢2結果.csv
            using (var streamWriter = new StreamWriter("股票查詢2結果.csv", false, System.Text.Encoding.UTF8))
            {
                // csv的標題
                streamWriter.WriteLine("StocksID,StockName,BuyTotal,SellTotal,BuySellOver,AvgPrice,SecBrokerCnt");

                // 輸出查詢1時 我把相同ID 的所有資料 存在同一個List => 直接從這裡面撈資料
                foreach (string key in resultDic.Keys)
                {
                    string stocksID = "";
                    string stockName = "";

                    int buyTotal = 0;
                    int sellTotal = 0;
                    int buySellOver = 0;                    

                    // 1. 我看網路上說 算錢用浮點 遲早被人扁 => 所以我用decimal
                    // 2. 以前也寫過題目 0.2 * 0.2 跟 0.04 比大小 兩者是不同的 => 浮點數不精確
                    decimal totalPrice = 0;
                    decimal avgPrice;

                    // 以卷商名稱為key 的 map => 用來計算卷商數量
                    Dictionary<string, int> secBrokerDic = new Dictionary<string, int>();      

                    // 對應key的 List<string> 內的每一筆資料 => csv的一行
                    foreach (string data in resultDic[key])
                    {
                        // 把原本要直接輸出成csv的字串,分開來取資料
                        string[] eachData = data.Split(",");
                                                
                        stocksID = eachData[(int)DataIndex.StockID];
                                                
                        stockName = eachData[(int)DataIndex.StockName];
                                                
                        buyTotal += int.Parse(eachData[(int)DataIndex.BuyQty]);

                        sellTotal += int.Parse(eachData[(int)DataIndex.SellQty]);

                        buySellOver = buyTotal - sellTotal;

                        totalPrice = decimal.Parse(eachData[(int)DataIndex.Price]) * (int.Parse(eachData[(int)DataIndex.BuyQty]) + int.Parse(eachData[(int)DataIndex.SellQty]));

                        // 如果卷商名稱沒有紀錄
                        if (!secBrokerDic.ContainsKey(eachData[(int)DataIndex.SecBrokerName]))
                        {
                            secBrokerDic.Add(eachData[(int)DataIndex.SecBrokerName], 1);                            
                        }                        
                    }                                        

                    // 平均 = 總購入或賣出價  / 買賣總和
                    if(buyTotal + sellTotal != 0)
                    {
                        avgPrice = totalPrice / (decimal)(buyTotal + sellTotal);
                    } else
                    {
                        avgPrice = 0;
                    }

                    streamWriter.WriteLine($"{stocksID},{stockName},{buyTotal},{sellTotal},{buySellOver},{avgPrice},{secBrokerDic.Keys.Count}");
                }
            }
        }


        /// <summary>
        /// 買賣超Top50
        /// </summary>
        public void Top50BuySellOver(string range)
        {
            // 建立 買賣超Top50.csv
            using(var streamWriter = new StreamWriter("買賣超Top50.csv", false, System.Text.Encoding.UTF8))
            {
                // (key1 => StockID  key2 => 卷商名稱)  int=> 買賣超總和
                // 我確定 stockID 唯一性  && 再進一步確定 該ID下value 的 卷商名稱唯一性   
                Dictionary<string, Dictionary<string, int>> buySellOverDic = new Dictionary<string, Dictionary<string, int>>();                

                // 如果搜尋範圍是All
                if(range.Equals("All"))
                {
                    // 遍尋一次所有的資料
                    foreach (var data in allData)
                    {
                        // 如果沒有這個ID紀錄
                        if (!buySellOverDic.ContainsKey(data.StockName))
                        {
                            buySellOverDic.Add(data.StockName, new Dictionary<string, int>());
                            buySellOverDic[data.StockName].Add(data.SecBrokerName, data.BuyQty - data.SellQty);
                        }
                        else
                        {
                            // 如果這個 ID 裡面沒有這個 卷商名稱紀錄的話
                            if (!buySellOverDic[data.StockName].ContainsKey(data.SecBrokerName))
                            {
                                buySellOverDic[data.StockName].Add(data.SecBrokerName, data.BuyQty - data.SellQty);
                            }
                            else // 否則更新 這個ID 裡面的 卷商買賣超紀錄
                            {
                                buySellOverDic[data.StockName][data.SecBrokerName] += data.BuyQty - data.SellQty;
                            }
                        }
                    }
                } else //如果有限制搜尋範圍 (By stockID)
                {                  
                    // 遍尋所有的資料
                    foreach(var data in allData)
                    {
                        // 如果目前給的範圍 有這個ID的話  納入這筆資料
                        if(range.IndexOf(data.StockID) != -1)
                        {
                            // 如果這個ID 對應的股票名稱 尚未紀錄
                            if (!buySellOverDic.ContainsKey(data.StockName))
                            {                            
                                // Key:新增股票名稱   Value:新增一個字典到
                                buySellOverDic.Add(data.StockName, new Dictionary<string, int>());

                                // Value字典的Key是 卷商名稱 , Value新增一筆買賣超
                                buySellOverDic[data.StockName].Add(data.SecBrokerName, data.BuyQty - data.SellQty);
                            }
                            else // 如果這個ID 對應的股票名稱 已經紀錄
                            {
                                // 內部Dictionary 的Key 沒有這個卷商名稱
                                if (!buySellOverDic[data.StockName].ContainsKey(data.SecBrokerName))
                                {
                                    buySellOverDic[data.StockName].Add(data.SecBrokerName, (data.BuyQty - data.SellQty));
                                }
                                else // 內部Dictionary 的Key 經有這個卷商名稱
                                {
                                    buySellOverDic[data.StockName][data.SecBrokerName] += data.BuyQty - data.SellQty;
                                }
                            }
                        }
                    }
                }
                
                // 寫入title資料
                streamWriter.WriteLine("StockName,SecBrokerName,BuySellOver");                

                // 跑一次 買賣超字典資料 取 Value裡面的字典
                foreach (var key in buySellOverDic.Keys)
                {
                    // 使用LINQ 轉成一個 List<KeyValuePair>  key紀錄 卷商名稱 / value紀錄 買賣超的值
                    var sortBuySellOver = buySellOverDic[key].ToList();

                    // 使用LINQ排序 (由大到小排序)
                    sortBuySellOver.Sort((buySellOver1, buySellOver2) => buySellOver2.Value.CompareTo(buySellOver1.Value));

                    // 如果 總共買賣超紀錄 筆數 >= 50 那找到50筆 否則 只找到紀錄的最大值
                    int listBound = (sortBuySellOver.Count >= 50) ? 50 : sortBuySellOver.Count;    

                    // 大到小前0筆
                    for (int i = 0; i < listBound; i++)
                    {       
                        // 0不納入,而且是從大到小 如果這個 <= 0 下面全都 <= 0 
                        if (sortBuySellOver[i].Value <= 0 )
                        {
                            break;
                        }                        
                        streamWriter.WriteLine($"{key},{sortBuySellOver[i].Key},{sortBuySellOver[i].Value}");
                    }                    

                    // 反轉陣列(變成由小到大)
                    sortBuySellOver.Reverse();
                    
                    // 小到大50筆
                    for (int i = 0; i < listBound; i++)
                    {
                        // 0不納入,而且是從小到大 如果這個 >= 0 下面全都 >= 0 
                        if (sortBuySellOver[i].Value >= 0)
                        {
                            break;
                        }

                        streamWriter.WriteLine($"{key},{sortBuySellOver[i].Key},{sortBuySellOver[i].Value}");
                    }
                }         
            } 
        }
    }
}
