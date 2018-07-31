using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfClass
{
    /// <summary>
    /// 计算类
    /// </summary>
    class Calculate
    {
        // 几个辅助的位操作:
        // 1. -n = ~(n-1) = ~n+1
        // 2. n&~(n-1), 获取n中二进制中的最后一个1
        // 3. n&(n-1), 去掉n中二进制中的最后一个1

        // 原码 ：二进制定点表示法，即最高位为符号位，“0”表示正，“1”表示负，其余位表示数值的大小。
        // 反码 ：正数的反码与其原码相同；负数的反码是对其原码逐位取反，但符号位除外。
        // 补码 ：正数的补码与其原码相同；负数的补码是在其反码的末位加1。

        /// <summary>
        /// 加(网上迭代)
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public int add(int num1, int num2)
        {
            if (num1 == 0) return num2;
            if (num2 == 0) return num1;
            // sum 代表结果集, carry 代表的是进位
            int sum = 0, carry = 0;
            while (num2 != 0)
            {
                //这里的num1^num2得到的正是两者相加，不带进位的结果
                //首轮结果 010010^100111 = 110101
                sum = num1 ^ num2;
                //carry代表的进位 (010010&100111) <<1= (000010)<<1 = (000100),在这里刚好进位了
                carry = (num1 & num2) << 1;
                //这里替换掉num1,num2是为了方便迭代，当carry为0,即无进位结束循环
                num1 = sum;
                num2 = carry;
            }
            return sum;
        }

        /// <summary>
        /// 加(自写迭代)
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="chen"></param>
        /// <returns></returns>
        public int add1(int num1, int num2)
        {
            if (num1 == 0) return num2;
            if (num2 == 0) return num1;

            int sum = num1 ^ num2;
            int carry = (num1 & num2) << 1;

            if (carry != 0)
                sum = add1(sum, carry);

            return sum;
        }

        /// <summary>
        /// 减
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public int sub(int num1, int num2)
        {
            return add(num1, add(~num2, 1));
        }

        /// <summary>
        /// 乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int multiply(int a, int b)
        {
            // 当乘一个数的时候，左移它所出现的位置步，
            // 比如：1011*0010那么就会左移动1位，1011*1000左移3位，
            // 对一个数进行拆分了，保留每一位上面的1，其余的都变成0。
            // 相当于分配率 rtn = a * b = a * (2^n1 + 2^n2 + 2^n3 + ...)

            // 利用 Dictionary 存放需要移动的位数，以及1向左移动i位的值
            // 用于快速缓存移动的位数，将移位的值->移位位数进行mapping
            Dictionary<int, int> dic = new Dictionary<int, int>();
            for (int i = 0; i < 32; i++)
            {
                dic.Add(1 << i, i);
            }
            int sum = 0;
            while (b > 0)
            {
                int last = b & (~(b - 1));//取得最后一个1;
                int count = dic[last];//取得移动的位数
                sum = add(sum, a << count);
                b = b & (b - 1); //去掉最后一个1;
            }
            return sum;
        }

        /// <summary>
        /// 除
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int div(int a, int b)
        {
            // 二进制的除法可以利用移位跟加减法进行计算。
            // 思路如下：先对除数进行移位操作，移位到一个临界值，即比被除数小的最大值，
            // 再用除数进行迭代相减，有点类似辗转相除法。
            // 举例：175 / 5 
            // ---- 1010 1111(a) / 0000 0101(b)                                            32 + 2 + 1 
            // ---- bleft = 6                                                           +——————————————
            // ----                                                                   5 | 175
            // ---- i = 6                                                                 160      
            // ---- 1 0100 0000(b<<6) > 1010 1111(a)                                    ————————
            // ----                                                                        15
            // ---- i = 5                                                                  10
            // ---- 1010 0000(b<<5) < 1010 1111(a)                                      ————————
            // ---- res = 0000 0000 | 0010 0000(1<<5) = 0010 0000                           5
            // ---- a = a - b<<5 = 1010 1111 - 1010 0000 = 0000 1111 = 175 - 160 = 15       5
            // ----                                                                     ————————
            // ---- i = 4                                                                   0
            // ---- 0101 0000(b<<4) > 0000 1111(a)
            // ---- 
            // ---- i = 3
            // ---- 0010 1000(b<<3) > 0000 1111(a)
            // ---- 
            // ---- i = 2
            // ---- 0001 0100(b<<2) > 0000 1111(a)
            // ---- 
            // ---- i = 1
            // ---- 0000 1010(b<<1) < 0000 1111(a)
            // ---- res = 0010 0000 | 0000 0010(1<<1) = 0010 0010
            // ---- a = a - b<<1 = 0000 1111 - 0000 1010 = 0000 0101 = 15 - 6 = 9
            // ---- 
            // ---- i = 0
            // ---- 0000 0101(b<<0) = 0000 0101(a)
            // ---- res = 0010 0010 | 0000 0001(1<<0) = 0010 0011 = 35
            // ---- a = a - b<<0 = 0000 0101 - 0000 0101 = 0
            // ----  

            if (a < b)
            {
                return 0;
            }
            int bleft = 0;  // b左移的位数
            // 迭代获取除数达到临界值需要移动的位数
            while ((b << bleft) < a)
            {
                bleft++;
            }
            int res = 0;    // 商
            for (int i = bleft; i >= 0; i--)
            {
                // 每次进行检验，如果当除数移位后，大于被除数，跳过这次循环
                if ((b << i) > a) continue;
                // 这里记录移位的结果集，当i满足的时候即为1不满足即为0
                res |= (1 << i);
                // 缩小被除数的范围。
                a = sub(a, b << i);
            }
            return res;
        }

        /// <summary>
        /// 幂
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int pow(int x, int y)
        {
            // 在这里不使用的循环的原因是，那样太需要计算很多次，
            // 这里的temp用于保存上次的记录的结果，直接拿出来用，避免的了重复的计算，
            // 我们一次分为奇偶来讨论，如 x^4 可以拆分为x^2*x^2 => x^(y/2)，
            // 而奇数的话，我们可以用上次保存的偶数乘以x得到。
            if (y == 1) return x;
            int result = 0;
            int temp = pow(x, y / 2);
            // y&1 可以验证奇偶  当y的二进制末尾不存在1即为偶数
            if ((y & 1) != 0)
            {
                result = x * temp * temp;// 奇数
            }
            else
            {
                result = temp * temp;// 偶数
            }
            return result;
        }
    }
}
