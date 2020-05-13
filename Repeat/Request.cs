using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat
{
    /// <summary>
    /// 请求
    /// </summary>
    public class Request
    {
        /// <summary>
        /// 需要生成的项
        /// </summary>
        public List<string> Items { get; set; }

        /// <summary>
        /// 模版
        /// </summary>
        public string Template { get; set; }
    }
}
