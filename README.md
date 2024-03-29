# Excel导表工具

## 功能


![image-20220531144853430](https://picsheet.oss-cn-hangzhou.aliyuncs.com/2522637-20220627234401309-1082970503.png)

- 支持int、float、bool、string基础类型
- 支持数组
- 支持kv;（key为数字，建议用list+新表）
- 支持枚举
- 支持unity类型vector3,vector2,color
- 自动生成csharp类
- 单个excel中多个sheet，依次导出 **第一行第一列格子中#{类名} = 生成C#类的class名**
- 单个sheet,文件名为类名
- 类型为空的列跳过
- 第一列标#开始的行跳过

## 使用

1. 设置config.txt文件，按需求配置； 

#为注释行必须；结尾

```
#excel存放路径;
excelPath:./Excel/;
#数据保存路径;
dataPath:./DataTable/;
#c#类保存路径;
classPath:./CSharp/;
#输出类型;
exportType:Json;
isExportServer:False
```

2. 双击运行DataTable.exe，等待执行完毕；
2. 第二行不能有空行；
2. 枚举单独表Enum.xlsx，其他表中的枚举值必须在这个表中存在，加载时会先生成enum.cs;

## 配表

- 第一行注释

- 第二行变量名（属性名）

- 第三行字段类型

- 第一列留空

- 数组：类型+[] e.g: int[]

- kv使用 

  类型：dic<string,int>

  变量名：变量名+:+key值

  e.g:

  | dic<string,float> | dic<string,float> | dic<string,float> |
  | ----------------- | ----------------- | ----------------- |
  | Attribute:atk     | Attribute:def     | Attribute:spd     |

- 枚举：自动生成的枚举类型从1开始，Enum类型为：Enum+变量名字段；

## 序列化

### json

LitJson库魔改；

将example/Csharp/中

JsonExtension.cs

UnityTypeBridge.cs

导入工程，对自定义类型(Vector,color)注册；



### bytes

bytes不支持dic；（好像没有必要，可以创建新表链接过去）

bytes存储结构：

![image-20220607180017443](https://picsheet.oss-cn-hangzhou.aliyuncs.com/image-20220607180017443.png)

生成的C#类自带parse方法；

csharp/tools文件DataMgr.cs中，DataLoader负责加载bytes数据；

支持索引加载单条数据；



个人博客：www.perilla.asia
