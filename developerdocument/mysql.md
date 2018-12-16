## mysql语句

------

### 数据库

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
        "username": "string not null",
        "usertype": "int not null",
        "password": "string not null",
        "token": "string",
        "mobile_name":"string",
        "mobile_credit":"string",
        "pc_name":"string",
        "pc_credit":"string"
    },
    "primarykey":"id"
}
```

#### create table
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