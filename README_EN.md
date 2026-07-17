# RimTalk QuietQwen

**English** | [中文](README.md)

A RimWorld mod that disables thinking mode for Aliyun Qwen models in [RimTalk](https://steamcommunity.com/sharedfiles/filedetails/?id=3365145210).

## Features

- Automatically detects RimTalk requests to Aliyun Qwen (`aliyuncs.com`)
- Injects `"enable_thinking": false` into the request body to disable thinking mode
- Duplicate injection prevention: skips if the parameter is already present
- Simple toggle to enable/disable the functionality
- No configuration required — works out of the box
- Default enabled

## How It Works

Uses a Harmony prefix patch on `UnityWebRequest.SendWebRequest()` to intercept POST requests targeting `aliyuncs.com` domains. Before the request is sent, it injects `"enable_thinking": false` at the top level of the JSON request body.

## Requirements

- RimWorld 1.5+
- [RimTalk](https://steamcommunity.com/sharedfiles/filedetails/?id=3365145210) (`cj.rimtalk`)
- Harmony (bundled with RimWorld)

## Installation

1. Subscribe to this mod on Steam Workshop, or copy the folder to `RimWorld/Mods/`
2. Ensure **RimTalk** is loaded before this mod
3. Launch the game — the mod is enabled by default

## Build

```bash
cd Source
dotnet build
```

Output: `1.6/Assemblies/RimTalkNoThinking.dll`

## License

MIT
