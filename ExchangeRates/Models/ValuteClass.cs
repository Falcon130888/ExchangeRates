
namespace ExchangeRates.Controllers
{   // класс валют
    public class ValuteClass
    {
        public string NumCode { get; }
        public string CharCode { get; }
        public string NameValute { get; }
        public string Value { get; }
        public string Nominal { get; }

        public ValuteClass(string numcode)
            => NumCode = numcode;

        public ValuteClass(string numcode, string charcode)
            : this(numcode)
            => CharCode = charcode;

        public ValuteClass(string numcode, string namevalute, string charcode)
            : this(numcode, charcode)
            => NameValute = namevalute;

        public ValuteClass(string numcode, string namevalute, string charcode, string value)
            : this(numcode, namevalute, charcode)
            => Value = value;

        public ValuteClass(string numcode, string namevalute, string charcode, string value, string nominal)
            : this(numcode, namevalute, charcode, value)
            => Nominal = nominal;
    }



}
