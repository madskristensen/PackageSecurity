$url = "https://raw.githubusercontent.com/RetireJS/retire.js/master/repository/npmrepository.json"
$file = Resolve-Path ".\src\Resources\npmrepository.json"

Invoke-WebRequest $url -OutFile $file