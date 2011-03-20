using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClasses
{
    class DBKeyViolationException : System.ApplicationException
    {
        
        public DBKeyViolationException():base("数据库中已经存在主键相同的记录！")
        {
                        
        }

    }
}
