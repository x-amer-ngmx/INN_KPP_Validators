using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INN_KPP_Validator.Model
{
    /// <summary>
    /// Объект Контрагента
    /// </summary>
    public class SoleTraderModel
    {
        //ИНН контрагента
        public string inn { get; set; }

        //КПП контрагента
        public string kpp { get; set; }
    }
}
