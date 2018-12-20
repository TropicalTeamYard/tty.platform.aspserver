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

#### 获取时间

##### 地址

https://server.wejh.imcr.me/api/time

成功

```json
{
	"errcode": 1,
	"errmsg": "获取时间成功",
	"data": {
		"term": "2018\/2019(1)",
		"month": "12",
		"week": 13,
		"day": 4,
		"is_begin": true
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
    "username":"$username", //学号
    "password":"$password", //密码
    "year":"$year", //学年(2017)
    "term":"$term", //学期(3为上学期，12为下学期，16为短学期)
}
```

失败

```json
{
	"status": "error",
	"msg": "参数错误"
}
```

```json
{
	"status": "error",
	"msg": "用户名或密码错误"
}
```

成功

```json
{
	"status": "success",
	"msg": [{
		"cd_id": "19832", //课程id
		"cdmc": "教404", //上课教室(场地名称？)
		"jc": "1-2节",  
		"jcor": "1-2", //第几节上课
		"jcs": "1-2",
		"jgh_id": "1166",
		"jgpxzd": "1",
		"jxb_id": "6CDD0B1E70ED68EBE053A11310AC0090", //教学班id
		"jxbmc": "线性代数-0010", //教学班名称
		"kch_id": "101", 
		"kcmc": "线性代数", //课程名称
		"khfsmc": "考查", //考核方式名称
		"listnav": "false",
		"localeKey": "zh_CN",
		"oldjc": "3",
		"oldzc": "65535",
		"pageable": true,
		"pkbj": "1",
		"rangeable": true,
		"rsdzjs": 0,
		"sxbj": "1",
		"totalResult": "0",
		"xm": "金建国", //教师姓名
		"xnm": "2018", //学年名
		"xqdm": "0",
		"xqh_id": "01",
		"xqj": "1",
		"xqjmc": "星期一",//星期几
		"xqm": "3",
		"xqmc": "朝晖校区",//校区名称
		"xsdm": "01",
		"xslxbj": "◆",
		"zcd": "1-16周",
		"zcmc": "副教授",
		"zyfxmc": "无方向",
        //这些字段是后来开发者为了方便理解添加的字段
		"name": "线性代数:金建国", //课程名称
		"collage": "", //开课学院 *谁把college拼错了?
		"classinfo": "1-16周:星期1(1-2) 教404;", //课程信息
		"classtype": "考查", //课程类型"未安排"/"考查"/"考试"
		"classscore": 2,//学分
		"classhuor": 32//学时 *谁把hour拼错了?
    }]
}
```
