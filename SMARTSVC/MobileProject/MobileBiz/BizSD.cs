//#define Test
using SMART;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MobileBiz
{
    public class BizSD : DBBase
    {
        public BizSD(string strClientID)
        {
            connection.ConnectDB(strClientID);
        }
        
       
    }
}

