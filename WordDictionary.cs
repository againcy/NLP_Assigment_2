using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NLP_Assigment_2
{
    class WordDictionary
    {
        private Dictionary<string, WordCharacteristic> trie;//词典
        public  Dictionary<string, int> characteristic;//存储词性和其出现的次数
        
        /// <summary>
        /// 所有词出现的总次数
        /// </summary>
        public int Total
        {
            get
            {
                return total;
            }
        }
        private int total;

        public WordDictionary()
        {
            trie = new Dictionary<string, WordCharacteristic>();
            characteristic = new Dictionary<string, int>();
            LoadDictionary();
        }

        public void LoadDictionary()
        {
            StreamReader sr = new StreamReader(@".\Resources\dict.txt");
            string line;
            while ((line=sr.ReadLine())!=null)
            {
                try
                {
                    string[] tmp = line.Split(new char[] { ' ' });
                    trie.Add(tmp[0], new WordCharacteristic());
                    trie[tmp[0]].character = tmp[2];
                    trie[tmp[0]].count = 0;
                }
                catch
                { }
            }
            sr.Close();
        }

        /// <summary>
        /// 添加一个词
        /// </summary>
        /// <param name="word">待添加的词</param>
        /// <returns>true: 成功添加; false: 字典中无该词</returns>
        public bool AddWord(string word)
        {
            if (trie.ContainsKey(word) == true)
            {
                trie[word].count++;
                if (characteristic.ContainsKey(trie[word].character)==true)
                {
                    characteristic[trie[word].character]++;
                }
                else
                {
                    characteristic.Add(trie[word].character, 1);
                }
                total++;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// 获取一个词出现的次数
        /// </summary>
        /// <param name="word">词</param>
        /// <returns>出现次数</returns>
        public int GetWordCount(string word)
        {
            if (trie.ContainsKey(word)) return trie[word].count;
            else return 0;
        }

        /// <summary>
        /// 获取词性
        /// </summary>
        /// <param name="word">词</param>
        /// <returns>词性</returns>
        public string GetCharacteristic(string word)
        {
            if (trie.ContainsKey(word)) return trie[word].character;
            else return null;
        }

        /// <summary>
        /// 获取一个词性的出现次数
        /// </summary>
        /// <param name="c">词性</param>
        /// <returns>词性的出现次数</returns>
        public int GetCharacteristicCount(string c)
        {
            if (characteristic.ContainsKey(c) == true) return characteristic[c];
            else return 0;
        }

        public List<string> GetAllWords()
        {
            return trie.Keys.ToList();
        }
    }
}
