# Sensitive Personal Information Notice

�o�O����ӷP�ӤH��T�q���A�ثe����@ CakeResume ���i�����R�ñN�ӷP���Ӹ�q���� User Email�C

## Getting Started
�o�ӱM�׬O�ϥ� .NET Core Console�B.NET Standard 2.0�BEF Core 2.1 Preview 1�BDapper �}�o�A�åB�]�t²�檺�T�h���[�c�B Dependency Injection�BRepository Pattern�A�j�h���O�m�ߩʽ�A��M���γo��·Ф@�˯వ��i�����Ӹ괣���q���\��C

### ����
��ĳ�ϥ� Visual Studio 2017 �� Visual Studio Code(�ݦw��C#�X�R�M��)�A�Ӷ}�Ҧ��M�סA�Y�ϥ� VS 2017 �}�ұM�קY�i�ظm�C

### �]�w
�b appsettings.json ���ݭn���]�w Mail �q�����]�w�A�d��:

```json
	"SecurityDetectMailConfig": {
		"Host": "smtp.sendgrid.net",
		"Port": 587,
		"User": "apikey",
		"Password": "Type Your Password",
		"FromAddress": "personal.information.detect@sendgrid.com",
		"FromDisplayName": "CakeResume �Ӹ��˴�(Personal Information Detect)",
		"Subject": "{0} �A�� CakeResume �i����e�]�����}�A�нT�{�O�_���Ӹ�ü{(Your CakeResume resume is currently made public. Please confirm if you have any security concerns)",
		"Body": "",
		"BodyPath": "Templates\\SecurityDetectTemplate.html"
	}
```

Host �A�i�H�����`�Ϊ��A�ҦpGmail(smtp.gmail.com)�A�b�ק� User �P Password �Y�i�C