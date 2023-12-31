## Comparisons between word files

**项目描述：** 

​		一个 Windows 窗体应用程序，旨在提供一个高效且直观的文档比对工具，用于执行 Word 页面比对操作。借助算法和用户友好的界面，用户可以比较两个或多个文档之间的页面内容，并生成相似度报表和热图以提供清晰的视觉展示。

**技术特点：** 

​		该项目基于Microsoft 的 WinForms 框架开发，使用 C# 编程语言和 Visual Studio 集成开发环境。核心比对算法是最小编辑距离算法（*The Minimum Edit Distance Algorithm*），其高效地计算文本之间的差异并在界面上进行可视化显示。项目采用自定义的图形绘制，可实现页面高亮、文本差异标记和热图生成等功能。

**功能特点：** 

​		具备如下功能特点：

- 实现了文档页面比对，支持双文档比对和多文档比对两种模式。
- 在双文档比对模式下，用户可导入两个文档或手动输入内容，然后执行比对操作。
- 提供了多种界面按钮，如导入文档、执行比对、切换比对样式、返回菜单等。
- 多文档比对模式支持添加、移除和清空文档，进行一次性比对操作，并生成相似度报表和热图。
- 可导出总结文档相似度的报表，为用户提供清晰的比对结果。

**界面设计：**

​		 通过直观的按钮和选项，用户可以轻松导入、比对和分析文档。切换比对方式等，从而满足不同的比对需求。

**未来展望：** 

​		随着项目的发展，我们计划不断优化和扩展功能，增加更多的比对算法、界面自定义选项以及与外部工具的集成，以满足更广泛的用户需求。同时，我们会持续关注用户反馈，确保项目在功能和性能上保持卓越。