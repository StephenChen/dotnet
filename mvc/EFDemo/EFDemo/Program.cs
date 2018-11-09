using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoModel entity = new DemoModel();
            T_Customer customer = new T_Customer { Address = "Wuhan Hongshan", Age = 24, UserName = "cxy" };
            entity.T_Customer.Add(customer);    // 这里只相当于SQL语句
            entity.SaveChanges();   // 这里才进行数据库操作，相当于F5执行
        }
    }
}
