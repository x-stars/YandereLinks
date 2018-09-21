# YandereSpider

获取 <https://yande.re/> 上最高质量图片链接的工具，仅能提取链接，不提供下载功能。

## YandereSpider (WPF)

新版 YandereSpider，使用 WPF 作为框架。

内含 `YandereSpider.YanderePage` 类，包含 <https://yande.re/> 页面的常见链接提取功能。
面向对象，便于功能扩展。

能够提取和遍历 Posts 页面（包括搜索结果）和 Pools 页面。

第一次启动时会请求修改内置浏览器版本为 Internet Explorer 11，请以管理员身份启动。

目前处于开发中，未来可能会加入下载功能（可能性不大）。

### 类 `YandereSpider.YanderePage`

此类包含了 <https://yande.re/> 页面的常见链接提取功能，
包括图片链接、（Pools 页面的）Pool 链接和上一页面与下一页面的链接。

同时实现了 `System.Collections.Generic.IEnumerable<YandereSpider.YanderePage>` 接口，
能通过 `foreach` 控制语句从当前页面遍历至最后一页。

## YandereSpider.WinForm

旧版 YandereSpider，已停止更新，未来不会开发新功能和修复错误，仅保留源代码作为参考。

使用 WinForm 作为框架。早期作品，基本面向过程。

只能遍历 Posts 页面（包括搜索结果）和提取单个 Pool 页面，不支持遍历 Pools 页面。
