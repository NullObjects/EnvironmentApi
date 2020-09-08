using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnvironmentApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class EnvironmentController : ControllerBase
    {
        private readonly IEnvironment _environment;

        public EnvironmentController(IEnvironment environment)
        {
            this._environment = environment;
        }

        /// <summary>
        /// 获取最近时间数据
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        [HttpGet("{span}")]
        public ObjectResult Get(string span)
        {
            var start = DateTime.Now;
            var end = DateTime.Now;
            switch (span)
            {
                case "hour":
                    start = start.AddHours(-1);
                    break;
                case "day":
                    start = start.AddDays(-1);
                    break;
                case "week":
                    start = start.AddDays(-7);
                    break;
                default:
                    return new ObjectResult(_environment.Select().OrderByDescending(x => x.RecordTime).First());
            }

            return new ObjectResult(_environment.Select()
                .Where(x => x.RecordTime >= start && x.RecordTime <= end));
        }

        /// <summary>
        /// 获取指定时间段数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet("{start}&&{end}")]
        public ObjectResult Get(string start, string end)
        {
            try
            {
                var startTime = Convert.ToDateTime(start);
                var endTime = Convert.ToDateTime(end);
                if ((endTime - startTime).Days <= 7)
                    return new ObjectResult(_environment.Select()
                        .Where(x => x.RecordTime >= startTime && x.RecordTime <= endTime));
                else
                    return new ObjectResult(_environment.Select().OrderByDescending(x => x.RecordTime).First());
            }
            catch
            {
                return new ObjectResult(_environment.Select().OrderByDescending(x => x.RecordTime).First());
            }
        }
    }
}