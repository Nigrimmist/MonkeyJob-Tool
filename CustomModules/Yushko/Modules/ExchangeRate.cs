using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using Yushko.ExRates;

namespace Yushko.Modules
{
    public class ExchangeRate : ModuleCommandBase
    {
        private IClient _client;

        public override void Init(IClient client)
        {
            _client = client;
        }
        public override double Version
        {
            get { return 1.0; }
        }

        public override ReadOnlyCollection<CallCommandInfo> CallCommandList
        {
            get
            {
                return new ReadOnlyCollection<CallCommandInfo>(new List<CallCommandInfo>()
                {
                    new CallCommandInfo("Курсы валют", new List<string>() {"курс", "exrate"}, new ReadOnlyCollection<ArgumentSuggestionInfo>(new List<ArgumentSuggestionInfo>()
                    {
                        new ArgumentSuggestionInfo()
                        {
                            ArgTemplate = @"[0-9]+\s{{cur1}}\s{{cur2}}",
                            Details = new List<SuggestionDetails>()
                            {
                                new SuggestionDetails() {Key = "cur1", GetSuggestionFunc = (text) => _curCodes.Where(x=>string.IsNullOrEmpty(text) || x.Value.ToLower().Contains(text.ToLower()) || x.Key.ToLower().Contains(text.ToLower())).Select(x => new AutoSuggestItem() {Value = x.Key, DisplayedKey = x.Key + " - " + x.Value}).ToList()},
                                new SuggestionDetails() {Key = "cur2", GetSuggestionFunc = (text) => _curCodes.Where(x=>string.IsNullOrEmpty(text) || x.Value.ToLower().Contains(text.ToLower()) || x.Key.ToLower().Contains(text.ToLower())).Select(x => new AutoSuggestItem() {Value = x.Key, DisplayedKey = x.Key + " - " + x.Value}).ToList()},
                            }
                        },
                        new ArgumentSuggestionInfo()
                        {
                            ArgTemplate = @"{{subcommand}}",
                            Details = new List<SuggestionDetails>()
                            {
                                new SuggestionDetails() {Key = "subcommand", GetSuggestionFunc = (text) => new List<string>() {"ставкареф", "все"}.Select(x => new AutoSuggestItem() {Value = x, DisplayedKey = x}).ToList()}
                            }
                        }
                    })),
                });
            }
        }

        

        public override string Title
        {
            get { return "Курсы валют"; }
        }

        public Dictionary<string, string> _curCodes = new Dictionary<string, string>()
        {
            {"USD", "доллар США"},
            {"EUR", "евро"},
            {"RUB", "российский рубль"},
            {"BYR", "белорусский рубль"},
            {"UAH", "гривна"},
            {"AUD", "австралийский доллар"},
            {"BGN", "болгарский лев"},
            {"DKK", "датская крона"},
            {"PLN", "злотый"},
            {"ISK", "исландская крона"},
            {"CAD", "канадский доллар"},
            {"CNY", "китайский юань"},
            {"KWD", "кувейтский динар"},
            {"MDL", "молдавский лей"},
            {"NZD", "новозеландский доллар"},
            {"NOK", "норвежская крона"},
            {"SGD", "сингапурский доллар"},
            {"KGS", "сом"},
            {"KZT", "тенге"},
            {"TRY", "турецкая лира"},
            {"GBP", "фунт стерлингов"},
            {"CZK", "чешская крона"},
            {"SEK", "шведская крона"},
            {"CHF", "швейцарский франк"},
            {"JPY", "йен"},
            {"IRR", "иранских риалов"},
        };

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Курсы валют по НацБанку РБ. Умеет конвертировать разные валюты, выводить стафку рефинансирования." +
                                  "\r\nКоды : " +
                                  "\r\nUSD - доллар США" +
                                  "\r\nEUR - евро" +
                                  "\r\nRUB - российский рубль" +
                                  "\r\nBYR - белорусский рубль" +
                                  "\r\nUAH - гривна" +
                                  "\r\nAUD - австралийский доллар" +
                                  "\r\nBGN - болгарский лев" +
                                  "\r\nDKK - датская крона" +
                                  "\r\nPLN - злотый" +
                                  "\r\nISK - исландская крона" +
                                  "\r\nCAD - канадский доллар" +
                                  "\r\nCNY - китайский юань" +
                                  "\r\nKWD - кувейтский динар" +
                                  "\r\nMDL - молдавский лей" +
                                  "\r\nNZD - новозеландский доллар" +
                                  "\r\nNOK - норвежская крона" +
                                  "\r\nSGD - сингапурский доллар" +
                                  "\r\nKGS - сом" +
                                  "\r\nKZT - тенге" +
                                  "\r\nTRY - турецкая лира" +
                                  "\r\nGBP - фунт стерлингов" +
                                  "\r\nCZK - чешская крона" +
                                  "\r\nSEK - шведская крона" +
                                  "\r\nCHF - швейцарский франк" +
                                  "\r\nJPY - йен" +
                                  "\r\nIRR - иранских риалов",
                    CommandScheme = "курс <кол-во> <код_валюты_из> <код_валюты_в>",
                    SamplesOfUsing = new List<string>()
                    {
                        "курс 1 usd byr",
                        "курс ставкареф",
                        "курс коды",
                        "курс все",
                        "курс help"
                    }

                };
            }
        }

        public override string IconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAB1UlEQVRIid2VIUiDURSFv7CwsGAwGAyGCUaD4oKCgjCDgojBYBCTwbBoEAwiIoYFbSILSyqITUREZBZBsQgiC4YJIgaDiMLCb3jv8d9d735m1AsPtnvuOee9t3vf4L9FJ1ABcr/gjALHwDLQm1SYAW6BCHgGsi0aTHpOWDVgBxgH0rKwrArnWjRoUzy5dkLRFPAlgGug3xDr80vHpSF+BUyEgnMFThsiWYHr69syDI4COKCAbUN8BngTNW+4Ow4xbxjUgTzAggLalXgH8O6xKvAqPqd8TZfP3SutdYCSSFwYuw9d8o7rijSuyyIa23LMbyZ0YoRrX6oiUTQMhgW+4E/YJnavoyjqHwHuRGLDIKRwwyePnjSMBVH3AvAgEqtNSBncpMrjf+LuPsngCRp/mJJBaMcN3Zj/Poyb1sib6lhBXdGBSmS8SIgl4q4Jcepzm4bBntA71I4R8TX0eIIcsCquM8L3USWeJm7pyG+OEeBDmUTAoiAWFFbH7rg1VZcPwL5hsKvIuYSdB7yuTpsCGAJODIMbQ6QPGDTy3cCZ4s8HsMRP8bC6DDErZhWvIsGMLygTvzNhzbRokCV+Pmq4J8OMFO4KVnHdZD3bzSLnOYl/m38vvgExuNOCw1vhrQAAAABJRU5ErkJggg=="; }
        }

        public ExRatesSoapClient rates = new ExRatesSoapClient();
        public List<int> requiredCurrencies = new List<int> { 978, 840, 643 };//EUR, USD, RUB //826=GBP, 
        public CultureInfo ruCulture = new CultureInfo("ru-RU", false);

        private DateTime _lastUpdated_exRatesDaily;
        private DataSet _exRatesDaily;
        public DataSet ExRatesDaily { get { return getExRatesDaily(); } }

        private DateTime _lastUpdated_currenciesRef;
        private DataSet _currenciesRef;
        public DataSet CurrenciesRef { get { return getCurrenciesRef(); } }

        private DateTime _lastUpdated_refRateOnDate;
        private DataSet _refRateOnDate;
        public DataSet RefRateOnDate { get { return getRefRateOnDate(); } }

        private DataSet getRefRateOnDate()
        {
            if ((DateTime.Today != _lastUpdated_refRateOnDate) || (!_refRateOnDate.IsInitialized))
            {
                _refRateOnDate = rates.RefRateOnDate(DateTime.Today);
                if (_refRateOnDate.IsInitialized)
                    _lastUpdated_refRateOnDate = DateTime.Today;
            }
            return _refRateOnDate;
        }

        private DataSet getCurrenciesRef()
        {
            if ((DateTime.Today != _lastUpdated_currenciesRef) || (!_currenciesRef.IsInitialized))
            {
                _currenciesRef = rates.CurrenciesRef(0);
                if (_currenciesRef.IsInitialized)
                    _lastUpdated_currenciesRef = DateTime.Today;
            }
            return _currenciesRef;
        }

        private DataSet getExRatesDaily()
        {
            if ((DateTime.Today != _lastUpdated_exRatesDaily) || (!_exRatesDaily.IsInitialized))
            {
                _exRatesDaily = rates.ExRatesDaily(DateTime.Today);
                if (_exRatesDaily.IsInitialized)
                    _lastUpdated_exRatesDaily = DateTime.Today;
            }
            return _exRatesDaily;
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            string[] arg = args.Split(' ');
            string result = "";

            switch (arg[0])
            {
                case "помощь":
                    result = getHelp(false);
                    break;
                case "help":
                    result = getHelp(true);
                    break;
                case "коды":
                    result = getCurrencies(false);
                    break;
                case "codes":
                    result = getCurrencies(true);
                    break;
                case "все":
                case "all":
                    result = getExRates(true);
                    break;
                case "ставкареф":
                case "refrate":
                    result = getRefRate();
                    break;
                default:
                    result = getConvertedCurrencies(arg);
                    break;
            }

            _client.ShowMessage(commandToken, CommunicationMessage.FromString(result));
        }

        //сконвертировать из одной валюты в другую
        private string getConvertedCurrencies(string[] arg)
        {
            if (arg.Length < 2) return getExRates(false);
            decimal amountToConvert = 0, convertedAmount=0, srcExRate=0, dstExRate=0;
            int srcCurrScale=0, dstCurrScale=0;
            String srcExCurr = arg[1].ToUpper(), dstExCurr="BYR", unknownCurrencyCode="";
            if (arg.Length>2) dstExCurr=arg[2].ToUpper();

            if (!decimal.TryParse(arg[0], out amountToConvert)) return getExRates(false);;

            if (ExRatesDaily.IsInitialized)
            {
                if (srcExCurr == "BYR")
                {
                    srcExRate = 1;
                    srcCurrScale = 1;
                }
                if (dstExCurr == "BYR")
                {
                    dstExRate = 1;
                    dstCurrScale = 1;
                }

                string currShortName;
                foreach (DataRow row in ExRatesDaily.Tables[0].Rows)
                {
                    currShortName = row.ItemArray[4].ToString().ToUpper();//currency shortname
                    if (srcExCurr == currShortName)
                    {
                        srcExRate = decimal.Parse(row.ItemArray[2].ToString());
                        srcCurrScale = int.Parse(row.ItemArray[1].ToString());
                    }
                    if (dstExCurr == currShortName)
                    {
                        dstExRate = decimal.Parse(row.ItemArray[2].ToString());
                        dstCurrScale = int.Parse(row.ItemArray[1].ToString());
                    }
                }

                if ((srcExRate > 0) && (srcCurrScale > 0))
                {
                    convertedAmount = amountToConvert * srcExRate / srcCurrScale;
                    if ((dstExRate > 0) && (dstCurrScale > 0))
                    {
                        convertedAmount = convertedAmount / dstExRate * dstCurrScale;
                    }
                    else {
                        unknownCurrencyCode += dstExCurr + " - неизвестный код валюты. \n";
                        dstExCurr = "BYR";
                    }
                }
                else
                {
                    unknownCurrencyCode += srcExCurr + " - неизвестный код валюты. \n";
                    return unknownCurrencyCode + getExRates(false);
                }
                ruCulture.NumberFormat.NumberGroupSeparator = " ";
                return string.Format("{0}{1} {2} = {3} {4}", unknownCurrencyCode, amountToConvert.ToString("N", ruCulture.NumberFormat), srcExCurr, convertedAmount.ToString("N", ruCulture.NumberFormat), dstExCurr);
            }
            else {
                return getExRates(false);
            }
        }

        //курсы валют
        private string getExRates(bool getAll)
        {
            StringBuilder result = new StringBuilder();

            if (ExRatesDaily.IsInitialized)
            {
                DateTime ExRateDate = DateTime.Parse(ExRatesDaily.ExtendedProperties["onDate"].ToString(), CultureInfo.InvariantCulture);
                result.Append("Курс валют НБРБ на ");
                result.AppendLine(ExRateDate.Date.ToString("dd MMMM yyyy", ruCulture.DateTimeFormat));

                foreach (DataRow row in ExRatesDaily.Tables[0].Rows)
                {
                    if (!getAll && !requiredCurrencies.Contains((int.Parse(row.ItemArray[3].ToString()))))
                    {
                        continue;
                    }
                    result.Append(row.ItemArray[0]); //currency full name
                    result.Append(" (");
                    result.Append(row.ItemArray[4]); //currency shortname
                    result.Append(") = ");
                    decimal currVal = decimal.Parse(row.ItemArray[2].ToString());
                    ruCulture.NumberFormat.NumberGroupSeparator = " ";
                    result.AppendLine(currVal.ToString("N", ruCulture.NumberFormat)); //BRB value
                }
            }
            return result.ToString();
        }

        //список кодов валют
        private string getCurrencies(bool eng)
        {
            StringBuilder result = new StringBuilder();
            if (CurrenciesRef.IsInitialized & ExRatesDaily.IsInitialized)
            {
                //т.к. полный справочник валют весьма большой, 
                // а ежедневные курсы обмена не содержат все валюты, то
                // выбираем только то что необходимо
                foreach (DataRow row1 in ExRatesDaily.Tables[0].Rows)
                {
                    foreach (DataRow row in CurrenciesRef.Tables[0].Rows)
                    {
                        if (row1.ItemArray[3].ToString() == row.ItemArray[4].ToString())//currencycodes
                        {
                            result.Append(row1.ItemArray[4]); //currency abreviation
                            result.Append(" - ");
                            if (eng)
                            {
                                result.Append(row.ItemArray[7]); //currency fullname
                            }
                            else
                            {
                                result.Append(row.ItemArray[6]); //currency fullname
                            }
                            result.AppendLine();
                            break;
                        }
                    }
                }
            }
            else
            {
                result.AppendLine("Ошибка выборки данных");
            }
            return result.ToString();
        }

        private string getHelp(bool eng)
        {
            StringBuilder result = new StringBuilder();
            if (eng)
            {
                result.AppendLine("Additional arguments:");
                result.AppendLine("all - all today's exchange rates");
                result.AppendLine("codes - list of currency codes");
                result.AppendLine("refrate - refinancing rate");
                result.AppendLine("<amount> <currency code> - convert given amount from given currency to belorussian roubles");
                result.AppendLine("<amount> <src currency code> <dst currency code> - convert given amount from src currency to dst currency");
            }
            else
            {
                result.AppendLine("Дополнительные аргументы:");
                result.AppendLine("все - все курсы валют на сегодня");
                result.AppendLine("коды - справочник кодов валют");
                result.AppendLine("ставкареф - ставка рефинансирования");
                result.AppendLine("<сумма> <код валюты> - сконвертировать указанную сумму из указанной валюты в белорусские рубли");
                result.AppendLine("<сумма> <исходный код валюты> <конечный код валюты> - сконвертировать указанную сумму из исходной валюты в конечную валюту");
            }
            return result.ToString();
        }

        //ставка рефинансирования
        private string getRefRate()
        {
            string result;
            if (RefRateOnDate.IsInitialized)
            {
                DateTime RefRateDate = DateTime.Parse(RefRateOnDate.Tables[0].Rows[0].ItemArray[0].ToString(), CultureInfo.InvariantCulture);
                string refRateValue = RefRateOnDate.Tables[0].Rows[0].ItemArray[1].ToString();
                result = string.Format("Ставка рефинансирования составляет {0}%. Установлена {1}", refRateValue, RefRateDate.Date.ToString("dd MMMM yyyy", ruCulture.DateTimeFormat));
            }
            else
            {
                result = "Ошибка выборки данных";
            }
            return result;
        }
    }
}
