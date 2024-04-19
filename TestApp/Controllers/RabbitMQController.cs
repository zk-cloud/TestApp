using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Razor.Text;

namespace TestApp.Controllers
{
    public class RabbitMQController : Controller
    {
        public ActionResult One(int count)
        {
            string queueName = "queue_demo_one";
            string exchangeName = "exchange_demo_one";

            //先创建连接
            var factory = new ConnectionFactory()
            {
                HostName = "152.136.98.193",//ip
                Port = 5672,//端口，15672 是 web 端管理用的，5672 是用于客户端与消息中间件之间可以传递消息
                UserName = "admin",//用户名
                Password = "1433223"//密码
            };

            //打开连接
            var connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();

            //定义队列
            channel.QueueDeclare(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            //定义交换机
            channel.ExchangeDeclare(exchange: exchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: null);

            //将队列绑定到交换机上
            channel.QueueBind(queue: queueName,
                exchange: exchangeName,
                routingKey: string.Empty,
                arguments: null);

            //发送队列
            for (int i = 0; i < count; i++)
            {
                string message = $"Task {i}";
                byte[] body = Encoding.UTF8.GetBytes(message);

                //发送消息
                channel.BasicPublish(exchange: exchangeName,
                    routingKey: string.Empty,
                    basicProperties: null,
                    body: body);

                Console.WriteLine($"消息：{message} 已发送");
            }

            return null;
        }
    }
}