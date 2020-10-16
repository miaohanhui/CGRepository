using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegralService.Models;
using IntegralService.Scheduler;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace IntegralService.Controllers
{
    [Route("api/Values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IntegralContext _context;
        private IScheduler _scheduler;
        public ValuesController(ISchedulerFactory schedulerFactory, IntegralContext context)
        {
            this._schedulerFactory = schedulerFactory;
            this._context = context;
        }

        [HttpGet]
        public async Task<string[]> Get()
        {
            ////1、通过调度工厂获得调度器
            //_scheduler = await _schedulerFactory.GetScheduler();
            ////2、开启调度器
            //await _scheduler.Start();
            ////3、创建一个触发器
            //var trigger = TriggerBuilder.Create()
            //                .WithSimpleSchedule(x => x.WithIntervalInSeconds(60).RepeatForever())//每两秒执行一次
            //                .Build();
            ////4、创建任务
            //var jobDetail = JobBuilder.Create<IntegralJob>()
            //                .WithIdentity("job", "group")
            //                .Build();
            ////5、将触发器和任务器绑定到调度器中
            //await _scheduler.ScheduleJob(jobDetail, trigger);

            RabbitMqHelper.Open("UserResgit_Integral", AddIntgralToUserAccount);

            return await Task.FromResult(new string[] { "value1", "value2" });
        }
        private void AddIntgralToUserAccount(string UserMsg)
        {            
            /*
             * 查询用户的积分账户并奖励注册积分
             */
            string[] arr = UserMsg.Split("|");
            Account account = new Account();
            account.UserId = int.Parse( arr[0].Split(":")[1]);
            account.IsFirstCreate = 1;
            account.Integral = 50;
            account.AccountNum = arr[1].Split(":")[1];
            _context.Accounts.Add(account);
            _context.SaveChanges();

            Console.WriteLine($"成功为账号创建积分账户：{account.AccountNum}");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
