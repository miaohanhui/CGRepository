using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GifService.Models
{
    public class GifJob : IJob
    {
        private readonly RedisService _redisService;
        private readonly GifContext _gifContext;
        public GifJob(RedisService redisService, GifContext gifContext)
        {
            _redisService = redisService;
            _gifContext = gifContext;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //工作内容
            await Task.Run(() =>
            {
                var key = DateTime.Now.ToString("yyyyMMdd") + ":000001";//固定礼物ID
                //把redis保存的抢换礼物记录保存到数据库
                var userIdList = _redisService.ServiceHttpGet("GetList?key=" + key).Result;
                if (!string.IsNullOrEmpty(userIdList))
                {
                    List<string> ls = userIdList.Split(",").ToList();
                    foreach (var id in ls)
                    {
                        UserGif obj = new UserGif();
                        obj.UserId = int.Parse(id);
                        obj.GifId = 1;
                        obj.GetDate = DateTime.Now;
                        obj.DeadLine = DateTime.Now.AddDays(7);
                        _gifContext.UserGifs.Add(obj);
                    }
                    _gifContext.SaveChanges();
                }

            });
        }
    }
}
