using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP_Assigment_2
{
    class NgramEstimation
    {
        private TrainingSet trainingSet;
        private FullPermutation orderGenerator;

        public NgramEstimation()
        {
            trainingSet = new TrainingSet();
        }

        private string EstimateWithCharacteristic(string [] words)
        {
            
            int count = words.Count();
            orderGenerator = new FullPermutation(count);
            int[] order;
            double pMax = 0;
            LinkedList<int[]> orderCandi = new LinkedList<int[]>();//候选的顺序集
            //for (int i = 0; i < count; i++) orderBest[i] = i;

            //生成词性数组
            string[] character = new string[count];
            for (int i = 0; i < count; i++) character[i] = trainingSet.trie.GetCharacteristic(words[i]);
             
            while ((order = orderGenerator.GetNext()) != null)
            {
                //生成词组的全排列，并做N-gram参数估计
                double p = (double)trainingSet.GetProbability_C(words[order[0]])/(double)trainingSet.trie.Total;
                for (int i = 1; i < count; i++)
                {
                    double tmp = trainingSet.GetProbability_C(words[order[i - 1]], words[order[i]]);
                    p = p * tmp;
                    if (p == 0) break;
                }
                if (p >= pMax)
                {
                    if (p > pMax) orderCandi.Clear();
                    int[] tmp = new int[count];
                    for (int i = 0; i < count; i++) tmp[i] = order[i];
                    orderCandi.AddLast(tmp);    
                    pMax = p;
                }
            }
            if (orderCandi.Count() == 0) return null;
            //对于根据词性最优排列的句子，找出其中最好的
            pMax = 0;
            int[] orderBest = new int[count];
            foreach(var o in orderCandi)
            {
                double p = (double)trainingSet.GetProbability(words[o[0]]) / (double)trainingSet.trie.Total;
                for (int i = 1; i < count; i++)
                {
                    double tmp = trainingSet.GetProbability(words[o[i - 1]], words[o[i]]);
                    p = p + tmp;
                }
                if (p > pMax)
                {
                    for (int i = 0; i < count; i++) orderBest[i] = o[i];
                    pMax = p;
                }
            }
            string result = "";
            for (int i = 0; i < count; i++) result += words[orderBest[i]];
            return result;
        }

        public string Estimate(string[] words)
        {
            Console.WriteLine(System.DateTime.Now.ToString() + "开始计算成句概率...");
            int count = words.Count();
            orderGenerator = new FullPermutation(count);
            int[] order;
            double pMax = 0;
            int[] orderBest=new int[count];
            for (int i = 0; i < count; i++) orderBest[i] = i;
            while ((order = orderGenerator.GetNext())!=null)
            {
                //生成词组的全排列，并做N-gram参数估计
                //double p=trainingSet.GetProbability(words[0]);
                //if (p == 0) continue;
                double p =(double) trainingSet.GetProbability(words[order[0]])/(double)trainingSet.trie.Total;
                for(int i = 1;i<count;i++)
                {
                    double tmp = trainingSet.GetProbability(words[order[i - 1]], words[order[i]]);
                    p = p * tmp;
                    if (p == 0) break;
                }
                if (p > pMax)
                {
                    for (int i = 0; i < count; i++) orderBest[i] = order[i];
                    pMax = p;
                }
            }
            string result;
            if (pMax == 0)
            { 
                result = EstimateWithCharacteristic(words);
            }
            else
            {
                result = "";
                for (int i = 0; i < count; i++) result += words[orderBest[i]];
                
            }
            Console.WriteLine(System.DateTime.Now.ToString() + "计算完毕...");
            return result;
        }
    }
}
