## mysql语句

------

### 数据库

#### 配置

##### mssql

将文档中所有`MySqlUtil`换成`SqlUtil`。

> 注意，因为`MySqlUtil`和`SqlUtil`依赖的类并不相同，所以创建了两个类，以适配数据库，但是两种数据库的语法是相同的。

修改`appsettings.json`中的`ConnectionStrings`为
```json
{
    "wejhplatform":"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=wejhplatform;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
}
```

##### mysql

导入`NuGet`包`MySQL.Data`。

将文档中所有`SqlUtil`换成`MySqlUtil`。

修改`appsettings.json`中的`ConnectionStrings`为
```json
{
    "wejhplatform":"server=localhost;userid=root;password=123456;database=wejhplatform;"
}
```

#### contents

```json
{
    "name":"wejhplatform",
    "tables":[
        "usercredit",

    ]
}
```

#### create database

```
create database wejhplatform;
use wejhplatform;
```

### 登录模块

#### models

```json
{
    "name": "usercredit",
    "table":{
        "id": "int not null AUTO_INCREMENT",
        "username": "text not null",
        "usertype": "int not null",
        "password": "text not null",
        "token": "text",
        "mobile_name":"text",
        "mobile_credit":"text",
        "pc_name":"text",
        "pc_credit":"text"
    },
    "primarykey":"id"
}
```

#### create table

mysql

```
create table usercredit(
    id int not null AUTO_INCREMENT,
    username text not null,
    usertype int not null,
    password text not null,
    token text,
    mobile_name text,
    mobile_credit text,
    pc_name text,
    pc_credit text,
    PRIMARY KEY (id)
);
```

### 用户信息模块

#### models

```json
{
    "name":"userinfo",
    "table":
    {
        "id":"int not null AUTO_INCREMENT",
        //credit
        "username":"text not null",
        //pwbinds
        "pwbind_lib":"text not null",
        "pwbind_card":"text not null",
        "pwbind_ycedu":"text not null",
        "pwbind_zjedu":"text not null",
        //infos
        "email":"text",
        "phone":"text",
    }
}
```

#### create table

mysql

```
create table userinfo(
    id int not null AUTO_INCREMENT,
    username text not null,
    pwbind_lib text not null,
    pwbind_card text not null,
    pwbind_ycedu text not null,
    pwbind_zfedu text not null,
    email text not null,
    phone text not null
);
```



