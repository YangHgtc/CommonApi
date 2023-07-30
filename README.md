# CommonApi
基于单体架构的Asp.net Core Webapi

# 整体架构

```ts
├─.template.config--模板的配置文件
└─src
    ├─1-PresentationLayer--表现层
    │  ├─CommonApi.Api--webapi
    │  │  ├─Controllers--controllers
    │  │  └─Properties--配置文件
    │  └─CommonApi.Common--通用webapi配置
    │      ├─Common--通用
    │      ├─Extensions--扩展
    │      └─Middlewares--中间件
    ├─2-BusinessLayer--业务层
    │  └─CommonApi.Business
    │      ├─IServices
    │      └─Services
    ├─3-DataLayer--数据层
    │  ├─CommonApi.DataBase--数据库
    │  │  └─Dapper
    │  ├─CommonApi.Mapper--实体对象映射
    │  │  └─Mapster
    │  ├─CommonApi.Repository--仓储层
    │  │  ├─IRepositories
    │  │  └─Repositories
    │  └─CommonApi.Validation--fluentvalidation
    │      └─RequestValidators
    ├─4-EntityLayer--实体层
    │  ├─CommonApi.DTO--请求响应和中间dto
    │  │  ├─Requests
    │  │  ├─Responses
    │  │  └─ServiceDTO
    │  └─CommonApi.Entity--数据库实体类
    └─5-CommonLayer--公共层
        └─CommonApi.Util--常用工具类
            ├─Extensions--扩展类
            └─Helpers--帮助类
```

