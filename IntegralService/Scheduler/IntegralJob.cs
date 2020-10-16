using IntegralService.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegralService.Scheduler
{
    public class IntegralJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            //工作内容
            return Task.Run(() =>
            {
                RabbitMqHelper.Open("UserRegist", AddIntgralToUserAccount);
            });
        }

        private void AddIntgralToUserAccount(string UserNum)
        {
            /*
             * 查询用户的积分账户并奖励注册积分
             */
            Console.WriteLine($"用户是:{UserNum}");
        }
    }
}
