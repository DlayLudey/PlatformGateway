# PlatformGateway

## Основной класс

Хранит в себе все модули и отвечает за их инициализацию

### Использование

Перед началом использования необходимо добавить **PlatformGateway** на любой пустой объект в самой первой сцене игры

Так как **PlatformGateway** это `Singleton` с `DontDestroyOnLoad`, все привязаные к его `GameObject` `MonoBehaviour'ы` останутся с ним между всеми сценами

После добавления **PlatformGateway** в проекте нужно создать ScriptableObject необходимых платформ и добавить их в `platforms` поле у PlatformGateway

Перед началом игры необходимо вызвать метод `PlatformGateway.Init()` и дождаться инициализации, после чего все методы будут доступны в соответсующих модулях

### Модули

- [Platform](Platform.md)
- [Storage](Storage.md)
- [Advertisement](Advertisement.md)
- [Payments](Payments.md)
- [Account](Account.md)
- [Social](Social.md)

### Поля

```csharp
List<PlatformBase> platforms; // Платформы доступные для данной игры
// Необходимо прокинуть их в SerializeField у PlatformGateway
```

```csharp
// Соответствующие модули
AdvertisementBase Advertisement;
PaymentsBase Payments;
StorageBase Storage;
ISocial Social;
IAccount Account;
PlatformBase CurrentPlatform; 
```

```csharp
PlatformType PlatformType; // Тип текущей платформы
```

### Основные методы

**Init()**

Инициализирует **PlatformGateway** и все его модули 

Важно, это IEnumerator который нужно дождаться перед дальнейшей работой с PlatformGateway