## 部署服务器

#### On Unbuntu 18.04

注册Microsoft key和产品仓储

```bush
wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb
```

安装依赖项

```bush
sudo dpkg -i packages-microsoft-prod.deb
```

安装 .NET SDK

```bush
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install dotnet-sdk-2.1
```

验证SDK安装情况

```bush
dotnet --info
```

启动项目

```bush
dotnet ${name}.dll
```

安装代理服务器ngnix

```bush
sudo apt-get install nginx
vim /etc/ngnix/sites-available/default
```

编辑server
```vim
server{
    listen 80;
    server_name localhost;
    location /{
        proxy_pass ${location};
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Conncection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
```

安装守护进程

```
sudo apt-get install supervisor
```

修改配置

```
cd /etc/supervisor/conf.d/
touch ${name}.conf
vim text.conf
```

修改内容
```
[program:ShareYunSourse]   
command=dotnet ShareYunSourse.Web.dll 
directory=/usr/ShareYunSourse
environment=ASPNETCORE__ENVIRONMENT=Production
user=www-data 
stopsignal=INT
autostart=true 
autorestart=true 
startsecs=1
stderr_logfile=/usr/log/ShareYunSourse.err.log
stdout_logfile=/usr/log/ShareYunSourse.out.log
```

