using System;

namespace BackEndWeek1
{   
    public class Program
    {
        // 指令長度
        public static readonly int COMMAND_LEN = 2;        

        public static void Main(string[] args)
        {
            // 程式能否結束
            bool canEnd = false;

            // 宣告 一個檔案系統
            FileSystem fileSystem = new FileSystem();         

            // 進入程式執行迴圈
            while (!canEnd)
            {
                Console.WriteLine("指令: -i \"Test Data\\data.csv\"");
                Console.WriteLine("指令: -s 列出所有不重複的股票代號");
                Console.WriteLine("指令: -q 股票代號 或 All 輸出該股票資訊");
                Console.WriteLine("指令: -t 股票代號 或 All 輸出Top50買賣超卷商");
                Console.WriteLine("指令: -o 離開程式");

                // 輸入            
                string input = Console.ReadLine();

                // 從輸入的字串 取出指令
                string command = input.Substring(0, COMMAND_LEN);

                // 判斷 指令
                switch (command)
                {
                    case "-i":
                        {
                            // 從"開始取路徑字串
                            string path = input.Substring(input.IndexOf("\""));

                            // 去掉兩個" ", 取出正確的路徑
                            path = path.Replace("\"", "");

                            // 讀檔
                            fileSystem.Loading(path);

                            break;
                        }
                    case "-s":
                        {
                            // 印出所有不重複的股票
                            fileSystem.PrintAllStocks();

                            break;
                        }
                    case "-q":
                        {
                            if(!input.Equals("-q"))
                            {
                                // 從指令 + 空格 之後取出範圍 
                                string range = input.Substring(COMMAND_LEN + 1);

                                // 根據範圍找資料
                                fileSystem.Search(range);
                            }                            

                            break;
                        }
                    case "-t":
                        {
                            if (!input.Equals("-t"))
                            {
                                // 從指令 + 空格 之後取出範圍 
                                string range = input.Substring(COMMAND_LEN + 1);

                                // 根據範圍找買賣超資料
                                fileSystem.Top50BuySellOver(range);
                            }                         

                            break;
                        }
                    case "-o":
                        {
                            canEnd = true;

                            break;
                        }   
                }
            }

            Console.WriteLine("Thank you!");            
        }
    }
}
