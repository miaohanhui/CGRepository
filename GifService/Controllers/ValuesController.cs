using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GifService.Models;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace GifService.Controllers
{
    [Route("api/Values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly GifContext _context;
        private IScheduler _scheduler;
        
        public ValuesController(ISchedulerFactory schedulerFactory, GifContext context)
        {
            this._schedulerFactory = schedulerFactory;
            this._context = context;          
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            //1、通过调度工厂获得调度器
            _scheduler = await _schedulerFactory.GetScheduler();
            //2、开启调度器
            await _scheduler.Start();
            //3、创建一个触发器
            var trigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever())//每300秒执行一次
                            .Build();
            //4、创建任务
            var jobDetail = JobBuilder.Create<GifJob>()
                            .WithIdentity("job", "group")
                            .Build();
            //5、将触发器和任务器绑定到调度器中
            await _scheduler.ScheduleJob(jobDetail, trigger);


            //执行rabbitmq
            RabbitMqHelper.Open("UserResgit_Gif", AddGifToUserAccount);

            return new string[] { "value1", "value2" };
        }

        private void AddGifToUserAccount(string UserMsg)
        {
            /*
             * 查询用户的积分账户并赠送首次注册礼物
             */
            string[] arr = UserMsg.Split("|");
            UserGif usergif = new UserGif();

            usergif.UserId = int.Parse( arr[0].Split(":")[1]);
            usergif.GifId = (int)GifType.RegistGif;
            usergif.GetDate = DateTime.Now;
            usergif.DeadLine = DateTime.Now.AddDays(7);
            _context.UserGifs.Add(usergif);
            _context.SaveChanges();

            Console.WriteLine($"成功为新创建的账号赠送礼物");
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
