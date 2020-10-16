using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.InMemory.ValueGeneration.Internal;
using StackExchange.Redis;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RedisService.Controllers
{
    [Route("api/Redis")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IDatabase _redis;
        public RedisController(RedisHelper client)
        {
            _redis = client.GetDatabase();
        }

        // GET: api/<RedisController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public class PostUserMsg
        {
            public string value { set; get; }
            public string userMsg { set; get; }
        }

        [HttpPost]
        [Route("SetUserData")]
        public string SetUserData([FromBody]PostUserMsg postUserMsg)
        {
            try
            {
                _redis.SetAdd("UserNum:Set", postUserMsg.value);//账号保存到set集合

                string[] arr = postUserMsg.userMsg.Split("|");
                for (var i = 0; i <= arr.Length; i++)
                {
                    string[] msg = arr[i].Split(":");
                    
                    _redis.HashSet("UserMsg:" + postUserMsg.value, msg[0], msg[1]);//把账号信息保存到Hash
                    _redis.KeyExpire("UserMsg:" + postUserMsg.value, DateTime.Now.AddDays(1)); //设置过期
                }

                //_redis.KeyExpire(key, DateTime.Now.AddSeconds(60)); 设置过期
                return "success";
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
        }
        [HttpGet]
        [Route("GetUserData")]
        public string GetUserData(string value)
        {
            try
            {
                if (_redis.SetContains("UserNum:Set", value))
                {
                    //账号已经存在redis 的账号集合
                    return value;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
        }
        // GET api/<RedisController>/5
        [HttpGet("{id}")]
        public string Get(string key)
        {
            var value = _redis.StringGet(key);
            return value;
        }

        // POST api/<RedisController>
        [HttpPost]
        public string Post(string key, string value)
        {
            try
            {
                _redis.StringSet(key, value);
                return "success";
            }
            catch(Exception ex)
            {
                return "error:" + ex.Message;
            }
        }

        // PUT api/<RedisController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RedisController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost]
        [Route("RobGif")]
        public async Task<RobGifResult> RobGif([FromBody]PostRobGif postRobGif)
        {
            try
            {
                string dateStr = DateTime.Now.ToString("yyyyMMdd");
                string lkey = dateStr + ":" + postRobGif.GifId.ToString("000000");
                long len = await _redis.ListLengthAsync(lkey);
                if (len >= 30)//30为设定的限抢礼物数
                {
                    return new RobGifResult { Message = "礼物已被抢空", leftCount = 0 };
                    
                }
                else
                {
                    len = await _redis.ListLeftPushAsync(lkey, postRobGif.UserId);
                    
                    return new RobGifResult { Message = "恭喜！抢换成功", leftCount = (30 - (int)len) };
                }
            }
            catch (Exception ex)
            {
                return new RobGifResult { Message = "错误:" + ex.Message, leftCount = -1 };
            }
        }

        public class PostRobGif
        {
            public int UserId { set; get; }
            public int GifId { set; get; }
        }

        public class RobGifResult
        { 
            public string Message { set; get; }
            public int leftCount { set; get; }
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<string> GetList(string key)
        {
            string userIdList = "";//抢到礼物ID为key的用户的ID列表
            var len = await _redis.ListLengthAsync(key);
            while (len > 0)
            { 
                var value = await _redis.ListRightPopAsync(key);
                userIdList += value.ToString() + ",";
                len--;
            }
            return userIdList;
        }
    }
}
