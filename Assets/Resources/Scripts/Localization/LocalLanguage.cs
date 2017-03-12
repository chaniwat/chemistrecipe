using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chemistrecipe.localization
{
    public abstract class LocalLanguage
    {

        private string _local_code;
        public string local_code
        {
            get
            {
                return _local_code;
            }
        }

        private Dictionary<string, string> words = new Dictionary<string, string>();

        public LocalLanguage(string local_code)
        {
            this._local_code = local_code;
        }

        public string getString(string key)
        {
            return words[key];
        }

        public void setString(string key, string value)
        {
            words[key] = value;
        }

    }
}
