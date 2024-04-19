using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Diagnostics;

namespace TestApp.Controllers
{
    public class ChannelController : Controller
    {
        public ActionResult Channel()
        {
            string queueName = "queue_demo_one";
            string exchangeName = "exchange_demo_one";

            var factory = new ConnectionFactory()
            {
                HostName = "152.136.98.193",//ip
                Port = 5672,//端口，15672 是 web 端管理用的，5672 是用于客户端与消息中间件之间可以传递消息
                UserName = "admin",//用户名
                Password = "1433223"//密码
            };

            var connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();

            //定义消费者
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, args) =>
            {
                var body = args.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Debug.WriteLine($"消费者接收消息 {message}");
            };

            //启动消费者
            channel.BasicConsume(queue: queueName,
                autoAck: true,//自动确认
                consumer: consumer);

            //处理完消息后，保持程序继续运行，可以继续接收消息
            Console.ReadLine();

            return null;
        }
    }
}