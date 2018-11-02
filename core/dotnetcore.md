- .NET Core：
	- 基本的类库(Core FX)
	- 采用 RyuJIT 编译的运行平台 Core CLR
	- 编译器平台 .NET Compiler Platform
	- 采用 AOT 编译技术运行最优化的包 Core RT (.NET Core Runtime)
	- 跨平台的 MSIL 编译器 LLILC (LLVM-based MSIL Compiler)

- RyuJIT 用以替换现有的 .NET Framework 的 JIT 及 JIT64。支持 SIMD(Single Instruction, Multiple Data)。应用于 .NET Framework 4.6 及 .NET Core。

- Core CLR 移植了 .NET Framework 的 CLR 的功能。包含核心程序库 mscorlib、JIT 编译器、GC 以及其他运行 MSIL 所需要的运行时环境。

- Core RT 以 AOT(Ahead-of-time)编译方式为主的核心功能，在 .NET Core 内称为 Core RT，在 UWP(Universal Windows Platform, 通用应用平台)则被称为 .NET Native。
- Core RT 会在构建时期(非运行时)在编译时将 MSIL 转换成平台本地的机器码，优点是引导时间短(JIT 采用的是运行时编译，使得引导时间拉长)，并且内存用量少。Core RT 在不同的平台会使用不同的 AOT 技术：
	- Windows 使用 .NET Native。
	- Mac OSX 与 Linux 使用 LLILC(同时支持 JIT 和 AOT)。

- LLILC (LLVM-based MSIL Compiler)是 .NET Core 在非Windows平台的 MSIL 编译器，基于 ECMA-335(Common Language Infrastructure)的标准将 MSIL 编译成源码运行，适用于可运行 LLVM 的操作系统。
- LLILC 同时支持 JIT(内含 RyuJIT 的实现)和 AOT(未来将开始支持)的编译方式。

- Roslyn .NET Compiler Platform(项目代码为 Roslyn)是将 .NET 平台的编译器架构标准化的平台，它可提供程序管理工具(如集成开发环境)情报，用以发展有助于编写程序与管理程序结构所需要的功能，如类型信息、语法结构、参考链接、语义、编译器、自动化、错误报告等功能，只要是遵循 CLI 标准的编程语言，都可用 .NET Compiler Platform 实现编译器。

- 通过 ASP.NET Core 可以获得的改进：
	- 一个统一的方式用于构建 Web UI 和 Web APIs。
	- 集成现代的客户端开发框架和开发流程。
	- 一个适用于云的，基于环境的配置系统。
	- 内置的依赖注入。
	- 新型的、轻量级的、模块化 HTTP 请求管道。
	- 运行于 IIS 或者自宿主(self-host)于你自己的进程的能力。
	- 基于支持真正的 side-by-side 应用程序版本化的 .NET Core 构建。
	- 完全以 NuGet 包的形式发布。
	- 新的用于简化现代 Web 开发的工具。
	- 可跨平台构建和运行 ASP.NET 应用。
	- 开源并且重视社区。

- ASP.NET Core 一个在 Main 方法中创建一个 Web 服务器的简单控制台应用程序。
- Main 调用遵循 builder 模式的 WebHostBuilder，创建一个 Web 应用宿主。这个 builder 具有用于定义 Web 服务器(如 UseKestrel)和 startup 类型(UseStartup)的方法。Build 和 Run 方法构建了用于宿主应用程序的 IWebHost，启动它来监听传入的 HTTP 请求。
- Startup 可用来定义请求处理管道和配置应用需要的服务。Startup 类必须是公开的。ConfigureServices 方法用于定义应用所使用的服务(如，ASP.NET MVC Core framework、Entity Framework Core、Identity)。Configure 方法定义请求管道中的中间件。
- 服务 应用中用于通用调用的组件。服务通过依赖注入获取并使用。
- 中间件 使用中间件构建请求处理管道。
- 服务器 ASP.NET Core 托管模式不直接监听请求，依赖于一个 HTTP Server 实现来转发请求到应用程序。
- 内容根目录 是应用程序所用到的所有内容的根路径。
- 网站根目录 项目中类似于 CSS、JS 和图片文件公开、静态的资源的目录。
- 配置 一个有序拉去数据的配置 providers。
- 环境 Development, Production。
- 使用 ASP.NET Core MVC 构建 Web UI 和 Web APIs
	- 可以使用 Model-View-Controller(MVC) 模式创建优秀的、可测试的 Web 应用程序。
	- 可以构建支持多种格式并且完全支持内容协商的 HTTP 服务。
	- Razor 提供了一种高效的语言，用于创建 Views。
	- Tag Helpers 启用服务器端的代码参与到 Razor 文件的创建和 HTML 元素渲染。
	- 可以使用自定义或者内置的 formatters(JSON, XML) 来构建完全支持内容协商的 HTTP 服务。
	- Model Binding 自动映射 HTTP 请求中的数据到 action 方法参数。
	- Model Validation 自动执行客户端和服务器端验证。

