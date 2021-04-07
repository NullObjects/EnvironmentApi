using System;
using System.Linq;
using EnvironmentApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DeviceStatusController : ControllerBase
    {
        private readonly EnvironmentContext _context;

        public DeviceStatusController(EnvironmentContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取最近时间数据
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        [HttpGet("{span}")]
        public ActionResult Get(string span)
        {
            var start = DateTime.Now;
            var end = DateTime.Now;
            switch (span.ToLower())
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
                    return new ObjectResult(_context.DeviceStatus.OrderByDescending(x => x.RecordTime).First());
            }

            return new ObjectResult(_context.DeviceStatus
                .Where(x => x.RecordTime >= start && x.RecordTime <= end));
        }

        /// <summary>
        /// 获取指定时间段数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet("{start}&&{end}")]
        public ActionResult Get(string start, string end)
        {
            try
            {
                var startTime = Convert.ToDateTime(start);
                var endTime = Convert.ToDateTime(end);
                if ((endTime - startTime).Days <= 7)
                    return new ObjectResult(_context.DeviceStatus
                        .Where(x => x.RecordTime >= startTime && x.RecordTime <= endTime));
                throw new Exception("时间超出最大限度,返回最新记录");
            }
            catch
            {
                return new ObjectResult(_context.DeviceStatus.OrderByDescending(x => x.RecordTime).First());
            }
        }
    }
}