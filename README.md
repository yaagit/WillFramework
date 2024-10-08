# Will Framework
### 简介
这是一个用于 Unity 应用开发的 C# 语言类极简 MVC 架构 ── WillFramework。
WillFramework 体量小巧， 支持自动化的 IOC 单例注册、单例依赖注入以及注入权限管理，拥有各种拿来即用的 Attributes。
WillFramework 拥有基于观察者模式的数据驱动事件系统、汇报事件系统和命令事件系统。
WillFramework 支持启动时自动化执行的 Initialize 代码（需要继承 AutoInitialize 接口）。
WillFramework 支持 View 层事件自动注销功能。
### 角色关系
通常情况下，WillFramework 具有三个角色：View，Service，Model。三个角色通讯关系如下所示：

![image](/Assets/WillFramework%20Images/WillFramework003.png)

#### CommandManager
CommandManager 作为框架的受 Ioc 容器托管的内置对象，拥有两个级别：HighLevelCommandManager、LowLevelCommandManager。
HighLevelCommandManager 能够发布和执行命令，LowLevelCommandManager 只能监听命令。每个角色的引用权限可能有所区别，View 层内嵌了 LowLevelCommandManager 和 HighLevelCommandManager 的功能，Service 只能引用 HighLevelCommandManager，Model 无法引用任何类型的 CommandManager。
### 类图关系

（黄色块为用户的自定义类型）

![image](/Assets/WillFramework%20Images/WillFramework004.png)

### View
View 角色比较特殊，通常要继承 Monobehavior，也就说明它的生命周期无法被 WillFramework 掌控。WillFramework 提供了基类 BaseView<T>，默认继承了 Monobehavior，View 通过继承 BaseView 能够获得事件监听、执行以及自动注销的功能。View 需要和 Unity 对接后传入至 Application 的启动参数，只有这样才会被纳入 Ioc 容器的管理。
### Attributes
WillFramework 提供了很多强语义化的注解，[View], [Controller], [Service], [Model], [Identity]，其中 Identity 为原始类型，打上该注解的类的生命周期会受到 WillFramework Ioc 容器的管理，默认模式为单例模式，因此用户无须操心对象的单例创建，回收等问题。
### 权限注入管理
WillFramework 提供了可高度定制的权限注入方式，但是不建议用户在初期修改角色之间的注入权限，以免过度违背 MVC 框架的定义。
