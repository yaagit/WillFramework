# Will Framework
### 简介
这是一个用于 Unity 游戏开发的 C# 语言类 MVC 架构 ── WillFramework。WillFramework 体量小巧， 支持自动化的 IOC 单例注册、单例依赖注入以及自由度极高的注入权限管理，支持轻松、简单的注入权限配置，同时也拥有各种拿来即用的语义化超强的 Attributes。对用户代码的侵入性极小（除了 View，无需强制要求你继承框架的一个基类，甚至也不用继承什么接口）。WillFramework 拥有基于观察者模式的数据驱动事件系统、汇报事件系统和命令事件系统。支持启动自动化执行的 Initialize 代码（需要继承 AutoInitialize 接口）。
### 角色关系
通常情况下，WillFramework 具有四个角色：Controller，View，Service，Model。除了 Controller，其他三个两两互相解耦，无法互相引用。Controller 作为总的调度中心可以引用所有的 View，Service，Model。它们之间通过 RepotAction 和 CommandManger 来进行通讯，具体通讯关系如下所示：

![image](/Assets/WillFramework%20Images/WillFramework001.png)
#### CommandManager
CommandManager 作为框架的受 Ioc 容器托管的内置对象，拥有两个级别：HighLevelCommandManager、LowLevelCommandManager。HighLevelCommandManager 只能够发布和执行命令，LowLevelCommandManager 只能监听命令。每个角色的引用权限不一样，Controller 只能够引用 HighLevelCommandManager，View、Service 只能引用 LowLevelCommandManager，Model 无法引用任何类型的 CommandManager。
#### ReportAction
ReportAction 作为专供 Service，View 使用的事件委托类，提供的功能很简单，就是委托给 Controller。由于 Model，Service，View 三个角色无法互相引用，因此它也无法委托给除了 Controller 外的其他角色。
### 类图关系
（黄色块为用户的自定义类型）
![image](/Assets/WillFramework%20Images/WillFramework002.png)
### View
View 角色比较特殊，通常要继承 Monobehavior，也就说明它的生命周期无法被 WillFramework 掌控。在使用 WillFramework 的过程中也要时刻记得这一点，不要把初始化框架类的代码放到 Start 函数里执行，因为你无法预料它执行的时机。WillFramework 提供了 BaseView<T>，默认继承了 Monobehavior，View 通过继承 BaseView 能够获得被动事件注销的功能。View 角色需要和 Unity 对接后传入 Application 的启动参数，这样才会被纳入 Ioc 容器的管理。
### Attributes
WillFramework 提供了很多强语义化的注解，[View], [Controller], [Service], [Model], [Identity]，其中 Identity 为原始类型，打上该注解的类的生命周期会受到 Ioc 容器的管理，默认模式为单例模式，因此用户无须操心对象的单例创建，回收等问题。在需要继承 Monobehavior 的情况下，不建议用户使用 [View]，因为 View 角色需要和 Unity 对接传入。 
### 启动自动化代码
WillFramework 强烈建议把初始化代码放进 AutoInitialize 方法里执行，尤其是初始化受 IOC 容器托管的引用。
### 注入权限管理
WillFramework 提供了可高度定制的注入权限方式，但是初期不建议用户修改角色之间的注入权限，以免过度违背 MVC 框架的定义。
