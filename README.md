# RimTalk QuietQwen

[English](README_EN.md) | **中文**

一个 RimWorld 模组，用于禁用 [RimTalk](https://steamcommunity.com/sharedfiles/filedetails/?id=3365145210) 中阿里云千问（Qwen）模型的思考功能。

## 功能特性

- 自动检测 RimTalk 发往阿里云千问（`aliyuncs.com`）的请求
- 在请求体中注入 `"enable_thinking": false` 以关闭思考模式
- 防重复注入：若请求体已包含该参数则跳过
- 提供开关，可随时启用/禁用
- 无需配置，开箱即用
- 默认启用

## 工作原理

通过 Harmony 前缀补丁拦截 `UnityWebRequest.SendWebRequest()`，对目标为 `aliyuncs.com` 域名的 POST 请求，在发送前将 `"enable_thinking": false` 注入到 JSON 请求体的顶层。

## 前置依赖

- RimWorld 1.5+
- [RimTalk](https://steamcommunity.com/sharedfiles/filedetails/?id=3365145210)（`cj.rimtalk`）
- Harmony（RimWorld 内置）

## 安装方法

1. 在 Steam 创意工坊订阅此模组，或将文件夹复制到 `RimWorld/Mods/`
2. 确保 **RimTalk** 在此模组之前加载
3. 启动游戏 — 模组默认启用

## 构建

```bash
cd Source
dotnet build
```

输出：`1.6/Assemblies/RimTalkNoThinking.dll`

## 许可证

MIT
