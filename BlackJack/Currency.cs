using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    enum Currency
    {
        Dollar, Euro, Krona
    }

    static class CurrencyUtil
    {
        public static String GetCode(Currency currency)
        {
            switch(currency) {
                case Currency.Dollar:
                    return "USD";
                case Currency.Euro:
                    return "EUR";
                case Currency.Krona:
                    return "SEK";
                default:
                    return "Non-supported currency";
            }
        }
    }
}
