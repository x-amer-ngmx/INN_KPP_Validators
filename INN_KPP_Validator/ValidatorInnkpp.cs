using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INN_KPP_Validator.Model;
using INN_KPP_Validator.NalogService;

namespace INN_KPP_Validator
{
    public class ValidatorInnkpp
    {
        /// <summary>
        /// Метод проверяет Индивидуальных предпринимателей
        /// </summary>
        /// <param name="dateRequest">Дата, на которую запрашиваются сведения</param>
        /// <param name="soleTraders">Перечень реквизитов Индивидуальных предпринимателей</param>
        /// <returns>Возвращает перечень запрошенных реквизитов и коды их состояний</returns>
        public ValidationResultModel ValidSoleTrader(DateTime dateRequest, List<SoleTraderModel> soleTraders)
        {
            if (soleTraders.Count > 10000) throw new Exception("Превышиние количества записей для проверки. Максимальное количество обрабатываемых записей не должно превышать 10 000 в одном запросе!");

            var date = $"{dateRequest:dd.MM.yyyy}";
            var res =
                (from trader in soleTraders select new NdsRequest2NP {INN = trader.inn, KPP = trader.kpp, DT = date})
                    .ToArray();
            return GlobalValidator(res, true);
        }

        /// <summary>
        /// Метод проверяет Юридических лиц
        /// </summary>
        /// <param name="dateRequest">Дата, на которую запрашиваются сведения</param>
        /// <param name="juridicalPersons">Перечень реквизитов Юридических лиц</param>
        /// <returns>Возвращает перечень запрошенных реквизитов и коды их состояний</returns>
        public ValidationResultModel ValidJuridicalPerson(DateTime dateRequest, List<JuridicalPersonModel> juridicalPersons)
        {
            if (juridicalPersons.Count > 10000) throw new Exception("Превышиние количества записей для проверки. Максимальное количество обрабатываемых записей не должно превышать 10 000 в одном запросе!");

            var date = $"{dateRequest:dd.MM.yyyy}";
            var res =
    (from juridical in juridicalPersons select new NdsRequest2NP { INN = juridical.inn, KPP = juridical.kpp, DT = date })
        .ToArray();

            return GlobalValidator(res);

        }

        private ValidationResultModel GlobalValidator(NdsRequest2NP[] personsList, bool isSoleTrade=false)
        {
            ValidationResultModel result = null;
            using (var context = new NalogService.FNSNDSCAWS2_PortClient())
            {
                try
                {
                    var res = context.NdsRequest2(new NdsRequest2Request(personsList));


                    result = new ValidationResultModel
                    {
                        dateActual = !isSoleTrade ? res.NdsResponse2.DTActUL : res.NdsResponse2.DTActFL,
                        resultPerson = (from ndsResponse2Np in res.NdsResponse2.NP
                            select new ValidationResultPersonModel
                            {
                                inn = ndsResponse2Np.INN,
                                kpp = ndsResponse2Np.KPP,
                                codeState = (int) ndsResponse2Np.State,
                                dateRequest = ndsResponse2Np.DT
                            }).ToList()
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }

            return result;
        }

    }
}
