# WebAdmin

����Ǩ�ƽű������
```PowerShell
$env:DOTNET_RUNNING_IN_MIGRATION="true"
dotnet ef migrations add InitMigration
dotnet ef database update
```