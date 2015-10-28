using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP_Assigment_2
{
    /// <summary>
    /// 生成一个0到n-1的全排列
    /// </summary>
    class FullPermutation
    {
        private int[] num;
        private int length;
        public FullPermutation(int n)
        {
            length = n;
            num = null;
        }

        /// <summary>
        /// 获取下一种排列
        /// </summary>
        /// <returns></returns>
        public int[] GetNext()
        {
            if (num==null)
            {
                num = new int[length];
                for (int i = 0; i < length; i++) num[i] = i;
                return num;
            }
            //找到应该加1的项
            int prev = -1;
            int id = length - 1;
            while (id >= 0 && num[id] > prev)
            {
                prev = num[id];
                id--;
            }
            if (id < 0)
            {
                //所有全排序生成结束
                return null;
            }
            //获取已经i之前已有的数字
            List<int> exist = new List<int>();
            int j;
            for (j = 0; j < id; j++)
            {
                exist.Add(num[j]);
            }
            num[id]++;
            while (exist.Contains(num[id]) == true) num[id]++;
            exist.Add(num[id]);
            for(j=id+1;j<length;j++)
            {
                for(int tmp = 0;tmp<length;tmp++)
                {
                    if (exist.Contains(tmp)==false)
                    {
                        num[j] = tmp;
                        exist.Add(tmp);
                        break;
                    }
                }
            }
            return num;
        }
        
    }
}
