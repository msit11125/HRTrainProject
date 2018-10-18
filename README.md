#### 1. 指令
- Nuget安裝
    - dotnet/cli: ` dotnet add package <packagename> `
    - nuget console: `Install-Package <packagename>`

- Command line ( vs code )
    - ` dotnet run `

- Npm安裝
	- ` npm install <packagename> --save `

#### 2. EntityFrameworkCore
- 套件:
` DbContext取得Connection: Microsoft.EntityFrameworkCore.Relational ` <br />
` Sqlserver: Microsoft.EntityFrameworkCore.SqlServer ` <br/>
` Mysql: Pomelo.EntityFrameworkCore.MySql `
- 指令 (對象專案 HRTrainEF):
	- 新增移轉
	` Add-Migration InitDB ` <br/>
	` dotnet ef migrations add InitDB `
	- 更新Db
	` Update-Database ` 
	- 讀取MySql
	` Scaffold-DbContext "Server=localhost;User Id=root;Password=from1992;Database=HRTrainDb" "Pomelo.EntityFrameworkCore.MySql" -OutputDir Models -force ` <br/>
	- 讀取SqlServer
	` Scaffold-DbContext "Server=.\sqlexpress;Database=HRTrainDb;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force ` <br/>
	` dotnet ef dbcontext scaffold "Server=(localdb)..." -o Models -f ` <br/>
	` dotnet ef database update `
#### 3. 多國語系
- 套件:
`　Microsoft.AspNetCore.Localization ` <br/>
[參考Blog]( https://ithelp.ithome.com.tw/articles/10196463 )

#### 4. 登入機制
- 套件:
` Microsoft.AspNetCore.Authentication.Cookies ` <br/>
[參考Blog](http://future-shock.net/blog/post/creating-a-simple-login-in-asp.net-core-2-using-authentication-and-authorization-not-identity)

#### 5. NLog
- 套件:
` NLog.Web.AspNetCore ` <br/>
` NLog ` <br/>
` NLog.Config ` <br/>
[參考Blog](https://github.com/NLog/NLog.Web/wiki/Getting-started-with-ASP.NET-Core-2)

#### 6. Semantic-UI
- Dependency: gulp:  ` npm install -g gulp `
[參考Blog](https://semantic-ui.com/introduction/getting-started.html)


#### 7. X.PagedList
[參考Blog](https://github.com/dncuug/X.PagedList)