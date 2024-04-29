using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TestApp.Jobs
{
    public class TestJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                Test();
            });
        }

        public void Test()
        {
            Console.WriteLine("Hello Quartz.Net");
        }
    }
}