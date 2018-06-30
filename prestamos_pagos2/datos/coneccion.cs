using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace prestamos_pagos2.datos
{
    class coneccion
    {
        public SqlConnection conn = new SqlConnection(@"Data Source=tcp:prestamospagos.database.windows.net,1433;Initial Catalog=prestamos_pagos; Persist Security Info=True;User ID=ochirinos; Password=Aa123456");
        //public SqlConnection conn = new SqlConnection(@"Data Source=127.0.0.1;Initial Catalog=prestamos_pagos; Persist Security Info=True;User ID=sa; Password=H283CE418902C;Max Pool Size=1024;Pooling=true;");

    }
}
