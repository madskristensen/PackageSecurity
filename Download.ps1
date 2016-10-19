$url = "https://raw.githubusercontent.com/RetireJS/retire.js/master/repository/npmrepository.json"
$file = Resolve-Path ".\src\Resources\npmrepository.json"

Write-Host "Downloading npmrepository.json..." -ForegroundColor Cyan -NoNewline

Invoke-WebRequest $url -OutFile $file

Write-Host "OK" -ForegroundColor Green