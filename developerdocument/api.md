## API文档

> `GET`方法已禁止，必须使用`POST`方法。

> 当前服务器地址`http://39.108.120.239`

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
    "bindname":"$bindname", //lib:图书馆,card:校园卡,ycedu:原创教务,zfedu:正方教务.
    "password":"$password",
}
```

```json
{
    "code":200,
    "msg":"绑定%成功。" //zfedu:绑定正方成功。
}
```

> 当前只有正方教务的绑定，其他类型还在开发中。

#### 获取时间

> `GET`和`POST`方法都可以。

##### 地址

```
${root}/api/time
```

##### 返回

```json
{
    "code":200,
    "msg":"获取时间成功",
    "data":{
        "year":2018,
        "term":3,//3为上学期，12为下学期，16为短学期。
        "begin":"2018-09-24",
        "end":"2019-01-27",
        "dayofweek":4,
        "week":13,
        "weeklasting":18
    }
}
```

#### 获取课表

##### 参数

```json
{
    "credit":"$credit"
}
```

> 获取课表是按照服务器时间来获取的，而服务器时间可有`api/time`来获取。

##### 返回

成功

```json
{
	"code": 200,
	"msg": "获取课表成功。",
	"data": [{
		"id": 0,
		"courseid": "19832",
		"year": 2018,
		"term": 3,
		"name": "线性代数",
		"college": "",
		"type": "考查",
		"teacher": "金建国",
		"campus": "朝晖校区",
		"location": "教404",
		"weekrange": "1-16",
		"dayofweek": "1",
		"timerange": "1-2",
		"classscore": 2,
		"classhour": 32
	}, {
		"id": 0,
		"courseid": "6D910A62D3DC31D9E053A11310ACD644",
		"year": 2018,
		"term": 3,
		"name": "大学英语",
		"college": "",
		"type": "考试",
		"teacher": "阮晓亮",
		"campus": "朝晖校区",
		"location": "文605",
		"weekrange": "1-16",
		"dayofweek": "1",
		"timerange": "3-4",
		"classscore": 2,
		"classhour": 32
	}, {
		"id": 0,
		"courseid": "19815",
		"year": 2018,
		"term": 3,
		"name": "高等数学Ⅰ",
		"college": "",
		"type": "考试",
		"teacher": "练晓鹏",
		"campus": "朝晖校区",
		"location": "教201",
		"weekrange": "1-16",
		"dayofweek": "1",
		"timerange": "6-7",
		"classscore": 2,
		"classhour": 32
	}, {
		"id": 0,
		"courseid": "19818",
		"year": 2018,
		"term": 3,
		"name": "程序设计基础C",
		"college": "",
		"type": "考试",
		"teacher": "潘清",
		"campus": "朝晖校区",
		"location": "教204",
		"weekrange": "1-16",
		"dayofweek": "2",
		"timerange": "1-2",
		"classscore": 2,
		"classhour": 32
	}, {
		"id": 0,
		"courseid": "19822",
		"year": 2018,
		"term": 3,
		"name": "人体健康与疾病",
		"college": "",
		"type": "未安排",
		"teacher": "陈艳",
		"campus": "朝晖校区",
		"location": "教301",
		"weekrange": "1-16",
		"dayofweek": "2",
		"timerange": "10-11",
		"classscore": 2,
		"classhour": 32
	}, {
		"id": 0,
		"courseid": "19832",
		"year": 2018,
		"term": 3,
		"name": "中国近现代史纲要",
		"college": "",
		"type": "考查",
		"teacher": "颜桂珍",
		"campus": "朝晖校区",
		"location": "教404",
		"weekrange": "1-16",
		"dayofweek": "3",
		"timerange": "1-2",
		"classscore": 2,
		"classhour": 32
	}, {
		"id": 0,
		"courseid": "19815",
		"year": 2018,
		"term": 3,
		"name": "高等数学Ⅰ",
		"college": "",
		"type": "考试",
		"teacher": "练晓鹏",
		"campus": "朝晖校区",
		"location": "教201",
		"weekrange": "1-16",
		"dayofweek": "3",
		"timerange": "3-4",
		"classscore": 2,
		"classhour": 32
	}, {
		"id": 0,
		"courseid": "6D910A62D3DC31D9E053A11310ACD644",
		"year": 2018,
		"term": 3,
		"name": "大学英语",
		"college": "",
		"type": "考试",
		"teacher": "阮晓亮",
		"campus": "朝晖校区",
		"location": "文605",
		"weekrange": "1-16",
		"dayofweek": "3",
		"timerange": "6-7",
		"classscore": 2,
		"classhour": 32
	}, {
		"id": 0,
		"courseid": "19827",
		"year": 2018,
		"term": 3,
		"name": "程序设计基础C",
		"college": "",
		"type": "考试",
		"teacher": "潘清",
		"campus": "朝晖校区",
		"location": "教307",
		"weekrange": "1-16",
		"dayofweek": "4",
		"timerange": "3-4",
		"classscore": 2,
		"classhour": 32
	}, {
		"id": 0,
		"courseid": null,
		"year": 2018,
		"term": 3,
		"name": "体育",
		"college": "",
		"type": "未安排",
		"teacher": "王德明",
		"campus": "朝晖校区",
		"location": "未排地点",
		"weekrange": "1-16",
		"dayofweek": "4",
		"timerange": "6-7",
		"classscore": 2,
		"classhour": 32
	}, {
		"id": 0,
		"courseid": "19815",
		"year": 2018,
		"term": 3,
		"name": "高等数学Ⅰ",
		"college": "",
		"type": "考试",
		"teacher": "练晓鹏",
		"campus": "朝晖校区",
		"location": "教201",
		"weekrange": "1-8",
		"dayofweek": "5",
		"timerange": "6-7",
		"classscore": 2,
		"classhour": 32
	}]
}
```
