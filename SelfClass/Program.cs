using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfClass
{
    class Program
    {
        static void Main(string[] args)
        {
            int aa = 0x12, bb = 0x27;
            int a = Convert.ToInt32("010010", 2), b = Convert.ToInt32("100111", 2);
            Calculate cal = new Calculate();
            int i1 = cal.add(1, 2);
            int i2 = cal.add1(1, 2);
            int i3 = cal.add(2, 2);
            int i4 = cal.add1(2, 2);

            int i5 = cal.sub(2, 0);
            int i6 = cal.sub(2, 1);
            int i7 = cal.sub(2, 2);
            int i8 = cal.sub(2, 3);

            int i11 = cal.multiply(2, 4);
            int i12 = cal.multiply(3, 4);
            int i13 = cal.multiply(3, 5);
            int i14 = cal.multiply(-3, -5);
            int i15 = cal.multiply(-3, 5);

            int i16 = cal.div(4, 2);
            int i17 = cal.div(7, 2);
            int i18 = cal.div(-8, 2);
            int i19 = cal.div(-7, 3);
        }
    }
}
