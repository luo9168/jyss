using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClasses
{
    class DBKeyViolationException : System.ApplicationException
    {
        
        public DBKeyViolationException():base("���ݿ����Ѿ�����������ͬ�ļ�¼��")
        {
                        
        }

    }
}
