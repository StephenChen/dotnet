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
	- 本质就是匿名函数 λ