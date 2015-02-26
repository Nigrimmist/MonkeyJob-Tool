using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonkeyJobTool.Entities;

namespace MonkeyJobTool.Extensions
{
    public static class StringExtensions
    {

        private static object _dictLocker = new object();


        private static Dictionary<Language, char[]> langKeyBoardDictionary = new Dictionary<Language, char[]>()
        {
            {Language.en, new []{'q','w','e','r','t','y','u','i','o','p','[',']','a','s','d','f','g','h','j','k','l',';','\'','z','x','c','v','b','n','m',',','.','/'}},
            {Language.ru, new[] {'й', 'ц', 'у', 'к', 'е', 'н', 'г', 'ш', 'щ', 'з', 'х', 'ъ', 'ф', 'ы', 'в', 'а', 'п', 'р', 'о', 'л', 'д', 'ж', 'э', 'я', 'ч', 'с', 'м', 'и', 'т', 'ь', 'б', 'ю', '.'}}
        };



        private static Dictionary<Language, Dictionary<Language, Dictionary<char,char>>> _languageDictionary;
        /// <summary>
        /// For searching index -> char
        /// </summary>
        public static Dictionary<Language, Dictionary<Language, Dictionary<char, char>>> LanguageDictionary
        {
            get
            {
                if (_languageDictionary == null)
                {
                    lock (_dictLocker)
                    {
                        if (_languageDictionary == null)
                        {
                            _languageDictionary = new Dictionary<Language, Dictionary<Language, Dictionary<char, char>>>();
                            foreach (var langSetI in langKeyBoardDictionary)
                            {
                                Dictionary<Language, Dictionary<char, char>> tDictionary = new Dictionary<Language, Dictionary<char, char>>();
                                foreach (var langSetJ in langKeyBoardDictionary)
                                {
                                    
                                    if(langSetJ.Key==langSetI.Key) continue; //skip duplicates
                                    
                                    var charIndexDict = new Dictionary<char, char>();
                                    for (int i = 0; i < langSetJ.Value.Length; i++)
                                    {
                                        charIndexDict.Add(langSetI.Value[i], langSetJ.Value[i]);
                                    }
                                    tDictionary.Add(langSetJ.Key, charIndexDict);
                                }
                                _languageDictionary.Add(langSetI.Key, tDictionary);
                            }
                        }
                    }
                }
                return _languageDictionary;
            }
            set { _languageDictionary = value; }
        }

        /// <summary>
        /// return set of words for other language sets using current keyboard layout language as default
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<string> GetOtherKeyboardLayoutWords(this string value)
        {
            var toReturn = new List<string>();

            Language currentKeyboardLang;
            currentKeyboardLang = Language.ru;

            foreach (var lang in langKeyBoardDictionary)
            {
                if (lang.Key != currentKeyboardLang)
                {
                    var currLangSetDict = LanguageDictionary[currentKeyboardLang][lang.Key]; //from lang to lang

                    StringBuilder sb = new StringBuilder(value);
                    for (int i = 0; i < value.Length; i++)
                    {
                        char foundChar;
                        if (currLangSetDict.TryGetValue(sb[i], out foundChar)) //if char pos found to replace
                        {
                            sb[i] = foundChar;
                        }
                    }
                    toReturn.Add(sb.ToString());
                }
            }

            return toReturn;
        }
    }
}


