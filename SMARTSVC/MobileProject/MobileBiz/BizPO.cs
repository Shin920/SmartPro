using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART;
using System.Data;
using System.Collections;

namespace MobileBiz
{
    public class BizPO: DBBase
    {
        public BizPO(string strClientID)
        {
            connection.ConnectDB(strClientID);
        }
        bool m_bInTrans = false;


        
    }
}
