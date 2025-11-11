# VN Framework 命令快速參考

## 🎮 通用命令

| 命令 | 功能 | 範例 |
|------|------|------|
| `Wait(秒數)` | 等待 | `Wait(2)` |
| `ShowUI()` | 顯示 UI | `ShowUI(-spd 2)` |
| `HideUI()` | 隱藏 UI | `HideUI(-i)` |
| `ShowDB()` | 顯示對話框 | `ShowDB()` |
| `HideDB()` | 隱藏對話框 | `HideDB()` |

---

## 👤 角色命令

### 基本操作
| 命令 | 功能 | 範例 |
|------|------|------|
| `CreateCharacter(名稱)` | 創建角色 | `CreateCharacter(Stella -e)` |
| `Show(名稱)` | 顯示角色 | `Show(Stella Alice)` |
| `Hide(名稱)` | 隱藏角色 | `Hide(Stella -i)` |
| `MoveCharacter(名稱 -x X -y Y)` | 移動角色 | `MoveCharacter(Stella -x 0.5 -y 0)` |

### 角色子命令
| 命令 | 功能 | 範例 |
|------|------|------|
| `角色.Show()` | 顯示 | `Stella.Show(-i)` |
| `角色.Hide()` | 隱藏 | `Stella.Hide()` |
| `角色.Move()` | 移動 | `Stella.Move(-x 0 -y 0 -sm)` |
| `角色.SetSprite()` | 換表情 | `Stella.SetSprite(happy)` |
| `角色.Highlight()` | 高亮 | `Stella.Highlight()` |
| `角色.Unhighlight()` | 取消高亮 | `Stella.Unhighlight()` |

---

## 🖼️ 背景/圖層命令

### WorldCanvas（推薦用於背景）
| 命令 | 功能 | 範例 |
|------|------|------|
| `SetWorldCanvasMedia(名稱)` | 設置背景 | `SetWorldCanvasMedia(classroom)` |
| `ClearWorldCanvasMedia()` | 清除背景 | `ClearWorldCanvasMedia(-i)` |

### 通用圖層
| 命令 | 功能 | 範例 |
|------|------|------|
| `SetLayerMedia(面板 媒體)` | 設置圖層 | `SetLayerMedia(background room)` |
| `ClearLayerMedia(面板)` | 清除圖層 | `ClearLayerMedia(background)` |

---

## 🎬 鏡頭效果

| 命令 | 功能 | 範例 |
|------|------|------|
| `CameraLookLeft()` | 向左看 | `CameraLookLeft(-a 400)` |
| `CameraLookRight()` | 向右看 | `CameraLookRight(-a 500)` |
| `CameraZoomIn()` | 放大 | `CameraZoomIn(-a 2)` |
| `CameraZoomOut()` | 縮小 | `CameraZoomOut(-a 0.5)` |
| `CameraReset()` | 重置 | `CameraReset(-spd 2)` |
| `CameraSetPosition()` | 精確設置 | `CameraSetPosition(-x 200 -y 100 -z 1.5)` |
| `CameraSetBackgroundScale()` | 設置背景縮放 | `CameraSetBackgroundScale(-a 1.5)` |

---

## 🔧 常用參數

| 參數 | 完整名稱 | 說明 | 預設值 |
|------|---------|------|--------|
| `-spd` | `-speed` | 速度 | 1 |
| `-i` | `-immediate` | 立即執行 | false |
| `-l` | `-layer` | 圖層編號 | 0 |
| `-a` | `-amount` | 數量/距離 | 依命令而異 |
| `-x` | - | X 座標 | 0 |
| `-y` | - | Y 座標 | 0 |
| `-z` | `-zoom` | 縮放 | 1 |
| `-sm` | `-smooth` | 平滑移動 | false |
| `-e` | `-enabled` | 啟用 | false |
| `-p` | `-panel` | 面板名稱 | - |
| `-m` | `-media` | 媒體名稱 | - |
| `-b` | `-blend` | 混合紋理 | - |

---

## 📝 常見模式

### 場景開始
```
SetWorldCanvasMedia(background)
ShowUI()
CreateCharacter(Stella -e)
wait(1)
```

### 角色對話切換
```
Stella.Highlight()
// 對話...
Stella.Unhighlight()

Alice.Highlight()
// 對話...
Alice.Unhighlight()
```

### 場景切換
```
// 淡出
Hide(Stella Alice)
ClearWorldCanvasMedia()
wait(1)

// 淡入
SetWorldCanvasMedia(new_background)
Show(Stella)
wait(1)
```

### 鏡頭探索
```
CameraLookLeft(-a 400)
wait(2)
CameraLookRight(-a 600)
wait(2)
CameraReset()
```

### 戲劇性特寫
```
CameraZoomIn(-a 1.8 -spd 2)
wait(2)
CameraReset()
```

---

## ⚡ 速度值參考

| 速度 | 說明 | 適用場景 |
|------|------|----------|
| 0.3-0.5 | 很慢 | 強調、戲劇性 |
| 0.8-1 | 正常 | 一般對話 |
| 1.5-2 | 快 | 快節奏場景 |
| 2-3 | 很快 | 動作場景 |

---

## 🎯 座標參考

### 角色位置（X 軸）
- `-1.0` - 畫面最左邊
- `-0.5` - 左側
- `0` - 中間
- `0.5` - 右側
- `1.0` - 畫面最右邊

### 鏡頭移動距離
- `200-300` - 小範圍移動
- `400-500` - 中範圍移動
- `600-800` - 大範圍移動

### 縮放值
- `0.5` - 縮小一半
- `1.0` - 原始大小
- `1.5` - 放大 1.5 倍
- `2.0` - 放大 2 倍

---

## ❌ 常見錯誤

### ❌ 錯誤
```
SetWorldCanvasMedia(bg1),SetWorldCanvasMedia(bg2)  // 用逗號連接
Show(Stella Alice Bob)                              // 太多參數
SetLayerMedia(image)                                // 缺少面板名稱
```

### ✅ 正確
```
SetWorldCanvasMedia(bg1)
SetWorldCanvasMedia(bg2)

Show(Stella Alice)
Show(Bob)

SetLayerMedia(background image)
```

---

## 📚 完整文檔

詳細說明請參閱：`CommandDocumentation.md`

