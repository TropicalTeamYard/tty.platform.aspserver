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
```

### 微信端微精弘后台

##### 登录**GET**

```json
{

}
```

成功

```json
{
	"errcode": 200,
	"errmsg": "登陆成功",
	"data": {
		"user": {
			"uno": "201806061201",
			"ext": {
				"school_info": {
					"name": null,
					"gender": 0,
					"grade": null,
					"college": null,
					"major": null,
					"class": null
				},
				"wechat_info": {
					"avatar": null,
					"nickname": null,
					"subscribe": false
				},
				"terms": {
					"score_term": "2017\/2018(1)",
					"class_term": "2017\/2018(1)",
					"exam_term": "2017\/2018(1)"
				},
				"passwords_bind": {
					"jh_password": 1,
					"card_password": 1,
					"lib_password": 1,
					"yc_password": 0,
					"zf_password": 0
				}
			},
			"updated_at": "2018-12-11 06:05:33",
			"created_at": "2018-12-11 06:05:33",
			"id": 45
		},
		"token": "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOjQ1LCJpc3MiOiJodHRwczovL3Rlc3Quc2VydmVyLndlamguaW1jci5tZS9hcGkvbG9naW4iLCJpYXQiOjE1NDQ1MDgzMzMsImV4cCI6MTU0NDU5NDczMywibmJmIjoxNTQ0NTA4MzMzLCJqdGkiOiI5TEVSVXdMWUFuOEt6RjlJIn0.wkjz4MpQDd-RcXmNpaEC7cM6LA0Pdr71OdPgB46QRY8"
	},
	"redirect": null
}
```

### 正方教务系统

#### 查询成绩**GET**

##### 地址

http://api.jh.zjut.edu.cn/student/classZf.php

#### 入参

```json
{
    "username":"text", //学号
    "password":"text", //密码
    "year":"int", //学年(2017)
    "term":"int", //
}
```

