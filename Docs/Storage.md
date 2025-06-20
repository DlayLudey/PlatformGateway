# Storage

## Отвечает за хранение игровых данных

Данные предварительно хранятся на стороне игрока в виде словаря и переодически отправляются на сервер в виде JSON строки с интервалом указанным в ScriptableObject соответсвующей платформы
### Основные методы

---

**Storage.SetValue\<T\>(string key, T value)**

Сохранение переменной с типом `T` по ключу `key`

Пример использования:
```csharp
// Сохранит переменную по ключом PlayerScore
Storage.SetValue("PlayerScore", 1000)
```
---

**Storage.GetValue\<T\>(string key, T defaultValue)**

Получение переменной с типом `T` по ключу `key`, в случае отсутствия переменной с таким ключом возвращается `defaultValue`

Пример использования:
```csharp
// Вернет переменную с ключом PlayerScore
// В случае если в переменной до этого ничего не хранилось вернет 0
Storage.GetValue("PlayerScore", 0)
```


### Вторичные методы

---

**Storage.SaveStoredData(Action onComplete = null, Action<string> onError = null)**

Отправляет все сохраненные в словаре данные на сервер платформы в виде JSON строки

**Этот метод не обязательно вызывать вручную, PlatformGateway сам сохраняет данные с интервалом указанным в ScriptableObject соответсвующей платформы**

Пример использования:
```csharp
Storage.SaveStoredData(() => 
{
    Debug.Log("Data Saved!");
}, error => 
{
    Debug.LogError($"Data save failed, error: {error}");
})
```

---

**Storage.LoadData(string key, Action<string> successCallback, Action<string> errorCallback = null)**

Напрямую загружает сохраненную строку данных

Пример использования:
```csharp
Storage.LoadData("Data", data => 
{
    Debug.Log(data);
}, error => 
{
    Debug.LogError($"Data load failed, error: {error}");
})
```

---

**Storage.SaveData(string key, string value, Action successCallback = null, Action<string> errorCallback = null)**

Напрямую сохраняет строку данных на сервер платформы

**Этот метод полностью перепишет все данные сохраненные до этого через PlatformGateway и может сломать сохранения**

Пример использования:
```csharp
Storage.SaveData("Data", "Some usefull data", () => 
{
    Debug.Log("Data saved!");
}, error => 
{
    Debug.LogError($"Data save failed, error: {error}");
})
```
