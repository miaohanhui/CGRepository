using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegralService.Models
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

        private static void Close()
        {
            channel.Close();
            connection.Close();
        }

        public static void Open(string queueName, Action<string> action)
        {
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            channel.ExchangeDeclare(exchangeName, exchangeType, false, false);
            channel.QueueDeclare(queueName, false, false, false, null);//持久化，排他性，是否自动删除
            channel.QueueBind(queueName, exchangeName, "");//交换机与队列绑定
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                if (action != null)
                    action(message);
                channel.BasicAck(ea.DeliveryTag, true);
            };
            channel.BasicConsume(queueName, false, consumer);//启动消费者
            Console.WriteLine("积分消费者已启动");
            
        }
    }
}
