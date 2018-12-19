## API文档

> `GET`方法已禁止，必须使用`POST`方法。

#### 登录

##### 地址

```
${root}/api/login
```

##### 参数

```json
{
    "username":"$username",
    "password":"$password",
    "devicetype":"mobile/pc",
    "devicename":"devicename"
}
```

> devicetype 必须指定为`mobile`或`pc`否则将无法正常访问。

##### 返回

返回状态码及消息

| code | msg |
| :---: | --- |
| 400 | 无效的访问。 |
| 403 | 设备类型不符合。 |
| 403 | 用户名或密码不能为空。 |
| 403 | 未注册账户。 |
| 403 | 密码错误。 |
| 200 | 登录成功。 |

成功返回数据

```json
{
    "code":200,
    "msg":"登录成功",
    "data":{
        "username":"201806061201",
        "usertype":1,
        "credit":"706637cb00ce4ab7a1c73014524d2847",
        "devicename":"android7.4"
    }
}
```

#### 自动登录

##### 地址

```
${root}/api/autologin
```

##### 参数
```json
{
    "credit":"$credit"
}
```

##### 返回

返回状态码及消息

| code | msg |
| :---: | --- |
| 400 | 无效的访问。 |
| 403 | 自动登录失败。 |
| 403 | 自动登录已失效，请重新绑定账号。 |
| 200 | 自动登录成功。 |

> 自动登录后，原来的`credit`会失效，这是为了提高安全性。

成功返回数据

```json
{
    "code":200,
    "msg":"自动登录成功",
    "data":{
        "username":"201806061201",
        "usertype":1,
        "credit":"706637cb00ce4ab7a1c73014524d2847",
        "devicename":"android7.4"
    }
}
```

#### 绑定密码

##### 地址

```
${root}/api/pwbind
```

##### 参数
```json
{
    "credit":"$credit",
    "bindname":"$bindname",
    "password":"$password",
}
```

```json
{
    "code":200,
    "msg":"$$$绑定成功"
}
```

