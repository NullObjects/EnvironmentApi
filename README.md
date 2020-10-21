# Environment
 - [查看Kotlin版](https://github.com/NullObjects/EnvironmentService)

### 框架
- ASP .Net Core Web Api
- Microsoft.EntityFrameworkCore.Tools
- MySql.Data.EntityFrameworkCore

### 加密策略
1. 由前台向Api发起请求获取RSA公钥
2. 获取公钥后前台向Api发起登录请求，携带RSA公钥加密后的登录信息
3. Api接收到请求后RSA私钥解密登录信息，然后AES加密登录信息
4. 将AES加密后数据与数据库中用户信息对比，如果无误，返回前台携带用户身份及角色的jwt
5. 前台获取用户信息，删除信息，修改信息等均需携带jwt
6. 后台可根据需要控制特定用户身份或特定角色等执行权限

### 配置
- appsettings.json
```
  "ConnectionStrings": {
    "EnvironmentConnection": "Server=; Database=; uid=; password=;"
  },
  "tokenManagement": {
    "secret": "",
    "issuer": "environment.cn",
    "audience": "EnvironmentApi",
    "accessExpiration": 30,
    "refreshExpiration": 60
  },
```

###
- 快捷启动
```
.zshrc添加
alias env_asp="cd ~/source/ASP/EnvironmentApi && sudo dotnet EnvironmentApi.dll"
```

### 数据接口
- 已配置https，可直接访问https://<ip地址>/<数据>/Get/<参数>
- method : get
#### <数据>
```
Environment : 环境信息
DeviceStatus : 设备状态
```
#### <参数>
- 时间段
```
以下时间均自当前时间推算
hour : 一小时内
day : 一天内
week : 一周内
other : 最新一条数据
```
```
yyyy-mm-ddThh:MM:ss&&yyyy-mm-ddThh:MM:ss : 开始时间&&结束时间
具体时间可省略，即yyyy-mm-dd&&yyyy-mm-dd
不得大于7天，否则数据量过大
```

### 用户管理接口
- 以下role中public为公共角色(必要)，admin为管理员角色
- 管理其他用户信息除public外，还需要admin角色(header["Authorization"]拥有role为public和admin的jwt)
#### 获取公钥
- URL : https://<ip地址>/Authentication/GetKey
- method : get
- 使用公钥加密password及old_password
#### 注册
- URL : https://<ip地址>/Authentication/Register
- method : post
- 注册管理员需要role为admin的jwt
```
body
{
    "username":"",
    "email":"",
    "password":"",
    "role":"public<::role1><::role2>"
}
```
#### 登录
- 获取jwt
- URL : https://<ip地址>/Authentication/Login
- method : post
```
body
{
    "username":"",
    "password":""
}
```
#### 修改用户信息
- URL : https://<ip地址>/Authentication/Modify
- method : post
- 需要role为public的jwt
```
body
{
    "username":"",
    "email":"",
    "oldPassword":"",
    "password":"",
    "role":"public<::role1><::role2>"
}
```
#### 获取用户信息
- URL : https://<ip地址>/Authentication/Information
- method : get
- 需要role为public的jwt
#### 删除用户信息
- URL : https://<ip地址>/Authentication/Delete/<用户名>
- method : get
- 需要role为public的jwt
