# SearchEngineWithLuceneNet
一个基于Lucene.Net开发的搜索引擎Demo 

**示例项目背景**

本项目模拟一个BBS论坛的文章内容管理系统，当用户发帖之后首先将内容存到数据库，然后对内容进行分词后存入索引库。因此，当用户在论坛站内搜索模块进行搜索时，会直接从索引库中进行匹配并获取查询结果。站内搜索界面的效果如下图所示：

![markdown](https://images2015.cnblogs.com/blog/381412/201604/381412-20160404201050359-320221498.jpg "markdown")
<br/>
**示例项目结构**
<br/>
来看看本Demo的项目结构，虽然只是做一个小Demo，还是使用了简单地三层结构来进行开发：
![markdown](https://images2015.cnblogs.com/blog/381412/201604/381412-20160404201335375-1503290102.jpg "markdown")
（1）Manulife.SearchEngine.Dao

　　顾名思义，数据访问层，与数据库进行交互，各种SQL！

（2）Manulife.SearchEngine.Service

　　业务逻辑层，对数据访问接口进行简单的封装，为UI层提供服务接口。

（3）Manulife.SearchEngine.Model

　　公共的实体对象，为各个层次提供Entity。

（4）Manulife.SearchEngine.Web

　　一个ASP.NET WebForm的网站，主要提供Admin管理操作（文章帖子的CRUD）以及站内搜索（我们的关注点就在这儿）。

**示例效果演示**
   前面说了那么多，终于到了Show Time。不过，也没什么好Show的：
　（1）一周热词
![markdown](https://images2015.cnblogs.com/blog/381412/201604/381412-20160404223153031-216822458.jpg "markdown")<br/>
  （2）搜索提示
![markdown](https://images2015.cnblogs.com/blog/381412/201604/381412-20160404223237593-356452400.png "markdown")<br/>
  （3）搜索结果
![markdown](https://images2015.cnblogs.com/blog/381412/201604/381412-20160404223350890-159850092.jpg "markdown")

### 参考博文

URL：<http://www.cnblogs.com/edisonchou/p/5351930.html>

> @EdisonChou
