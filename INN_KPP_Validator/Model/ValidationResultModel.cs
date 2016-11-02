using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INN_KPP_Validator.Model
{

    //Возвращаемый результат
    public class ValidationResultModel
    {
        // Дата, на которую актуальны данные, используемые для проверки
        public string dateActual { get; set; }

        public List<ValidationResultPersonModel> resultPerson { get; set; }
    }

    public class ValidationResultPersonModel : SoleTraderModel
    {
        //Состояние контрагента
        public int codeState { get; set; }

        //Дата, на которую запрашиваются сведения
        public string dateRequest { get; set; }

    }
}
