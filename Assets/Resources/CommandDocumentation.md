# VN Framework 命令系統說明文件

## 目錄
1. [通用命令 (CMD_DatabaseExtension_General)](#通用命令)
2. [角色命令 (CMD_DatabaseExtension_Characters)](#角色命令)
3. [圖形面板命令 (CMD_DatabaseExtension_GraphicPanels)](#圖形面板命令)
4. [範例命令 (CMDDatabaseExtensionExamples)](#範例命令)

---

## 通用命令

### Wait - 等待
**用途：** 暫停執行指定的秒數

**語法：**
```
Wait(秒數)
```

**範例：**
```
Wait(2)      // 等待 2 秒
Wait(0.5)    // 等待 0.5 秒
Wait(1.5)    // 等待 1.5 秒
```

---

### ShowUI - 顯示對話系統
**用途：** 顯示整個對話系統 UI

**語法：**
```
ShowUI()
ShowUI(-spd 速度)
ShowUI(-i)
```

**參數：**
- `-spd` / `-speed`：過渡速度（預設：1）
- `-i` / `-immediate`：立即顯示，無動畫

**範例：**
```
ShowUI()           // 以預設速度顯示
ShowUI(-spd 2)     // 2倍速顯示
ShowUI(-i)         // 立即顯示
```

---

### HideUI - 隱藏對話系統
**用途：** 隱藏整個對話系統 UI

**語法：**
```
HideUI()
HideUI(-spd 速度)
HideUI(-i)
```

**參數：**
- `-spd` / `-speed`：過渡速度（預設：1）
- `-i` / `-immediate`：立即隱藏，無動畫

**範例：**
```
HideUI()           // 以預設速度隱藏
HideUI(-spd 3)     // 3倍速隱藏
HideUI(-i)         // 立即隱藏
```

---

### ShowDB - 顯示對話框
**用途：** 只顯示對話框（不包含其他 UI 元素）

**語法：**
```
ShowDB()
ShowDB(-spd 速度)
ShowDB(-i)
```

**參數：**
- `-spd` / `-speed`：過渡速度（預設：1）
- `-i` / `-immediate`：立即顯示

**範例：**
```
ShowDB()           // 顯示對話框
ShowDB(-spd 2)     // 快速顯示
```

---

### HideDB - 隱藏對話框
**用途：** 只隱藏對話框

**語法：**
```
HideDB()
HideDB(-spd 速度)
HideDB(-i)
```

**範例：**
```
HideDB()           // 隱藏對話框
HideDB(-i)         // 立即隱藏
```

---

## 角色命令

### CreateCharacter - 創建角色
**用途：** 在場景中創建角色

**語法：**
```
CreateCharacter(角色名)
CreateCharacter(角色名 -e)
CreateCharacter(角色名 -e -i)
```

**參數：**
- `-e` / `-enabled`：創建後立即啟用顯示
- `-i` / `-immediate`：立即顯示（配合 -e 使用）

**範例：**
```
CreateCharacter(Stella)              // 創建但不顯示
CreateCharacter(Stella -e)           // 創建並淡入顯示
CreateCharacter(Stella -e -i)        // 創建並立即顯示
```

---

### Show - 顯示角色
**用途：** 顯示一個或多個角色

**語法：**
```
Show(角色名1 角色名2 ...)
Show(角色名 -i)
```

**參數：**
- `-i` / `-immediate`：立即顯示，無淡入效果

**範例：**
```
Show(Stella)                  // 淡入顯示 Stella
Show(Stella Alice)            // 同時淡入顯示兩個角色
Show(Stella -i)               // 立即顯示 Stella
```

---

### Hide - 隱藏角色
**用途：** 隱藏一個或多個角色

**語法：**
```
Hide(角色名1 角色名2 ...)
Hide(角色名 -i)
```

**參數：**
- `-i` / `-immediate`：立即隱藏

**範例：**
```
Hide(Stella)                  // 淡出隱藏 Stella
Hide(Stella Alice)            // 同時隱藏兩個角色
Hide(Stella -i)               // 立即隱藏
```

---

### MoveCharacter - 移動角色
**用途：** 將角色移動到指定位置

**語法：**
```
MoveCharacter(角色名 -x X座標 -y Y座標)
MoveCharacter(角色名 -x X座標 -y Y座標 -spd 速度)
MoveCharacter(角色名 -x X座標 -y Y座標 -sm)
```

**參數：**
- `-x`：X 軸座標
- `-y`：Y 軸座標
- `-spd` / `-speed`：移動速度（預設：1）
- `-sm` / `-smooth`：平滑移動
- `-i` / `-immediate`：立即移動到目標位置

**範例：**
```
MoveCharacter(Stella -x 0.5 -y 0)              // 移動到 (0.5, 0)
MoveCharacter(Stella -x -0.3 -y 0.2 -spd 2)    // 快速移動
MoveCharacter(Stella -x 0 -y 0 -sm)            // 平滑移動
MoveCharacter(Stella -x 1 -y 0 -i)             // 立即移動
```

---

### 角色子命令

對於已創建的角色，可以使用點語法調用特定命令：

**語法：**
```
角色名.命令()
```

**可用命令：**

#### 顯示/隱藏
```
Stella.Show()           // 顯示 Stella
Stella.Show(-i)         // 立即顯示
Stella.Hide()           // 隱藏 Stella
Stella.Hide(-i)         // 立即隱藏
```

#### 移動
```
Stella.Move(-x 0.5 -y 0)              // 移動 Stella
Stella.Move(-x 0 -y 0 -spd 2 -sm)     // 平滑快速移動
```

#### 設置精靈圖
```
Stella.SetSprite(表情名)                    // 切換表情
Stella.SetSprite(happy -spd 0.3)          // 快速切換
Stella.SetSprite(sad -l 1)                // 在圖層 1 設置
Stella.SetSprite(angry -i)                // 立即切換
```

#### 高亮
```
Stella.Highlight()           // 高亮 Stella
Stella.Highlight(-i)         // 立即高亮
Stella.Unhighlight()         // 取消高亮
Stella.Unhighlight(-i)       // 立即取消高亮
```

---

## 圖形面板命令

### SetLayerMedia - 設置圖層媒體
**用途：** 在指定面板的圖層上顯示圖片或視頻

**語法：**
```
SetLayerMedia(面板名 媒體名)
SetLayerMedia(媒體名 -p 面板名)
SetLayerMedia(面板名 媒體名 -l 圖層 -spd 速度)
```

**參數：**
- `-p` / `-panel`：面板名稱
- `-l` / `-layer`：圖層編號（預設：0）
- `-m` / `-media`：媒體名稱
- `-spd` / `-speed`：過渡速度（預設：1）
- `-i` / `-immediate`：立即顯示
- `-b` / `-blend`：混合紋理名稱
- `-aud` / `-audio`：使用視頻音頻

**範例：**
```
// 基本用法
SetLayerMedia(background Classroom)

// 指定圖層
SetLayerMedia(background Classroom -l 1)

// 使用參數語法
SetLayerMedia(Classroom -p background -l 0 -spd 2)

// 立即顯示
SetLayerMedia(background Classroom -i)

// 使用混合紋理過渡
SetLayerMedia(background Classroom -b fade_texture)
```

---

### ClearLayerMedia - 清除圖層媒體
**用途：** 清除指定面板的圖層內容

**語法：**
```
ClearLayerMedia(面板名)
ClearLayerMedia(面板名 -l 圖層)
ClearLayerMedia(面板名 -spd 速度)
```

**參數：**
- `-p` / `-panel`：面板名稱
- `-l` / `-layer`：圖層編號（-1 = 全部，預設：-1）
- `-spd` / `-speed`：淡出速度（預設：1）
- `-i` / `-immediate`：立即清除
- `-b` / `-blend`：混合紋理

**範例：**
```
ClearLayerMedia(background)              // 清除所有圖層
ClearLayerMedia(background -l 0)         // 只清除圖層 0
ClearLayerMedia(background -spd 2)       // 快速淡出
ClearLayerMedia(background -i)           // 立即清除
```

---

### SetWorldCanvasMedia - 設置 WorldCanvas 媒體
**用途：** 在 WorldCanvas 上顯示背景圖片（專用於 WorldCanvas）

**語法：**
```
SetWorldCanvasMedia(媒體名)
SetWorldCanvasMedia(媒體名 -l 圖層)
SetWorldCanvasMedia(媒體名 -spd 速度)
```

**參數：**
- `-l` / `-layer`：圖層編號（預設：0）
- `-spd` / `-speed`：過渡速度（預設：1）
- `-i` / `-immediate`：立即顯示
- `-b` / `-blend`：混合紋理

**範例：**
```
SetWorldCanvasMedia(barracks)                    // 顯示 barracks 背景
SetWorldCanvasMedia(Classroom -spd 2)            // 快速切換
SetWorldCanvasMedia(barracks -l 1)               // 在圖層 1 顯示
SetWorldCanvasMedia(Classroom -i)                // 立即顯示
```

---

### ClearWorldCanvasMedia - 清除 WorldCanvas 媒體
**用途：** 清除 WorldCanvas 的背景

**語法：**
```
ClearWorldCanvasMedia()
ClearWorldCanvasMedia(-l 圖層)
ClearWorldCanvasMedia(-spd 速度)
```

**參數：**
- `-l` / `-layer`：圖層編號（-1 = 全部，預設：-1）
- `-spd` / `-speed`：淡出速度（預設：1）
- `-i` / `-immediate`：立即清除

**範例：**
```
ClearWorldCanvasMedia()              // 淡出清除所有圖層
ClearWorldCanvasMedia(-i)            // 立即清除
ClearWorldCanvasMedia(-l 0)          // 只清除圖層 0
ClearWorldCanvasMedia(-spd 2)        // 2倍速淡出
```

---

## 鏡頭效果命令

### CameraLookLeft - 向左看
**用途：** 移動鏡頭向左

**語法：**
```
CameraLookLeft()
CameraLookLeft(-a 距離)
CameraLookLeft(-a 距離 -spd 速度)
```

**參數：**
- `-a` / `-amount`：移動距離（像素，預設：300）
- `-spd` / `-speed`：移動速度（預設：1）
- `-i` / `-immediate`：立即移動

**範例：**
```
CameraLookLeft()                    // 向左移 300 像素
CameraLookLeft(-a 500)              // 向左移 500 像素
CameraLookLeft(-a 400 -spd 2)       // 快速向左移
CameraLookLeft(-a 300 -i)           // 立即向左移
```

---

### CameraLookRight - 向右看
**用途：** 移動鏡頭向右

**語法：**
```
CameraLookRight()
CameraLookRight(-a 距離)
CameraLookRight(-a 距離 -spd 速度)
```

**參數：**
- `-a` / `-amount`：移動距離（像素，預設：300）
- `-spd` / `-speed`：移動速度（預設：1）
- `-i` / `-immediate`：立即移動

**範例：**
```
CameraLookRight()                   // 向右移 300 像素
CameraLookRight(-a 600)             // 向右移 600 像素
CameraLookRight(-a 400 -spd 1.5)    // 1.5倍速向右移
```

---

### CameraZoomIn - 放大
**用途：** 放大鏡頭

**語法：**
```
CameraZoomIn()
CameraZoomIn(-a 倍數)
CameraZoomIn(-a 倍數 -spd 速度)
```

**參數：**
- `-a` / `-amount`：縮放倍數（預設：1.5）
- `-spd` / `-speed`：縮放速度（預設：1）
- `-i` / `-immediate`：立即縮放

**範例：**
```
CameraZoomIn()                      // 放大 1.5 倍
CameraZoomIn(-a 2)                  // 放大 2 倍
CameraZoomIn(-a 1.8 -spd 2)         // 快速放大 1.8 倍
CameraZoomIn(-a 2.5 -i)             // 立即放大 2.5 倍
```

---

### CameraZoomOut - 縮小
**用途：** 縮小鏡頭

**語法：**
```
CameraZoomOut()
CameraZoomOut(-a 倍數)
CameraZoomOut(-a 倍數 -spd 速度)
```

**參數：**
- `-a` / `-amount`：縮放倍數（預設：0.75）
- `-spd` / `-speed`：縮放速度（預設：1）
- `-i` / `-immediate`：立即縮放

**範例：**
```
CameraZoomOut()                     // 縮小至 0.75 倍
CameraZoomOut(-a 0.5)               // 縮小至 0.5 倍
CameraZoomOut(-a 0.6 -spd 2)        // 快速縮小
```

---

### CameraReset - 重置鏡頭
**用途：** 將鏡頭重置到原始位置和縮放

**語法：**
```
CameraReset()
CameraReset(-spd 速度)
CameraReset(-i)
```

**參數：**
- `-spd` / `-speed`：重置速度（預設：1）
- `-i` / `-immediate`：立即重置

**範例：**
```
CameraReset()                       // 平滑重置
CameraReset(-spd 2)                 // 快速重置
CameraReset(-i)                     // 立即重置
```

---

### CameraSetPosition - 設置鏡頭位置
**用途：** 精確設置鏡頭的位置和縮放

**語法：**
```
CameraSetPosition(-x X座標 -y Y座標)
CameraSetPosition(-x X座標 -y Y座標 -z 縮放)
CameraSetPosition(-x X座標 -y Y座標 -z 縮放 -spd 速度)
```

**參數：**
- `-x`：X 軸位置
- `-y`：Y 軸位置
- `-z` / `-zoom`：縮放倍數（預設：1）
- `-spd` / `-speed`：移動速度（預設：1）
- `-i` / `-immediate`：立即設置

**範例：**
```
CameraSetPosition(-x 200 -y 100)                    // 移動到 (200, 100)
CameraSetPosition(-x 300 -y 50 -z 1.5)              // 移動並放大
CameraSetPosition(-x -200 -y -100 -z 2 -spd 2)      // 快速移動並放大
CameraSetPosition(-x 0 -y 0 -z 1 -i)                // 立即重置
```

---

### CameraSetBackgroundScale - 設置背景縮放
**用途：** 設置背景的安全縮放比例（避免黑邊）

**語法：**
```
CameraSetBackgroundScale(-a 比例)
```

**參數：**
- `-a` / `-amount`：縮放比例（預設：1.3）

**範例：**
```
CameraSetBackgroundScale(-a 1.5)    // 設置為 1.5 倍（更大移動範圍）
CameraSetBackgroundScale(-a 1.2)    // 設置為 1.2 倍（更清晰）
CameraSetBackgroundScale(-a 1.0)    // 不縮放（可能露出黑邊）
```

**建議值：**
- 移動距離 ±200px：1.2 倍
- 移動距離 ±300px：1.3 倍（預設）
- 移動距離 ±400px：1.4 倍
- 移動距離 ±500px：1.5 倍

---

## 範例命令

### Print - 打印訊息
**用途：** 在 Console 中打印訊息（用於測試）

**語法：**
```
Print()                              // 打印預設訊息
Print_LP(訊息)                       // 打印單行訊息
Print_MP(訊息1 訊息2 ...)            // 打印多行訊息
```

**範例：**
```
Print()
Print_LP(Hello World)
Print_MP(Line1 Line2 Line3)
```

---

### Lambda - Lambda 表達式範例
**用途：** 展示 Lambda 表達式的使用（用於測試）

**語法：**
```
Lambda()
Lambda_LP(參數)
Lambda_MP(參數1 參數2 ...)
```

---

### Process - 處理程序範例
**用途：** 展示協程的使用（用於測試）

**語法：**
```
Process()                    // 執行 5 次，每次等待 1 秒
Process_LP(次數)            // 執行指定次數
Process_MP(訊息1 訊息2 ...) // 執行多次，每次顯示訊息
```

**範例：**
```
Process()                    // 運行 5 次
Process_LP(3)               // 運行 3 次
Process_MP(Task1 Task2)     // 執行兩個任務
```

---

### MoveCharDemo - 移動角色示範
**用途：** 展示角色移動（需要場景中有名為 "Image" 的物件）

**語法：**
```
MoveCharDemo(left)
MoveCharDemo(right)
```

---

## 完整劇本範例

### 範例 1：基本對話場景
```
// 設置背景
SetWorldCanvasMedia(classroom)
wait(0.5)

// 顯示 UI
ShowUI()
wait(0.3)

// 創建並顯示角色
CreateCharacter(Stella -e)
wait(1)

// 對話...
```

### 範例 2：角色移動與表情
```
// 角色進場
CreateCharacter(Stella)
Stella.Move(-x -0.5 -y 0 -i)
Stella.Show()
wait(1)

// 切換表情
Stella.SetSprite(happy -spd 0.3)
wait(0.5)

// 移動到中間
Stella.Move(-x 0 -y 0 -spd 1 -sm)
wait(1)

// 高亮角色
Stella.Highlight()
```

### 範例 3：鏡頭效果
```
// 設置背景
SetWorldCanvasMedia(room)
wait(1)

// 環顧房間
CameraLookLeft(-a 400 -spd 0.8)
wait(2)

CameraLookRight(-a 800 -spd 1.2)
wait(2)

// 發現重要物品
CameraSetPosition(-x 300 -y 100 -z 1.5 -spd 1)
wait(2)

// 放大特寫
CameraZoomIn(-a 1.5 -spd 2)
wait(2)

// 重置
CameraReset(-spd 1.5)
```

### 範例 4：場景切換
```
// 第一個場景
SetWorldCanvasMedia(morning_classroom)
CreateCharacter(Stella -e)
wait(3)

// 淡出
Hide(Stella)
ClearWorldCanvasMedia()
wait(1)

// 第二個場景
SetWorldCanvasMedia(evening_school)
CreateCharacter(Alice -e)
wait(3)
```

---

## 通用參數說明

### 速度參數 (-spd / -speed)
- 數值越大，動作越快
- 預設值通常為 1
- 常用值：0.5（慢）、1（正常）、2（快）、3（很快）

### 立即執行 (-i / -immediate)
- 跳過動畫，立即完成動作
- 適用於需要快速切換的場景

### 圖層參數 (-l / -layer)
- 數值越大，顯示在越上層
- 預設為 0
- 可用於疊加多個圖片

---

## 注意事項

1. **命令語法**
   - 每個命令獨立一行
   - 不要用逗號連接命令
   - 參數用空格或 `-參數名` 格式

2. **資源路徑**
   - 所有媒體資源需放在 Resources 資料夾中
   - 使用相對路徑或資源名稱

3. **等待時間**
   - 適當使用 `Wait()` 讓場景更自然
   - 避免動作太快導致玩家跟不上

4. **效能考慮**
   - 清除不再使用的圖層
   - 避免同時顯示過多圖形

5. **錯誤處理**
   - 如果資源找不到，會在 Console 顯示錯誤
   - 檢查資源名稱和路徑是否正確

---

## 版本資訊
- 版本：1.0
- 最後更新：2025
- 框架：Unity VN Framework


