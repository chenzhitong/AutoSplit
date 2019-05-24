using System;
using System.IO;
using System.Threading;

namespace AutoSplitCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var address = new string[0];
            try
            {
                address = File.ReadAllLines("address.txt");
            }
            catch (Exception)
            {
                Console.WriteLine("请创建 address.txt 文件，每一行为钱包中的一个地址，支持多个地址。仅有一个地址时文件的内容就是地址本身");
                Console.ReadLine();
            }
            var random = new Random();

            //初始金额
            var initConfirmd = Tools.GetBalance();
            if (initConfirmd == 0)
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
                var value = (int)(initConfirmd / (decimal)Math.Pow(2, i));
                for (int j = 1; j <= Math.Pow(2, i); j++)
                {
                    var index = random.Next(0, address.Length);
                    var index2 = random.Next(0, address.Length);
                    if (Tools.Send(address[index], value, address[index2]))
                    {
                        Console.WriteLine($"{i} 转账成功：{address[index]} {value}\t{j}");
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
