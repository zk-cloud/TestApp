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
        public void Execute(IJobExecutionContext context)
        {
            Task.Factory.StartNew(() =>
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