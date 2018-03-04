# Sensitive Personal Information Notice

這是關於敏感個人資訊通知，目前有實作 CakeResume 的履歷分析並將敏感的個資通知該 User Email。

## Getting Started
這個專案是使用 .NET Core Console、.NET Standard 2.0、EF Core 2.1 Preview 1、Dapper 開發，並且包含簡單的三層式架構、 Dependency Injection、Repository Pattern，大多都是練習性質，當然不用這麼麻煩一樣能做到履歷的個資提醒通知功能。

### 環境
建議使用 Visual Studio 2017 或 Visual Studio Code(需安裝C#擴充套件)，來開啟此專案，若使用 VS 2017 開啟專案即可建置。

### 設定
在 appsettings.json 中需要先設定 Mail 通知的設定，範例:

```json
	"SecurityDetectMailConfig": {
		"Host": "smtp.sendgrid.net",
		"Port": 587,
		"User": "apikey",
		"Password": "Type Your Password",
		"FromAddress": "personal.information.detect@sendgrid.com",
		"FromDisplayName": "CakeResume 個資檢測(Personal Information Detect)",
		"Subject": "{0} 你的 CakeResume 履歷當前設為公開，請確認是否有個資疑慮(Your CakeResume resume is currently made public. Please confirm if you have any security concerns)",
		"Body": "",
		"BodyPath": "Templates\\SecurityDetectTemplate.html"
	}
```

Host 你可以換成常用的，例如Gmail(smtp.gmail.com)，在修改 User 與 Password 即可。