<a id="preparing-a-model"></a>
# 制定模型

如何获得生成 `.glb` (Meshy, Hi3D,混合器)到一个聚合物,结构大小和现实世界大小的游戏,在它进入 `<Mod>/Structures/` (重建)或 `<Mod>/ItemModels/` (原文 ) 进口本身: [structures.md](structures.md).

命令从PowerShell中的repo根运行。 需要 Python 3; `prepare-model.py` 也需要 `pip install pillow numpy`.

<a id="1-inspect"></a>
## 1. 检查

```powershell
python tools\model-info.py <Mod>\Structures\my-hut.glb
```

它以尺寸(X,Y上,Z),最长的侧面,三角形,材料,每个结构的像素大小和MB。 关于节点规模或旋转的标志 `prepare-model.py` 不能放置:从 Blender 重新出口,使用转换(对象 > 应用 > 所有转换)。

<a id="2-cut-the-polycount"></a>
## 2. 切断聚合物

| 善良 | 三角形 | 文学 |
| --- | --- | --- |
| 建造或装饰 | 五千到二十万 | 2048 |
| 物品(持有,落下,在商店) | 低于5000 | 1024 |

发电机通常提供100,000或更多;进口警告超过150,000。 要切断:

- **Meshy:** 把模型放在预算中(四角形或三角形都工作),保持结构,
下载如 GLB 又一次。
- **混合器:** 文件 > 进口 glTF,选择按钮,添加一个恶性变量器(Collapse)和降低比例,直到面部在状态中计算。
字符串在预算中,应用它,然后文件 > 出口 > glTF 二元(`.glb`( ) 使用 适用 变量。

☆  (  )`collisionTriangles`,默认 2000),所以切割可见的面板是关于框架速度和文件大小。

<a id="3-choose-the-size"></a>
## 3. 选择尺寸

进口将模型视为尺寸;发电机的尺寸是自愿的。 請將實際尺寸列出並將其安置在模型的設定檔案中([主条目.md “Settings”](structures.md#settings)但是,不是在模型中:

- 建造: `"height"` 在米中 `<model>.json`. Kenshi人们大约有1.8米的高度,除了现实世界大小之外,就读小了。
尺寸你会描述,并添加20%(2.2m的帖子变成 `2.64`).
- 物品: `"size"`其最长的侧面,在 `<model>.json` 其次,在 `ItemModels/`.

<a id="4-shrink-the-textures"></a>
## 4. 把文本的

```powershell
python tools\prepare-model.py <Mod>\Structures\my-hut.glb 2048
```

此分類上一篇: 每個字符串的重編(建築物的2048字符串),通常從數字切割到幾分鐘 一个可选的第三个论点也将模型转移到最长的侧面,因为一个设置文件是不需要的。 而且,它还会增加一半的结构(`textureSize`但是,完成破碎的文件保持了复制小。

跑步 `model-info.py` 再次检查结果。

<a id="5-import-and-check"></a>
## 5. 进口和检查

要么要把它放在自己身上,要么把它放在自己的身上。 [首頁 〉外文書 〉文學 〉西洋學 〉Quick Start](structures.md#quick-start)). 路由器每行一行:

```
Structure my-hut.glb -> "My Hut": 2 part(s), 12480 triangles, 3.1 m tall, collision mesh, unlocked by start
Item beast-capsule.glb -> mesh mymod_beast_capsule.mesh, material 950000-MyMod.mod, 4082 triangles, 0.11 m long
```

检查高度或长度,然后看看它在游戏中:放置建筑物,或下载并拿起项目。

<a id="6-commit"></a>
## 6. 承诺

承诺的 `.glb`其它 `.json` 和文件夹的 `ids.json` 一起。 `ids.json` 保持每个模型的记录ID固定,并保存这些ID所放置的建筑物的名称。
