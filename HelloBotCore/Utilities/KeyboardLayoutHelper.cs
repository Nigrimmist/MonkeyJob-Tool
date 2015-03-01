using System.Collections.Generic;
using System.Linq;
using HelloBotCore.Entities;

namespace HelloBotCore.Utilities
{
    public static class KeyboardLayoutHelper
    {
        
        private static readonly object _dictLocker = new object();

        private static readonly Dictionary<Language, char[]> langKeyBoardDictionary = new Dictionary<Language, char[]> {
            { Language.ru, "йцукенгшщзхъфывапролджэячсмитьбю.ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,".ToCharArray() },
            { Language.en, "qwertyuiop[]asdfghjkl;'zxcvbnm,./QWERTYUIOP{}ASDFGHJKL:\"ZXCVBNM<>?".ToCharArray() }
        };

        private static Dictionary<Language, KeyValuePair<Language, char[]>[]> Switches
        {
            get
            {
                if (_switches == null)
                {
                    lock (_dictLocker)
                    {
                        if (_switches == null)
                        {
                            _switches = new Dictionary<Language, KeyValuePair<Language, char[]>[]>();
                            foreach (var lang1 in langKeyBoardDictionary)
                            {
                                var lst = new List<KeyValuePair<Language, char[]>>();
                                foreach (var lang2 in langKeyBoardDictionary)
                                {
                                    var replacement = new char[char.MaxValue];
                                    for (var c = (char)0; c < char.MaxValue; c++)
                                        replacement[c] = c;

                                    for (var c = 0; c < lang1.Value.Length; c++)
                                        replacement[lang1.Value[c]] = lang2.Value[c];
                                    lst.Add(new KeyValuePair<Language, char[]>(lang2.Key, replacement));
                                }
                                _switches[lang1.Key] = lst.ToArray();
                            }
                        }
                    }
                }
                return _switches;
            }
            set { _switches = value; }
        }

        private static Dictionary<Language, KeyValuePair<Language, char[]>[]> _switches;

        /// <summary>
        /// will switch
        /// </summary>
        /// <param name="value"></param>
        /// <param name="wordLanguage"></param>
        /// <returns></returns>
        public static List<string> GetOtherKeyboardLayoutWords(this string value, Language wordLanguage)
        {
            var toReturn = new List<string>();
            foreach (var lang in Switches[wordLanguage])
            {
                if (lang.Key != wordLanguage)
                {
                    var retstr = new char[value.Length];
                    var len = value.Length;
                    for (var c = 0; c < len; c++)
                        retstr[c] = lang.Value[value[c]];

                    toReturn.Add(new string(retstr));
                }
            }
            return toReturn;
        }

        public static Language DetectLanguage(this string value)
        {
            Dictionary<Language, int> counter = langKeyBoardDictionary.ToDictionary(d=>d.Key,d=>0);
            int len = value.Length > 10 ? 10 : value.Length;

            foreach (KeyValuePair<Language, char[]> lang in langKeyBoardDictionary)
            {
                for (var i = 0; i < len; i++)
                {
                    if (lang.Value.Contains(value[i]))
                    {
                        counter[lang.Key]++;
                    }
                }
            }
            return counter.OrderBy(x => x.Value).Last().Key;
        }
    }
}
