# Common shortcut keys
# ctrl+p  search files       ctrl+shift+o   搜索属性方法


#   无法将“Scaffold-DbContext”项识别为 cmdlet、函数、脚本文件或可运行程序的名称。 nuget安装 Microsoft.EntityFrameworkCore.Tools


# 进入含有包含项目或解决方案文件的目录  dotnet build
# ctrl+shift+b 编译项目

# dotnet restore - 恢复项目的依赖项和工具。   根据csproj nuget配置下载对应的package  
# dotnet run  在项目文件所在目录执行源代码      dotnet run   --urls  "http://*.7000"
# dotnet app.dll   执行main函数所在dll
#  dotnet publish  -c  release
# dotnet publish [项目路径(在当前项目路径下不需要指定)] -o [发布后的文件存放路径]
#  dotnet test.dll & 后台运行 ssh终端关闭test进程也会关闭

# 这个launchSettings.json 的配置   Now listening on: https://localhost:6001  Now listening on: http://localhost:6000   但是http://localhost:6000不能访问 只能访问https://localhost:6001
 #  "launchUrl": "http://localhost:6000/api/persons",
 #  "applicationUrl": "https://localhost:6001;http://localhost:6000", 

# Scaffold-DbContext "Server=127.0.0.1;Port=3306;Database=mydb; User=root;Password=root;Pooling=true; Max Pool Size=100;" Pomelo.EntityFrameworkCore.MySql -OutputDir Entities   -Force




#     div>ul>li*3>a


# ctrl+shift+\  匹配大括号


#  vs   CTRL + SHIFT + F9 取消所有断点  注意不是vscode的快捷键