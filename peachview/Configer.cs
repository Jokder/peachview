using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace peachview
{
    public static class Configer
    {
        private static string ConfigPath
        {
            get
            {
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\peach";
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                return folder + @"\peachview.conf";
            }
        }

        private static List<string> _ConfContens;

        public static string ReadByKey(string key)
        {
            FileStream fs = null;
            StreamReader sr = null;
            try
            {
                fs = new FileStream(ConfigPath, FileMode.OpenOrCreate);
                sr = new StreamReader(fs);
                _ConfContens = new List<string>();
                string rline = sr.ReadLine();
                while (!string.IsNullOrEmpty(rline))
                {
                    _ConfContens.Add(rline);
                    rline = sr.ReadLine();
                }
                var conflines = from line in _ConfContens
                    where !line.StartsWith("#") && line.Contains("=")
                    select line;
                Dictionary<string, string> confs = new Dictionary<string, string>();
                foreach (var line in conflines)
                {
                    if (confs.ContainsKey(line.Split('=')[0])) continue;
                    confs.Add(line.Split('=')[0], line.Split('=')[1]);
                }
                if (!confs.ContainsKey(key)) return string.Empty;
                return confs[key];
            }
            catch
            {
                MessageBox.Show("读取配置文件出错了.", "呵呵", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (sr != null) sr.Close();
                if (fs != null) fs.Close();
            }
            return string.Empty;
        }

        public static void SaveConf(string key, string value)
        {
            if (_ConfContens == null) return;
            int index = 0;
            while (_ConfContens.Count > index)
            {
                string line = _ConfContens[index++];
                if (!line.StartsWith("#") && line.Contains("=") && line.Split('=')[0] == key)
                {
                    line = key + "=" + value;
                    _ConfContens[index - 1] = line;
                    goto SAVECONFIG;
                }
            }
            _ConfContens.Add(key + "=" + value);
        SAVECONFIG:
            File.WriteAllLines(ConfigPath, _ConfContens);
        }
    }
}
