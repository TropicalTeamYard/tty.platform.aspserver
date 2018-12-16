## 需要使用的api及其说明

------

### 精弘用户中心

#### 地址

```json
{
    "jh.user":"http://user.jh.zjut.edu.cn/api.php"
}
```

##### 登录**GET**

```json
{
    "app":"passport",
    "action":"login",
    "passport":"$username",
    "password":"$password"
}
```

返回数据

错误1（未注册）

```json
{
    "state":"error",
    "info":"通行证（学号）不存在或者未注册，如未注册，请注册后重新登陆"
}
```

成功
```json
{
    "state":"success",
    "info":"登录成功",
    "data":{
        "pid":"201806061201",
        "email":"t1542462994@outlook.com",
        "currentUidCount":"0",
        "availableUidCount":"3",
        "lastLoginTime":"1544883861",
        "lastLoginIP":"10.128.69.69",
        "activeTime":"1534913073",
        "activeIP":"172.16.9.101",
        "type":"1",
        "zjutmail":null
    }
}

