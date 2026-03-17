# NCUT Internet Auto Login V2 - C# Version

## 簡介
這是 NCUT 校園網自動登入工具的 C# Windows Forms 版本，從 Python 腳本轉換而來。
**現在使用專業的 MetroFramework UI 函式庫！**

## 🎨 介面特色（NEW!）
- ✨ **MetroFramework 整合** - 使用成熟的開源 UI 函式庫
- 🎯 **Metro 設計風格** - 現代化、專業的視覺效果
- 🌙 **深色主題** - 預設使用舒適的深色主題
- 📦 **統一控件樣式** - 所有控件使用一致的 Metro 風格
- 💫 **流暢體驗** - 專業的按鈕、文字框、面板設計
- 🎨 **可自訂主題** - 支援多種配色方案
- 📱 **穩定可靠** - 基於成熟的開源函式庫
- 🚀 **單一 EXE** - 使用 Costura.Fody 打包成單一執行檔（NEW!）

## 功能特點
- ✅ 自動檢測網路連線狀態
- ✅ 連線失敗時自動嘗試登入
- ✅ **現代化 GUI 介面** - Electron 風格設計
- ✅ 即時日誌顯示
- ✅ 支援自訂帳號和密碼
- ✅ **系統托盤支援** - 最小化到系統托盤，不佔用任務欄空間
- ✅ **開機自動啟動** - 可設定 Windows 開機時自動執行
- ✅ **後台啟動模式** - 可選擇自啟動時直接在後台運行（最小化到托盤）
- ✅ 托盤右鍵選單快速操作
- ✅ **自訂圖示** - 支援使用 ic_launcher.ico 圖示

## 使用說明

### 基本操作
1. 啟動應用程式
2. 在現代化的介面中輸入您的校園網帳號和密碼
3. 點擊藍色的「開始監控」按鈕
4. 程式將自動監控網路連線狀態，並在需要時自動登入
5. 點擊紅色的「停止監控」按鈕可停止監控
6. 點擊「清除日誌」按鈕可清除日誌記錄

### 現代化介面說明
- **標題欄**: 深色主題，可拖動視窗，右側有最小化/最大化/關閉按鈕
- **帳號密碼卡片**: 第一個白色卡片，輸入登入資訊
- **設定卡片**: 第二個白色卡片，包含自動啟動選項
- **日誌卡片**: 第三個白色卡片，顯示系統日誌

### 系統托盤功能
- **最小化到托盤**: 點擊視窗的最小化按鈕，程式會縮到系統托盤
- **關閉到托盤**: 點擊視窗的關閉按鈕（X），程式會隱藏到托盤而不會結束
- **雙擊托盤圖示**: 顯示主視窗
- **右鍵托盤圖示**: 開啟快速選單
  - 顯示視窗 / 隱藏視窗
  - 開始監控 / 停止監控
  - 結束程式

### 開機自動啟動
1. **啟用自動啟動**
   - 勾選設定卡片中的「開機自動啟動」選項
   - 程式會在 Windows 登入時自動執行

2. **後台啟動模式（新功能）**
   - 先啟用「開機自動啟動」
   - 再勾選「自啟動時在後台運行」
   - 程式將在開機時直接最小化到系統托盤，不會顯示主視窗
   - 適合需要靜默運行的場景

3. **設定說明**
   - 使用 Windows 登錄檔（Registry）實現，安全可靠
   - 兩個選項互相關聯：
     - 取消「開機自動啟動」會自動停用「自啟動時在後台運行」
     - 「自啟動時在後台運行」只有在啟用自動啟動時才能選擇

### 命令列參數
程式支援以下命令列參數：
- `-minimized` 或 `/minimized`: 啟動時直接最小化到系統托盤
- 範例：`NCUT-Internet-Auto-Login.exe -minimized`

### 自訂圖示
程式已設定好使用 `ic_launcher.ico` 圖示。若要添加自訂圖示：

1. **使用 Visual Studio（推薦）**
   - 在方案總管中，右鍵點擊項目 → 屬性
   - 選擇 `應用程式` 標籤
   - 在 `圖示` 下拉選單中選擇 `瀏覽...`
   - 選擇您的 `ic_launcher.ico` 文件

2. **手動添加**
   - 將 `ic_launcher.ico` 複製到項目根目錄
   - 編輯 .csproj 文件，在 `<PropertyGroup>` 中添加：
     ```xml
     <ApplicationIcon>ic_launcher.ico</ApplicationIcon>
     ```

詳細說明請參考：[如何添加圖示.md](如何添加圖示.md)

## 技術實作
- **目標框架**: .NET Framework 4.7.2
- **C# 版本**: 7.3
- **UI 框架**: 自定義 ModernUI
- **打包工具**: Costura.Fody 6.0.0 - 單一 EXE 打包（NEW!）
- **主要功能**:
  - `CheckConnection()`: 檢查網路連線狀態（連接到 1.1.1.1:53）
  - `LoginAsync()`: 執行自動登入流程
  - `ExtractMagicFromUrl()`: 從 URL 提取 magic 參數
  - `ExtractRedirectUrl()`: 從頁面內容提取重新導向 URL
  - `ExtractGatewayIp()`: 從 URL 提取閘道 IP
  - `CheckCaptivePortalTitle()`: 驗證認證頁面標題
  - `EnableAutoStart()` / `DisableAutoStart()`: 管理開機自啟動
  - `EnableStartMinimized()` / `DisableStartMinimized()`: 管理後台啟動
  - `NotifyIcon`: 系統托盤整合
  - 命令列參數處理：支援 `-minimized` 參數

### ModernUI 組件
- **MetroForm**: 主視窗，深色主題
- **MetroPanel**: 3 個面板（帳號密碼、設定、日誌）
- **MetroButton**: 現代化按鈕（開始、停止、清除）
- **MetroTextBox**: 支援 Placeholder 的文字框
- **MetroCheckBox**: Material Design 風格勾選框
- **MetroLabel**: 統一的標籤樣式

### 使用的 NuGet 套件
```
MetroFramework v1.2.0.3
MetroFramework.Design v1.2.0.3
MetroFramework.Fonts v1.2.0.3
MetroFramework.RunTime v1.2.0.3
Fody v6.8.2
Costura.Fody v6.0.0
```

詳細設計說明請參考：
- [MetroFramework整合指南.md](MetroFramework整合指南.md)
- [打包單一EXE指南.md](打包單一EXE指南.md)

## 登錄檔設定
程式使用以下登錄檔項目：
- **位置**: `HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run`
- **鍵值**:
  - `NCUT_Internet_Auto_Login`: 自動啟動程式路徑
  - `NCUT_StartMinimized`: 後台啟動標記（1=啟用，不存在=停用）

## 與 Python 版本的差異
1. **非同步處理**: 使用 C# 的 `async/await` 取代 Python 的同步處理
2. **HTTP 請求**: 使用 `HttpClient` 取代 Python 的 `requests`
3. **ModernUI**: 使用專業的自定義設計風格介面
4. **執行緒安全**: 使用 `Invoke` 確保跨執行緒的 UI 更新安全
5. **系統托盤**: 完整的系統托盤整合，支援背景執行
6. **開機自啟動**: 使用 Windows Registry 實現開機自動啟動
7. **後台啟動**: 支援靜默啟動模式
8. **圖示支援**: 支援自訂應用程式和托盤圖示
9. **命令列參數**: 支援啟動參數控制
10. **深色主題**: 舒適的深色 Metro 主題
11. **穩定的 UI 框架**: 基於成熟的 MetroFramework 函式庫

## 系統需求
- Windows 作業系統
- .NET Framework 4.7.2 或更高版本
- 建議螢幕解析度: 1280x720 或更高
- MetroFramework 套件（已包含）

## 使用情境
### 情境 1：桌面使用
- 不勾選任何自動啟動選項
- 手動開啟程式使用
- 可以隨時查看日誌
- 享受現代化的視覺體驗

### 情境 2：開機自動啟動並顯示視窗
- 只勾選「開機自動啟動」
- 開機時自動開啟程式視窗
- 可以立即看到程式狀態

### 情境 3：後台靜默運行（推薦）
- 勾選「開機自動啟動」和「自啟動時在後台運行」
- 開機時程式直接在後台執行
- 不干擾桌面，需要時從托盤開啟

## 注意事項
- 設定開機自啟動需要修改 Windows 登錄檔，部分防毒軟體可能會提示
- 開機自啟動的登錄檔路徑為：`HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run`
- 程式關閉時會最小化到托盤，若要完全結束請使用托盤圖示右鍵選單的「結束程式」
- 自訂圖示需要 .ico 格式文件，建議包含多個尺寸（16x16, 32x32, 48x48, 256x256）
- 後台啟動模式下，程式會自動最小化到托盤，不會顯示主視窗
- 可以隨時從系統托盤雙擊圖示來顯示主視窗
- 現代化介面需要較新的系統字體支援（Segoe UI）

## 截圖預覽
新的 MetroFramework 介面包含：
- 🌙 深色 Metro 主題
- 📦 三個 Metro 面板（帳號密碼、設定、日誌）
- 🔵 高亮按鈕設計
- 📝 深灰色背景
- ✨ 統一的 Metro 控件風格
- 💫 專業的視覺效果

## 文件說明
- **README.md**: 主要使用說明（本文件）
- **後台啟動功能使用指南.md**: 後台啟動詳細教學
- **如何添加圖示.md**: 圖示設定指南
- **MetroFramework整合指南.md**: MetroFramework 使用說明
- **UI函式庫選擇指南.md**: UI 函式庫比較和選擇建議
- **打包單一EXE指南.md**: 完整的單一 EXE 打包教學（NEW!）
- **快速打包指南.md**: 3 步驟快速打包（NEW!）
- **Costura打包完成總結.md**: 打包配置總結（NEW!）

## 🚀 快速打包（NEW!）

### 3 步驟打包成單一 EXE

1. **切換到 Release 模式**
   ```
   Visual Studio 工具列: Debug ▼ → Release ▼
   ```

2. **建置專案**
   ```
   按 Ctrl+Shift+B
   ```

3. **取得單一 EXE**
   ```
   位置: bin\Release\NCUT-Internet-Auto-Login.exe
   ```

### 使用 PowerShell 腳本（自動化）

```powershell
# 在專案目錄執行
.\Build-Release.ps1
```

這個腳本會自動：
- 清除舊的建置
- 建置 Release 版本
- 驗證輸出文件
- 顯示文件資訊

### 打包說明

- ✅ 所有 DLL 已嵌入到 EXE
- ✅ 無需額外文件
- ✅ 啟用壓縮
- ✅ 可直接分發

詳細說明：[打包單一EXE指南.md](打包單一EXE指南.md)

## 作者
- 原始 Python 版本: sangege & AI LIFE
- C# 版本轉換: GitHub Copilot
- 現代化 UI 設計: GitHub Copilot

## GitHub
https://github.com/apple050620312/NCUT-Internet-Auto-Login

---

## ⭐ 更新日誌

### v2.2 - 單一 EXE 打包（當前版本）
- 🚀 整合 Costura.Fody 打包工具
- 📦 支援單一 EXE 分發
- 🔧 自動嵌入所有依賴 DLL
- 💾 啟用壓縮減小文件大小
- 📝 提供完整打包文件
- 🤖 PowerShell 自動化建置腳本

### v2.1 - MetroFramework 整合
- ✨ 整合 MetroFramework UI 函式庫
- 🎨 使用 Metro 設計風格控件
- 🌙 預設深色主題
- 💫 專業穩定的視覺效果
- 📦 統一的控件樣式
- 🔧 移除自訂繪圖代碼
- ✅ 提高整體穩定性

### v2.0 - 現代化介面重構
- ✨ 全新的 Electron 風格介面設計
- 🎨 自訂標題欄和無邊框視窗
- 📦 卡片式佈局設計
- 💫 現代化控件（按鈕、文字框、勾選框）
- 🎯 統一的主題管理系統
- 📱 響應式佈局設計

### v1.0 - 初始版本
- ✅ Python 轉 C# Windows Forms
- ✅ 系統托盤支援
- ✅ 開機自動啟動
- ✅ 後台啟動模式
