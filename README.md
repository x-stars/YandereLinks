# YandereSpider

获取 <https://yande.re/> 上最高质量图片链接的工具，仅能提取链接，不提供下载功能。

## YandereSpider (WPF)

新版 YandereSpider，使用 WPF 作为框架。面向对象，便于功能扩展。

支持语言

* 简体中文
* English

以内置浏览器作为导航，所有提取链接的操作均针对当前浏览器显示的页面。

能够提取和遍历 Posts 页面（包括搜索结果）、Pool 页面和 Pools 页面。

内含 `YandereSpider.YanderePage` 类，包含 <https://yande.re/> 页面的常见链接提取功能。

### 控制台模式

本应用程序支持控制台启动，相对于窗口启动，主要优势在于**多线程并发访问**。
窗口模式则因设计理念（浏览器导航）原因，不提供并发访问功能。

控制台模式的启动参数说明如下。

YandereSpider.exe PageLink [-e PageCount] [-t MaxThreads] [-o OutFile] [-h]

| 名称 | 内容       | 说明                                                              |
| ---- | ---------- | ----------------------------------------------------------------- |
|      | PageLink   | 要提取链接的 yande.re 页面的 URL。支持多值输入（最大 65536）。    |
| -e   | PageCount  | 指定要遍历页面的数量；为 0 则不进行遍历，为 -1 则遍历至最后一页。 |
| -t   | MaxThreads | 指定 HTTP 访问的最大线程数。                                      |
| -o   | OutFile    | 指定输出图片链接的文本文件的路径。                                |
| -h   |            | 显示简要的帮助信息。                                              |

完成图片链接的提取后，若链接数量小于 65536，则会将所有链接复制到剪贴板。

### 类 `YandereSpider.YanderePage`

实现接口：

* `System.IDisposable`
* `System.Collections.Generic.IReadOnlyList<out YandereSpider.YanderePage>`
* `System.IEquatable<YandereSpider.YanderePage>`

包含了 <https://yande.re/> 页面的常见链接提取功能，
包括图片链接、（Pools 页面的）Pool 链接和上一页面与下一页面的链接。

访问级别为公共 `public`，可通过引用 YandereSpider.exe 来访问此类。

## YandereSpider.WinForm

旧版 YandereSpider，使用 WinForm 作为框架。早期作品，基本面向过程。

已停止更新，未来不会开发新功能和修复错误，仅保留源代码作为参考。

只能遍历 Posts 页面（包括搜索结果）和提取单个 Pool 页面，不支持遍历 Pools 页面。
