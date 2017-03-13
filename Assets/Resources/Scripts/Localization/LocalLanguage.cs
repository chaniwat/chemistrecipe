using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chemistrecipe.localization
{
    public abstract class LocalLanguage
    {

        private string _locale;
        public string locale
        {
            get
            {
                return _locale;
            }
        }

        private Dictionary<string, string> words = new Dictionary<string, string>();

        public LocalLanguage(string local_code)
        {
            this._locale = local_code;
        }

        public string getString(string key)
        {
            if(!words.ContainsKey(key))
            {
                return key;
            }

            return words[key];
        }

        public void setString(string key, string value)
        {
            words[key] = value;
        }

    }
}
