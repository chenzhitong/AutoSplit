using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoSplit
{
    class Program
    {
        static void Main(string[] args)
        {
            //初始金额
            var initConfirmd = Tools.GetBalance();
            if(initConfirmd == 0)
            {
                Console.WriteLine("余额不足");
                Console.ReadLine();
                return;
            }
            //分10批转账
            for (int i = 1; i <= 10; i++)
            {
                //实际金额
                var confirmd = Tools.GetBalance();
                //每次转账金额
                var value = initConfirmd / (decimal)Math.Pow(2, i);
                for (int j = 1; j <= Math.Pow(2, i); j++)
                {
                    if (Tools.Send("AcdB……钱包地址", value))
                    {
                        Console.WriteLine($"{i} 转账成功：{value}\t{j}");
                    }
                }
                confirmd = Tools.GetBalance();
                while (confirmd < initConfirmd)
                {
                    Thread.Sleep(3000);
                    confirmd = Tools.GetBalance();
                    Console.WriteLine($"{i} 已确认{confirmd}/{initConfirmd}");
                }
                Console.WriteLine($"第{i}批转账结束");
            }
            Console.WriteLine($"测试结束");
            Console.ReadLine();
        }
    }
}
