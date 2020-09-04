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
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ObjectResult Get()
        {
            return new ObjectResult(_environment.Select());
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
                case "month":
                    start = start.AddMonths(-1);
                    break;
                case "year":
                    start = start.AddYears(-1);
                    break;
                case "latest":
                    return new ObjectResult(_environment.Select().OrderByDescending(x => x.RecordTime).First());
                default:
                    return new ObjectResult(_environment.Select());
            }

            return new ObjectResult(_environment.Select()
                .Where(x => x.RecordTime >= Convert.ToDateTime(start) && x.RecordTime <= Convert.ToDateTime(end)));
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
                return new ObjectResult(_environment.Select()
                    .Where(x => x.RecordTime >= Convert.ToDateTime(start) && x.RecordTime <= Convert.ToDateTime(end)));
            }
            catch
            {
                return new ObjectResult(_environment.Select());
            }
        }
    }
}