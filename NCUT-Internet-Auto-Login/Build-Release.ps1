# NCUT Internet Auto Login V2 - Release 建置腳本
# 自動建置並打包成單一 EXE

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  NCUT Internet Auto Login V2 - Release 建置  " -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# 設定變數
$projectPath = "NCUT-Internet-Auto-Login.csproj"
$configuration = "Release"
$outputDir = "bin\Release"
$exeName = "NCUT-Internet-Auto-Login.exe"

# 檢查專案文件是否存在
if (!(Test-Path $projectPath)) {
    Write-Host "? 錯誤: 找不到專案文件 $projectPath" -ForegroundColor Red
    Write-Host "請確認在專案目錄中執行此腳本" -ForegroundColor Yellow
    pause
    exit 1
}

Write-Host "?? 專案資訊:" -ForegroundColor Green
Write-Host "   專案: $projectPath" -ForegroundColor White
Write-Host "   配置: $configuration" -ForegroundColor White
Write-Host "   輸出: $outputDir" -ForegroundColor White
Write-Host ""

# 步驟 1: 清除舊的建置
Write-Host "?? 步驟 1/3: 清除舊的建置..." -ForegroundColor Yellow
if (Test-Path "bin") {
    Remove-Item -Path "bin" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "   ? 已清除 bin 資料夾" -ForegroundColor Green
}
if (Test-Path "obj") {
    Remove-Item -Path "obj" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "   ? 已清除 obj 資料夾" -ForegroundColor Green
}
Write-Host ""

# 步驟 2: 建置專案
Write-Host "?? 步驟 2/3: 建置 Release 版本..." -ForegroundColor Yellow
Write-Host ""

# 使用 MSBuild 建置
$msbuildPath = "msbuild"
try {
    & $msbuildPath $projectPath /p:Configuration=$configuration /t:Rebuild /v:minimal /nologo
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "   ? 建置成功！" -ForegroundColor Green
    } else {
        Write-Host ""
        Write-Host "   ? 建置失敗！" -ForegroundColor Red
        Write-Host "   請檢查建置錯誤訊息" -ForegroundColor Yellow
        pause
        exit 1
    }
} catch {
    Write-Host ""
    Write-Host "   ? 建置失敗：$_" -ForegroundColor Red
    pause
    exit 1
}
Write-Host ""

# 步驟 3: 驗證輸出
Write-Host "? 步驟 3/3: 驗證輸出檔案..." -ForegroundColor Yellow
$exePath = Join-Path $outputDir $exeName

if (Test-Path $exePath) {
    $fileInfo = Get-Item $exePath
    $fileSize = [math]::Round($fileInfo.Length / 1KB, 2)
    
    Write-Host "   ? EXE 文件已生成" -ForegroundColor Green
    Write-Host "   檔案路徑: $exePath" -ForegroundColor White
    Write-Host "   檔案大小: $fileSize KB" -ForegroundColor White
    Write-Host "   建立時間: $($fileInfo.LastWriteTime)" -ForegroundColor White
} else {
    Write-Host "   ? 找不到 EXE 文件" -ForegroundColor Red
    pause
    exit 1
}
Write-Host ""

# 完成
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  ? 建置完成！" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "?? 單一 EXE 位置:" -ForegroundColor Green
Write-Host "   $exePath" -ForegroundColor White
Write-Host ""
Write-Host "?? 測試建議:" -ForegroundColor Yellow
Write-Host "   1. 創建測試資料夾" -ForegroundColor White
Write-Host "   2. 只複製 EXE 文件" -ForegroundColor White
Write-Host "   3. 執行並測試所有功能" -ForegroundColor White
Write-Host ""

# 詢問是否開啟輸出資料夾
$openFolder = Read-Host "是否開啟輸出資料夾？ (Y/N)"
if ($openFolder -eq "Y" -or $openFolder -eq "y") {
    explorer $outputDir
}

Write-Host ""
Write-Host "按任意鍵結束..." -ForegroundColor Gray
pause
