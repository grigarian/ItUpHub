# SEO Оптимизация в ItUpHub

## Быстрый старт

### 1. Импорт хука

```typescript
import { usePageSEO } from '../utils/hooks/useSEO';
```

### 2. Использование в компоненте

```typescript
export default function MyPage() {
  // SEO оптимизация
  usePageSEO.home(); // для главной страницы
  // или
  usePageSEO.projects(); // для страницы проектов
  // или
  usePageSEO.project(projectTitle, projectDescription); // для страницы проекта
  
  return (
    <div>
      {/* Содержимое страницы */}
    </div>
  );
}
```

## Доступные SEO конфигурации

### Главная страница
```typescript
usePageSEO.home();
```
- **Title:** `ItUpHub - IT Сообщество | Проекты, команды, развитие` (58 символов)
- **Description:** `ItUpHub - платформа для IT сообщества. Создавайте проекты, находите команду, развивайте навыки и участвуйте в инновационных разработках.` (158 символов)
- **Keywords:** `IT, проекты, разработка, команда, программирование, технологии, сообщество, стартап, веб-разработка, мобильная разработка`

### Страница проектов
```typescript
usePageSEO.projects();
```
- **Title:** `Проекты | ItUpHub - Каталог IT проектов` (47 символов)
- **Description:** `Найдите интересные IT проекты для участия. Веб-разработка, мобильные приложения, искусственный интеллект и многое другое.` (142 символа)
- **Keywords:** `IT проекты, веб-разработка, мобильные приложения, AI, машинное обучение, программирование, команда разработчиков`

### Страница проекта
```typescript
usePageSEO.project(projectTitle, projectDescription);
```
- **Title:** `{Название проекта} | ItUpHub`
- **Description:** Описание проекта (обрезается до 200 символов)
- **Keywords:** `IT проект, {название проекта}, разработка, команда, программирование, технологии`

### Страница профиля
```typescript
usePageSEO.profile(userName);
```
- **Title:** `Профиль {Имя пользователя} | ItUpHub`
- **Description:** `Профиль разработчика {Имя пользователя} на платформе ItUpHub. Портфолио проектов, навыки и опыт.`
- **Keywords:** `профиль разработчика, {Имя пользователя}, портфолио, навыки программирования, IT специалист`

### Страница входа
```typescript
usePageSEO.login();
```
- **Title:** `Вход | ItUpHub - Авторизация` (35 символов)
- **Description:** `Войдите в свой аккаунт ItUpHub для доступа к проектам, командам и возможностям развития.` (108 символов)
- **Keywords:** `вход, авторизация, ItUpHub, аккаунт, логин`

### Страница регистрации
```typescript
usePageSEO.register();
```
- **Title:** `Регистрация | ItUpHub - Создать аккаунт` (45 символов)
- **Description:** `Создайте аккаунт на ItUpHub и присоединяйтесь к IT сообществу. Начните создавать проекты и находить команду.` (120 символов)
- **Keywords:** `регистрация, создать аккаунт, ItUpHub, IT сообщество, присоединиться`

## Кастомная SEO конфигурация

Для создания собственной SEO конфигурации используйте хук `useSEO`:

```typescript
import { useSEO } from '../utils/hooks/useSEO';

export default function CustomPage() {
  useSEO({
    title: 'Моя страница | ItUpHub',
    description: 'Описание моей страницы для поисковых систем.',
    keywords: 'ключевые, слова, для, страницы',
    ogTitle: 'Моя страница',
    ogDescription: 'Описание для социальных сетей',
    ogUrl: 'https://itupHub.com/my-page'
  });
  
  return <div>Содержимое страницы</div>;
}
```

## Рекомендации

### Заголовки (title)
- ✅ Длина до 70 символов
- ✅ Содержат ключевые слова
- ✅ Уникальные для каждой страницы
- ✅ Привлекательные для пользователей
- ❌ Не используйте дублирующиеся заголовки

### Описания (description)
- ✅ Длина до 200 символов
- ✅ Описывают ценность страницы
- ✅ Содержат призыв к действию
- ✅ Релевантные для поисковых запросов
- ❌ Не копируйте текст с других страниц

### Ключевые слова (keywords)
- ✅ Релевантные и популярные
- ✅ Не спамятся
- ✅ Охватывают различные аспекты контента
- ✅ Включают длинные хвосты
- ❌ Не используйте слишком много ключевых слов

## Примеры использования в существующих страницах

### Home.tsx
```typescript
import { usePageSEO } from '../utils/hooks/useSEO';

export default function Home() {
  usePageSEO.home();
  // ... остальной код
}
```

### ProjectsPage.tsx
```typescript
import { usePageSEO } from '../utils/hooks/useSEO';

export default function ProjectsPage() {
  usePageSEO.projects();
  // ... остальной код
}
```

### ProjectPage.tsx
```typescript
import { usePageSEO } from '../utils/hooks/useSEO';

export default function ProjectPage() {
  const [project, setProject] = useState(null);
  
  useEffect(() => {
    if (project) {
      usePageSEO.project(project.title, project.description);
    }
  }, [project]);
  
  // ... остальной код
}
```

## Тестирование

Для проверки SEO оптимизации:

1. Откройте страницу в браузере
2. Нажмите F12 для открытия DevTools
3. Перейдите на вкладку Elements
4. Найдите теги `<title>` и `<meta name="description">`
5. Проверьте содержимое и длину

## Мониторинг

Для отслеживания эффективности SEO:

1. **Google Search Console** - для мониторинга позиций
2. **Google Analytics** - для анализа трафика
3. **PageSpeed Insights** - для проверки скорости
4. **Lighthouse** - для комплексной оценки

## Полезные ссылки

- [SEO документация](frontend/SEO_OPTIMIZATION.md)
- [Пример компонента](frontend/src/components/SEOExample.tsx)
- [Утилиты SEO](frontend/src/utils/seo.ts)
- [React Hook](frontend/src/utils/hooks/useSEO.tsx) 