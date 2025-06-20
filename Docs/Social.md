# Advertisement

## Отвечает за социальные взаимодействия

По большей части актуально только для платформ Mail.Ru

### Поля

```csharp
// Поддерживается ли указанный метод на текущей платформе
bool isAddToFavoritesSupported;
bool isAddToHomeScreenSupported;
bool isCreatePostSupported;
bool isExternalLinksAllowed;
bool isInviteFriendsSupported;
bool isJoinCommunitySupported;
bool isRateSupported;
bool isShareSupported;
```

### Основные методы

**AddToFavorites(Action<bool> onComplete = null)**

Предлагает игроку добавить игру в избранное

---

**AddToHomeScreen(Action<bool> onComplete = null)**

Предлагает игроку добавить игру на рабочий стол/главный экран

---

**CreatePost(Dictionary<string, object> options, Action<bool> onComplete = null)**

Предлагает игроку создать пост от его имени

---

**InviteFriends(string inviteText, Action<bool> onComplete = null, Action<string> onError = null)**

Предлагает игроку пригласить друзей в игру с текстом `inviteText`

---

**JoinCommunity(Dictionary<string, object> options, Action<bool> onComplete = null)**

Предлагает игроку вступить в группу

---

**Rate(Action<bool> onComplete = null)**

Предлагает игроку поставить оценку игре

---

**Share(Dictionary<string, object> options, Action<bool> onComplete = null)**

Предлагает игроку поделится игрой
