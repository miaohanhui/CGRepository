using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockMarket.Models
{
    public static class RabbitMqHelper
    {
        private static ConnectionFactory connectionFactory;//rabbit连接工厂
        private static IConnection connection;//rabbit连接
        private static IModel channel;//连接频道
        private static string exchangeName = "UserManage";
        private static string exchangeType = "fanout";//交换机类型
        static RabbitMqHelper()
        {
            connectionFactory = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                UserName = "guest",
                Password = "guest"
            };
        }

        private static void Open()
        {
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
        }

        private static void Close()
        {
            channel.Close();
            connection.Close();
        }

        public static void SendFanoutMsg(string Message, string QueueName)
        {
            Open();
            channel.ExchangeDeclare(exchangeName, exchangeType, false, false, null);//声明交换机         
            var bytes = Encoding.UTF8.GetBytes(Message);
            channel.BasicPublish(exchangeName, "", null, bytes);
            Console.WriteLine($"服务器发送消息{Message}");
            Close();
        }
    }
}
