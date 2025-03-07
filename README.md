# WebAdmin

运行迁移脚本的命令：
```PowerShell
$env:DOTNET_RUNNING_IN_MIGRATION="true"
dotnet ef migrations add InitMigration
dotnet ef database update
```