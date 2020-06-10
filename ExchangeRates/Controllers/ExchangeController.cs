using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using System.Text;

namespace ExchangeRates.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        // объявляем список валют
       private List<ValuteClass> ValList = new List<ValuteClass>();

        // функция получения данных из cbr
        private List<ValuteClass> GetListCbr ()
        {
            string result;
            System.Xml.Linq.XElement xml;
            HttpWebRequest request;
            WebResponse response = null;
            // пытаемся получить данные из cbr
            try
            {
                request = WebRequest.Create("http://www.cbr.ru/scripts/XML_daily.asp") as HttpWebRequest;
                response = request.GetResponse();
                // установили пакет CodePakages для чтения кодировки windows-1251
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.GetEncoding("windows-1251"));
                // пробуем считать данные в xml
                     result = readStream.ReadToEnd();
                     xml = System.Xml.Linq.XElement.Parse(result);
                    foreach (System.Xml.Linq.XElement valute in xml.Elements("Valute"))
                    {
                        // добавляем экземпляр в список
                        ValList.Add(new ValuteClass(valute.Element("NumCode").Value,
                                                    valute.Element("Name").Value,
                                                    valute.Element("CharCode").Value,
                                                    valute.Element("Value").Value,
                                                    valute.Element("Nominal").Value));
                    }
            }
            catch
            {   // Вывод ошибки
                ValList.Add(new ValuteClass("Ошибка считывания или подключения"));
                return ValList;
            }
            return ValList;
        }

        // GET api/exchange Это список всех валют cbr
        [HttpGet]
        public ActionResult<IEnumerable<ValuteClass>> Get()
        {
            return GetListCbr();
        }

        // GET api/exchange/5 это выбор курса валю по NumCode
        [HttpGet("{NumCode}")]
        public ActionResult<string> Get(string NumCode)
        {
            // ищем курс валюты по NumCode
            foreach (ValuteClass valute in GetListCbr())
            {
                if (valute.NumCode == NumCode) return valute.Value;
            }
            return "Курс по данному коду не найден";
        }
    }
}
