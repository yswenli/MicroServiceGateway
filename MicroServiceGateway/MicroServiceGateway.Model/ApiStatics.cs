using System.Collections;
using System.Collections.Generic;

namespace MicroServiceGateway.Model
{
    /// <summary>
    /// 调用次数统计
    /// </summary>
    public class ApiStatics : IEnumerable<Apistatistical>
    {
        List<Apistatistical> _list;

        /// <summary>
        /// 调用次数统计
        /// </summary>
        public ApiStatics()
        {
            _list = new List<Apistatistical>();
        }

        public IEnumerator<Apistatistical> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// add
        /// </summary>
        /// <param name="url"></param>
        /// <param name="count"></param>
        public void Add(string url,long count)
        {
            _list.Add(new Apistatistical() {Url=url,Count= count });
        }
    }

    /// <summary>
    /// 调用次数统计
    /// </summary>
    public class Apistatistical
    {
        public string Url { get; set; }

        public long Count { get; set; }
    }
}
