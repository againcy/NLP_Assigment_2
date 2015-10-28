using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using JiebaNet.Segmenter;

namespace NLP_Assigment_2
{
    class TrainingSet
    {
        private Regex regexChinese = new Regex(@"([\u4E00-\u9FA5a-zA-Z0-9+#&\._]+)", RegexOptions.Compiled);
        private Dictionary<string, Dictionary<string, int>> countWordGroup;//[word1][word2]==p表示word2跟着word1出现的次数为p
        private Dictionary<string, Dictionary<string, int>> countCharacterGroup;//[c1][c2]==p表示词性c1后出现词性c2的词的次数为p
        //private Dictionary<string, int> countHead;//一个词出现在句首的概率
        private JiebaSegmenter segmenter;
        public WordDictionary trie;

        public TrainingSet()
        {
            countWordGroup = new Dictionary<string, Dictionary<string, int>>();
            countCharacterGroup = new Dictionary<string, Dictionary<string, int>>();
            //countHead = new Dictionary<string, int>();
            segmenter = new JiebaSegmenter();
            trie = new WordDictionary();

            LoadTrainingSet();
            OutputDictionary();
        }

        /// <summary>
        /// 读入训练集，并统计所有词组word1,word2出现的次数
        /// </summary>
        private void LoadTrainingSet()
        {
            StreamReader sr = new StreamReader("Training.txt",Encoding.UTF8);
            string line;
            while ((line = sr.ReadLine())!=null)
            {
                foreach(var block in regexChinese.Split(line))
                {
                    if (regexChinese.IsMatch(block)==false) continue;
                    var words = segmenter.Cut(block);
                    if (words == null || words.Count() == 0) continue;
                    string prev = null;
                    foreach (string word in words)
                    {
                        if (trie.AddWord(word) == false) break;
                        if (prev != null)
                        {
                            #region 根据词本身计数
                            if (countWordGroup.ContainsKey(prev) == true)
                            {
                                //已经出现过word1
                                if (countWordGroup[prev].ContainsKey(word) == true)
                                {
                                    //词组word1,word2计数++
                                    countWordGroup[prev][word]++;
                                }
                                else
                                {
                                    //添加词组word1,word2
                                    countWordGroup[prev].Add(word, 1);
                                }
                            }
                            else
                            {
                                //直接添加新的词组word1,word2
                                countWordGroup.Add(prev, new Dictionary<string, int>());
                                countWordGroup[prev].Add(word, 1);
                            }

                            #endregion

                            #region 根据词性计数
                            string c_prev = trie.GetCharacteristic(prev);
                            string c_word = trie.GetCharacteristic(word);
                            if (c_prev != null && c_word != null)
                            {
                                if (countCharacterGroup.ContainsKey(c_prev) == true)
                                {
                                    //已经出现过c_prev
                                    if (countCharacterGroup[c_prev].ContainsKey(c_word) == true)
                                    {
                                        //c_prev,c_word计数++
                                        countCharacterGroup[c_prev][c_word]++;
                                    }
                                    else
                                    {
                                        //添加词组word1,word2
                                        countCharacterGroup[c_prev].Add(c_word, 1);
                                    }
                                }
                                else
                                {
                                    //直接添加新的词组word1,word2
                                    countCharacterGroup.Add(c_prev, new Dictionary<string, int>());
                                    countCharacterGroup[c_prev].Add(c_word, 1);
                                }
                            }
                        }
                        #endregion
                        prev = word;
                    }
                }
            }
            sr.Close();
        }

        public void OutputDictionary()
        {
            StreamWriter sw = new StreamWriter("Words.txt");
            foreach (var word in trie.GetAllWords())
            {
                sw.WriteLine(word);
            }
            sw.Close();

        }

        /// <summary>
        /// 获取词组word1,word2的概率
        /// </summary>
        /// <param name="word1"></param>
        /// <param name="word2"></param>
        /// <returns>Count(word1,word2)/Count(word1)或0</returns>
        public double GetProbability(string word1,string word2)
        {
            if (countWordGroup.ContainsKey(word1) == false) return 0;
            else
            {
                if (countWordGroup[word1].ContainsKey(word2) == false) return 0;
                else
                {
                    int cnt1 = countWordGroup[word1][word2];
                    int cnt2 = trie.GetWordCount(word1);
                    return (double)cnt1 / (double)cnt2;
                }
            }
        }

        /// <summary>
        /// 获取词word出现的概率
        /// </summary>
        /// <param name="word"></param>
        /// <returns>word出现的概率或0</returns>
        public double GetProbability(string word)
        {
            return (double)trie.GetWordCount(word) / trie.Total;
        }

        /// <summary>
        /// 获取两个词词性组合出现的概率
        /// </summary>
        /// <param name="word1"></param>
        /// <param name="word2"></param>
        /// <returns>Count(word1,word2)/Count(word1)或0</returns>
        public double GetProbability_C(string word1,string word2)
        {
            string c1 = trie.GetCharacteristic(word1);
            string c2 = trie.GetCharacteristic(word2);
            if (c1 == null || c2 == null) return 0;
            
            int cnt1 = countCharacterGroup[c1][c2];
            int cnt2 = trie.GetCharacteristicCount(c1);
            return (double)cnt1 / (double)cnt2;
                
            
        }
    }
}
