# Storage

## Отвечает за покупки

Процесс покупки отличается для разных платформ, в зависимости от `Payments.ConsummationSupported` нужно проводить процесс консумации

Консумация - это способ удостоверится что игрок получил покупку, если покупку можно "потребить" (к примеру монеты или жизни которые можно потратить) то на платформах с консумацией необходимо отправить запрос на потребление (консумацию) данного товара

Пример алгоритма покупки с консумацией
- Игрок отправил запрос на покупку товара
- Покупка прошла и в игру вернулся коллбек что можно выдавать товар
- В случае успешного подключения игрока и отсутсвия ошибок происходит отправляется запрос на консумацию товара и по коллбеку выдается награда
- В случае каких либо ошибок или перезапуска страницы в неподходящий момент, перед началом игры проходит проверка всех не консумированных товаров, их консумация и выдача награды потерянных товаров

### Внутренние структуры данных

---

```csharp
// Struct с общими данными о продукте
public struct Product
{
    public string productId; // ID продукта по которому происходит покупка
    public string name; // Название продукта, используется только у некоторых платформ
    public string description; // Описание продукта, используется только у некоторых платформ
    public int price; // Цена продукта
}
```

```csharp
// Struct с информацией для консумации купленного продукта
public struct PurchasedProduct
{
    public string productId; // ID продукта по которому происходит консумация, такой же ID что и в Product
    public string consummationToken; // Токен по которому происходит консумация, уникален у каждой отдельной покупки совершенной игроком
}
```


### Поля

---

```csharp
Product[] Products; // Список товаров на платформе
```

```csharp
string CurrencyName; // Название валюты на текущей платформе

Sprite CurrencySprite; // Иконка валюты на текущей платформе
```

```csharp
bool PaymentsSupported; // Поддерживаются ли на текущей платформе покупки
bool ConsummationSupported; // Поддерживается ли на текущей платформе консумация
```

### Основные методы

---

**Purchase(string productId, bool consumable, Action onSuccessCallback = null, Action<string> onErrorCallback = null)**

Производит покупку товара по `productId`\

В `consumable` указывается то, нужно ли эту покупку законсумировать, консумация произойдет автоматически в процессе покупки внутри **PlatformGateway**

Пример использования:
```csharp
Payments.Purchase("coins", true, () => 
{
    Debug.Log("Successfully bought coins!");
}, error => 
{
    Debug.LogError($"Purchase failed, error: {error}");
})
```

---

**GetPurchases(Action<PurchasedProduct[]> onSuccessCallback, Action<string> onErrorCallback = null)**

Получает список всех необработанных покупок которые нужно сконсумировать

Пример использования:
```csharp
Payments.GetPurchases(products => 
{
    foreach(var product in products)
        Debug.Log($"ProductId: {product.productId} \n Token: {product.consummationToken}");
}, error => 
{
    Debug.LogError($"Failed to get purchases, error: {error}");
})
```
---

**ConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null)**

Консумирует покупку на стороне платформы по `productToken` получаемому из `PurchasedProduct`

Пример использования:
```csharp
Payments.ConsumePurchase(product.consumationToken, () => 
{
    Debug.Log($"Consumed product with id: {product.productId} and token {product.consummationToken}");
}, error => 
{
    Debug.LogError($"Consumation failed, error: {error}");
})
```
