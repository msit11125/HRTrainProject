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
	` Scaffold-DbContext -UseDatabaseNames "Server=localhost;User Id=root;Password=from1992;Database=HRTrainDb" "Pomelo.EntityFrameworkCore.MySql" -OutputDir Models -force ` <br/>
	- 讀取SqlServer
	` Scaffold-DbContext -UseDatabaseNames "Server=.\sqlexpress;Database=HRTrainDb;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force ` <br/>
	` dotnet ef dbcontext scaffold "Server=(localdb)..." -o Models -f -UseDatabaseNames ` <br/>
	` dotnet ef database update -UseDatabaseNames `
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
[官方文件](https://semantic-ui.com/introduction/getting-started.html)


#### 7. X.PagedList
[參考Blog](https://github.com/dncuug/X.PagedList)

#### 8. ViewComponents
[MSDN](https://docs.microsoft.com/zh-tw/aspnet/core/mvc/views/view-components?view=aspnetcore-2.1)
