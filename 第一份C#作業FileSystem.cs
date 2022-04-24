using System;
using System.IO;
using System.Diagnostics;

namespace BackEndWeek1
{
    public class FileSystem
   {
        // stock資料庫
        private StockData stockData;

        // 計時器
        private Stopwatch stopwatch;       

        /// <summary>
        /// 建構子(初始化欄位物件)
        /// </summary>
        public FileSystem()
        {
            stockData = new StockData();

            stopwatch = new Stopwatch();           
        }  

        /// <summary>
        /// 讀取檔案
        /// </summary>
        /// <param name="path"> 檔案路徑 </param>
        public void Loading(string path)
        {     
            // 有幾列標題
            int titleCount = 0;

            // 設定好相對路徑
            path = @"..\..\" + @path;

            Console.WriteLine("開始讀檔");

            // 開始計時
            stopwatch.Start();
            
            try
            {
                // Debug: 測試路徑
                Console.WriteLine(path);

                // 使用using 能夠自動呼叫 StreamReader的Dispose
                using (var reader = new StreamReader(path))
                {
                    // reader還有資料能讀取
                    while (!reader.EndOfStream)
                    {
                        // 讀取一行資料
                        dynamic data = reader.ReadLine();

                        // 根據.csv格式 用,來區分 每一個資料
                        data = data.Split(",");

                        // 跳過title 不輸入
                        if (titleCount <= 0)
                        {
                            titleCount++;
                            continue;
                        }

                        // stockData 加入一筆新資料
                        stockData.Add(data);
                    }

                    // 印出時間
                    PrintTime(stopwatch,"讀檔完畢");
                }
            }
            catch (IOException e)
            {
                PrintTime(stopwatch,"讀檔失敗");

                Console.WriteLine("找不到檔案!");
            }
        }

        /// <summary>        
        /// 列出所有股票代號、名稱
        /// </summary>
        public void PrintAllStocks()
        {
            // 開始計時
            stopwatch.Start();

            // 呼叫stockData 輸出所有股票的csv
            stockData.PrintAllStock();

            // 跑完後印出時間
            PrintTime(stopwatch, "列出完畢");            
        }

        /// <summary>
        /// 根據範圍找股票資料
        /// </summary>
        /// <param name="range">要查詢的範圍</param>
        public void Search(string range)
        {
            // 開始計時
            stopwatch.Start();

            // 呼叫資料Seach 輸出兩張csv
            stockData.Search(range);

            // 跑完後印出時間
            PrintTime(stopwatch, "股票查詢");
        }


        /// <summary>
        /// 買賣超Top50
        /// </summary>
        public void Top50BuySellOver(string range)
        {
            stopwatch.Start();

            stockData.Top50BuySellOver(range);

            PrintTime(stopwatch, "買賣超Top50");
        }


        /// <summary>
        /// 印出時間
        /// </summary>
        /// <param name="stopwatch">現在的計時器</param>
        public void PrintTime(Stopwatch stopwatch, string hint)
        {
            TimeSpan timeSpan = stopwatch.Elapsed;
            
            // 這次時間用完了 reset
            stopwatch.Reset();

            // 印出時間
            Console.WriteLine("{0} 耗時 - {1:00}:{2:00}.{3:000}",hint, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }
    }
}
