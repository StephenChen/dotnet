## C# 语法新特性
### C# 2.0 新特性
- 泛型(Generics)
	- 2.0 增加了泛型。引入类型参数的概念。
	`public class List<T>{ }`
	- 泛型允许将一个实际的数据类型规约延迟至泛型的实例被创建时才确定。
	- 有点：
		- 编译时可以保证类型安全。
		- 不用做类型装换，获得一定的性能提升。
- 泛型方法、泛型委托、泛型接口
	```
	// 泛型委托
	public delegate void Del<T>(T item);
	public static void Notify(int i) { }
	Del<int> m1 = new Del<int>(Notify);
	// 泛型接口
	public class MyClass<T1, T2, T3> : MyInterface<T1, T2, T3> { 
		public T1 Method1(T2 param1, T3 param2) { 
			throw new NotImplementedException(); 
		} 
	}
	interface MyInterface<T1, T2, T3> { T1 Method1(T2 param1, T3 param2); }
	// 泛型方法
	static void Swap<T>(ref T t1, ref T t2) { T temp = t1; t1 = t2; t2 = temp; }
	public void Interactive() {
		string str1 = "a; string str2 = "b";
		Swap<string>(ref str1, ref str2);
		Console.WriteLine(str1 + "," + str2);
	}
	```

- 泛型约束(constraints)
	- 可以给泛型的类型参数加上约束。
	```
	WhereT:struct				类型参数需是值类型
	WhereT:class				类型参数需是值类型
	WhereT:new()				类型参数需是值类型
	WhereT:<base class name>	类型参数需是值类型
	WhereT:<interface name>		类型参数需是值类型
	WhereT:U					类型参数需是值类型
	```

- 部分类(partial)
	- 申明类、结构或接口时使用 partial 关键字可让源码分布在不同的文件中。仅是编译器提供的功能，编译时会把 partial 关键字定义的类合在一起编译。

- 匿名方法
	- 本质就是委托，函数式编程的最大特点就是把方法作为参数和返回值。ConsoleWrite->MulticastDelegate(intPtr[])->Delegate(object,intPtr)匿名方法：编译后会生成委托对象，生成方法，然后把方法装入委托对象，最后赋值给声明的委托变量。匿名方法可以省略参数：编译的时候会自动为这个方法按照委托签名的参数添加参数。
	```
	public delegate void ConsoleWrite(string strMsg);
	// 匿名方法测试
	ConsoleWrite delCW1 = new ConsoleWrite(WriteMsg);
	delCW1("天下第一");
	ConsoleWrite delCW2 = delegate(string strMsg) {
		Console.WriteLine(strMsg);
	};
	delCW2("天下第二");
	```

### C# 3.0/C# 3.5 新特性
- 自动属性
```
// new:
public class User
{
	public int Id { get; set; } // 自动属性
	public string Name { get; set; }
}
// old:
private int id;
public int Id
{
	get { return id; }
	set { id = value; }
}
```

- 隐式推断类型 Var
	- var 主要用于表示一个 LINQ 查询的结果。
```
var customer = new User();
var i = 1;
```
	- var 隐式类型的限制：
		- 被声明的变量必须是一个局部变量，而不是静态或实例字段。
		- 变量必须在声明的同时被初始化，因为编译器要根据初始化值推断类型。
		- 初始化的对象不能是一个匿名函数。
		- 初始化表达式不能是 null。
		- 语句中只声明一次变量，声明后不能更改类型。
		- 赋值的数据类型必须是可以在编译时确定的类型。
	
- 对象集合初始器
	- 对象初始化：
	`User iser = new User { Id = 1, Name = "cxy" };`
	- 集合初始化：
	`List<Dog> dogs = new List<Dog>() { new Dog() { Name = "Tom", Age = 1 }, new Dog() { Name = "Lucy", Age = 3 } };`
	- 创建并初始化数组：
	`string[] array = { "西施", "貂蝉" };`

- 匿名类
	- 类型名由编译器生成，并且不能在源代码级使用，每个属性的类型由编译器推断。使用 `new { object initializer }` 或 `new[] { object, ...}`来初始化一个匿名类或不确定类型的数组。
	`var p = new { Id = 1, Name = "cxy", Age = "25" }; // 属性名字和顺序不同会生成不同类`
	- 在编译后会生成一个“泛类型”，使用反汇编工具 Reflector 来查看。包含如下信息：
		- 获取所有初始值的构造函数，顺序与属性顺序一样。
		- 属性的私有只读字段。
		- 重写 Object 类中的 Equals、GetHashCode、ToString() 方法。
		- 包含公共只读类型，属性不能为 null、匿名函数、指针类型。
	用处：
		- 避免过度的数据积累。
		- 为一种情况特别进行的数据封装。
		- 避免进行单调重复的编码。
	应用场合：
		- 直接使用 select new { object initializer } 这样的语法就是将一个 LINQ 查询的结果返回到一个匿名类中。
	注意：
		- 当出现“相同”的匿名类时，编译器只会创建一个匿名类。
		- 编译器如何区分匿名类是否相同。
		- 属性名、属性值(因为这些属性是根据值来确定类型的)、属性个数、属性的顺序。
		- 匿名类的属性是只读的，可放心传递，并且可用在线程间共享数据。

- 扩展方法(this string strSelf 扩展 string ,this int self 扩展 int)
	- 一种特殊的静态方法，可像扩展类型上的实例方法一样进行调用，能向现有的类型“添加”方法，无需创建新的派生类型、重新编译或以其他方式修改原始类型。
	```
	// 例：在编译时直接将 str.WriteSelf(2018) 替换成：StringUtil.WriteSelf(str, 2018);
	// 想为一个类型添加一些成员时，可以使用扩展方法：
	public static class StringUtil {
		public static void WriteSelf(this string strSelf, int year) {
			Console.WriteLine(string.Format("..{0}..{1}..", strSelf, year));
		}
	}
	// 测试：
	// 扩展方法
	string str = "拉拉";
	str.WriteSelf(2018);
	```
	- 编译器认为一个表达式要使用一个实例方法，没找到，需要检查导入的命名空间和当前命名空间里所有的扩展方法，并匹配到合适的方法。
	- 注意：
		- 实例方法优先于扩展方法(允许存在同名实例方法和扩展方法)。
		- 可以在空引用上调用扩展方法。
		- 可以被继承。
		- 并不是任何方法都能用作扩展方法使用，必须有一下特征：
			- 它必须放在一个非嵌套、非泛型的静态类中。
			- 它至少有一个参数。
			- 第一个参数必须附加 this 关键字。
			- 第一个参数不能有任何其他修饰符(out/ref)。
			- 第一个参数不能是指针类型，其类型决定是在何种类型上进行扩展。

- 系统内置委托
	- Func/Action 委托使用可变性：
	```
	Action<object> test = delegate(object o) { Console.WriteLine(o); };
	Action<string> test2 = test;
	Func<String> fest = delegate() { return Console.ReadLine(); };
	fest2 = fest;

	public delegate void Action();
	public delegate bool Predicate<in T>(T obj);
	public delegate int Comparison<in T>(T x, T y);
	```
	- 协变指的是委托方法的返回值类型直接或间接继承自委托签名的返回值类型，逆变则是参数类型继承自委托方法的参数类型 System.Func，代表有返回类型的委托。
	```
	public delegate TResult Func<out TResult>();
	public delegate TResult Func<in T, out TResult>(T arg);
	......
	```
	- 注：输入泛型参数 -in 最多可以有16个，输出泛型参数 -out 只有一个。System.Action 代表无返回类型的委托。
	```
	public delegate void Action<in T>(T obj);	// list.Foreach
	public delegate void Action<in T1, in T2>(T1 arg1, T2 arg2);
	......
	```
	- 注：最多有16个参数，System.Predicate<T> 代表返回 bool 类型的委托，用作执行表达式。
	`public delegate bool Predicate<in T>(T obj);	// list.Find`
	- System.Comparison<T> 代表返回 int 类型的委托，用于比较两个参数的大小。
	`public delegate int Compatison<in T>(T x, T y);	// list.Sort`

- Lambda 表达式
	- 本质就是匿名函数，Lambda 表达式基于数学中λ演算，直接对应于其中的 lambda 抽象(lambda abstraction)，是一个匿名函数。可以包含表达式和语句，并且可用于创建委托和表达式树类型。
	- Lambda 表达式的运算符为=>。=>运算符具有与赋值运算符(=)相同的优先级。
	- Lambda 的基本形式是：`(input parameters) => expression`
	- 只有在 Lambda 有一个输入参数时，括号才是可选的：
		- `(x, y) => x == y`
		- `(int x, string s) => s.Length > x`
		- `() => SomeMethod()`
	- 最常见的场景是 IEnumerable 和 IQueryable 接口的 Where<>(c=>c.ID>3)。
	- Lambda 表达式中的变量范围：
		- 捕获的变量将不会被作为垃圾回收，直至引用变量的委托超出范围为止。
		- 在外部方法中看不到 Lambda 表达式内引入的变量。
		- Lambda 表达式无法从封闭方法中直接捕获 ref 或 out 参数。
		- Lambda 表达式中的返回语句不会导致封闭方法返回。
		- Lambda 表达式不能包含其目标位于所包含匿名函数主体外部或内部的 goto 语句、break 语句或 continue 语句。
	- Lambda 表达式缩写推演
		- new Func<string, int>(delegate(string str) { return str.Length; });
		- delegate(string str) { return str.Length; }	匿名函数
		- (string str) => { return str.Length; }	Lambda 语句
		- (string str) => str.Length	Lambda 表达式
		- (str) => str.Length	让编译器推断参数类型
		- str => str.Length	去掉不必要的括号
	- 例如：
	```
	delegate int AddDel(int a, int b);	// 定义一个委托
	#region lambda
	
	AddDel fun = delegate(int a, int b) { return a + b; };	// 匿名函数
	// Console.WriteLine(fun(1, 3));
	// lambda 参数类型可以进行隐式判断，可以省略类型 lambda 本质就是匿名函数
	AddDel funLambda = (a, b) => a + b;
	List<string> strs = new List<string>() { "1", "2", "3" };
	var temp = strs.FindAll(s => int.Parse(s) > 1);
	foreach (var item in temp) {
		Console.WriteLine(item);
	}
	// Console.WriteLine(funLambda(1, 3));
	
	#endregion
	
	static void Main(string[] args) {
		List<int> nums = new List<int>() { 1, 2, 3, 4, 6, 9, 12 };
		// 使用委托的方式
		List<int> evenNums = nums.FindAll(GetEvenNum);
		foreach (var item in evenNums) {
			Console.WriteLine(item);
		}
		
		Console.WriteLine("使用 Lambda 的方式");
		List<int> evenNumLambdas = nums.FindAll(n => n % 2 == 0);
		foreach (var item in evenNumLambdas) {
			Console.WriteLine(item);
		}
		Console.ReadKey();
	}

	static bool GetEvenNum(int num) {
		if (num % 2 == 0) { 
			return true;
		}
		return false;
	}
	```

- 标准查询运算符(SQO)
	- 定义在 System.Linq.Enumerable 类中的 50 多个为 IEnumerable<T>准备的扩展方法。
	- 标准查询运算符提供了筛选、投影、聚合、排序等功能。
	```
	private List<User> InitLstData() {
		new User { Id = 1, Name = "cxy1", Age = 21 },
		new User { Id = 2, Name = "cxy2", Age = 22 },
		new User { Id = 3, Name = "cxy3", Age = 23 },
		new User { Id = 4, Name = "cxy4", Age = 24 }
	};
	- 筛选集合(Where)：提供对于一个集合的筛选功能，需要提供一个带 bool 返回值的“筛选器”(匿名方法、委托、Lambda 表达式均可)，表明集合中某个元素是否应该被返回。
	```
	var lst = InitLstData();
	var result = lst.Where(x => x.Age >= 30).ToList();
	result.ForEach(r => Console.WriteLine(string.Format("{0},{1},{2}", r.Id, r.Name, r.Age)));
	Console.ReadKey();
	```
	- 查询投射 Select：返回新对象集合 IEnumerable<TSource>Select()。
	```
	var result = lst.Where(x => x.Age >= 30).Select(s => s.Name).ToList();
	result.ForEach(x => Console.WriteLine(x));
	- 统计数量 int Count()。
	`lst.Where(x => x.Age >= 30).Count();`
	- 多条件排序 OrderBy().ThenBy().ThenBy()
	`lst.OrderBy(x => x.Age).OrderBy(x => x.Id)`
	- 集合连接 Join():新加一个Student类，并初始化数据。
	```
	public class Student {
		public int ID { get; set; }
		public int UserId { get; set; }
		public string ClassName { get; set; }
	}
	List<Student> lstStu = new List<Student>() {
		new Student { ID = 1, UserId = 1, ClassName = "8ban" },
		new Student { ID = 1, UserId = 3, ClassName = "2ban" },
		new Student { ID = 1, UserId = 2, ClassName = "1ban" }};
	var result = lst.Join(lstStu, u => u.Id, p => p.UserId, (u, p) => new { UserId = u.Id, Name = u.Name, ClassName = p.ClassName });
	- 即时加载 FindAll
	`List<User> lstUsr = lst.FindAll(x => x.Age >= 30);`
	- SQO缺点：语句太庞大复杂。

- LINQ
	- 查询表达式
	```
	IEnumerable<Dog> listDogs = from dog in dogs
	where dog.Age > 5
	// let d = new { Name = dog.Name }
	orderby dog.Age descending
	select dog;
	// select new { Name = dog.Name }
	```
	- 以 from 开始，以 select 或 group by 子句结尾。输出是一个 IEnumerable<T> 或 IQueryable<T> 集合。
	- 注：T的类型由 select 或 group by 推断。
	- LINQ 分组：
	`IEnumerable<int, Dog>> listGroup = from dog in listDogs where dog.Age > 5 group dog by dog.Age;`
	- 遍历分组：
	```
	foreach (IGrouping<int, Dog> group in listGroup) {
		Console.WriteLine(group.Key + "岁：");
		foreach (Dog d in group) {
			Console.WriteLine(d.Name + ",age=" + d.Age);
		}
	}
	```
	- 注意：LINQ 查询语句编译后会转成标准查询运算符。
	- LINQPad工具。

### C# 4.0 新特性
- 可选参数和命名参数
	- 可选参数：当调用方法，不给参数赋值时，使用定义的默认值。
		- 可选参数不能为参数列表的第1个参数，必须位于所有的必选参数之后(除非没有必选参数);
		- 可选参数必须指定默认值，而且默认值必须是一个常量表达式，不能为变量;
		- 所有可选参数以后的参数都必须是可选参数。
	- 命名参数：通过命名参数调用，实参顺序可以和形参不同。
	- 对于简单的重载，可以使用可选参数和命名参数混合的形式来定义方法，提高代码的运行效率。
	```
	public class Dog {
		public string Name { get; set; }
		public int Age { get; set; }
		/// <summary>
		/// 参数默认值 和 命名参数
		/// </summary>
		/// <param name="name"></param>
		/// <param name="age"></param>
		public void Say(string name = "wangwangwang", int age = 1) {
			Console.WriteLine(name + "," + age);
		}
	}
	```
	- 让 name 使用默认值，age 怎么给值：`_dog.Say(age:3);`。

- Dynamic 特性
	- 需引用 System.Dynamic 命名空间：
	```
	using System.Dynamic;
	// Dynamic
	dynamic Customer = new ExpandoObject();
	Customer.Name = "cxy";
	Customer.Male = true;
	Customer.Age = 24;
	Console.WriteLine(Customer.Name + Customer.Age + Customer.Male);
	Console.ReadKey();
	```

- params(并非新的语法特性)：使用 params 关键字作为方法参数可以指定采用数目可变的参数，可以发送参数声明中所指定类型用逗号分隔的参数列表或指定类型的参数数组，还可以不发送参数。
	- 注：在方法声明中的 params 关键字之后不允许有任何其他参数，并且在方法生命中只允许一个 params 关键字。
	```
	public void ParamsMethod(params int[] list) {
		for (int i = 0; i < list.Length; i++) {
			Console.WriteLine(list[i]);
		}
		Console.ReadLine();
	}
	
	ParamsMethod(25, 24, 21, 15);
	ParamsMethod(25, 24, 21, 15);
	```

### C# 5.0 新特性
- 最重要的就是异步个等待(async 和 await)，在方法的返回值前面添加关键字 async，同时在方法体中需要异步调用的方法面前再添加关键字 await。这个异步方法必须以 Task 或者 Task<TResult> 作为返回值。


## Entity Framework
- ADO.NET Entity Framework

### EF 简
- O/R Mapping
	- 广义上，ORM 指面向对象的对象模型和关系型数据库的数据结构之间的相互转换。
	- 狭义上，ORM 可被认为是基于关系型数据库的数据存储，实现一个虚拟的面向对象的数据访问接口。

- ORM in EF
	- 利用抽象化数据结构，将每个数据库对象转换成应用程序对象(entity)，数据字段转换为属性(property)，关系转换为结合属性(association)。
	- 抽象化结构：
		- 概念层：负责向上的对象与属性显露与访问。
		- 对应层：将上方的概念层和底下的存储层的数据结构对应在一起。
		- 存储层：依不同数据库与数据结构而显露出实体的数据结构体，和 Provider(数据提供者)，负责实际对数据库的访问和 SQL 的产生。

- EF 优点
	- 极大地提高开发效率。
	- 提供了强大的模型设计器。
	- 跨数据库支持。
- EF 缺点
	- 性能不好，性能有损耗(生成 SQL 脚本阶段)。

### Database First 开发方式(旧的开发方式)
- 创建 Database First Demo
- EF 原理
	- 实体数据模型(EDM):EF 中存在一个主要的文件：*.edmx，是 EF 的核心。
- 
### EF 增删改查
- 附加数据库