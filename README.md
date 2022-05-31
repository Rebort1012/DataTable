# Excel导出json工具

## 功能

![image-20220531144853430](https://gitee.com/small-perilla/pic-go/raw/master/image-20220531144853430.png)

- 支持int、float、bool、string基础类型
- 支持数组
- 支持kv
- 支持枚举
- 支持unity类型vector3,vector2,color
- 自动生成csharp类
- 单个excel中多个sheet，依次导出

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

## 配表

- 第一行注释

- 第二行字段类型

- 第三行变量名（属性名）

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

LitJson库魔改；

将example/Csharp/中

JsonExtension.cs

UnityTypeBridge.cs

导入工程，对自定义类型(Vector,color)注册；



个人博客：www.perilla.work
