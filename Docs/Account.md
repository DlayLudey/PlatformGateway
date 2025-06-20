# Account

## Отвечает за данные аккаунта игрока

### Внутренние структуры данных

```csharp
public class PlayerData
{
    public string name; // Имя/ник игрока
    public string id; // Id профиля игрока на платформе
    public string profilePictureUrl; // URL аватарки игрока
}
```

### Поля

```csharp
PlayerData Player; // Данные об игроке
```

### Вторичные методы

**GetPlayerData()**

Получает данные игрока

Важно, получение данных происходит автоматически при инициализации **PlatformGateway**
