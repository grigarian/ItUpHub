# Деплой ItUpHub на Vercel

## 🚀 Быстрый старт

### 1. Подготовка GitHub репозитория

1. **Создайте репозиторий на GitHub:**
   ```bash
   git init
   git add .
   git commit -m "Initial commit"
   git branch -M main
   git remote add origin https://github.com/your-username/ituphub.git
   git push -u origin main
   ```

### 2. Настройка Vercel

1. **Зарегистрируйтесь на [vercel.com](https://vercel.com)**
2. **Подключите GitHub аккаунт**
3. **Нажмите "New Project"**
4. **Выберите ваш репозиторий**
5. **Настройте проект:**

   **Build Settings:**
   - Framework Preset: `Other`
   - Root Directory: `./` (оставьте пустым)
   - Build Command: `cd frontend && npm install && npm run build`
   - Output Directory: `frontend/build`

### 3. Переменные окружения

В настройках проекта добавьте:

```
# Database
DATABASE_URL=your_postgresql_connection_string

# JWT
JWT_SECRET_KEY=your_jwt_secret_key

# CORS
CORS__ALLOWEDORIGINS=https://your-vercel-domain.vercel.app

# Environment
ASPNETCORE_ENVIRONMENT=Production
```

### 4. База данных

Для базы данных используйте:
- **Supabase** (бесплатно)
- **Railway** (бесплатно)
- **Neon** (бесплатно)

### 5. Деплой

1. **Нажмите "Deploy"**
2. **Дождитесь завершения сборки**
3. **Получите URL вашего сайта**

## 🔧 Альтернативный подход

Если .NET бэкенд не работает на Vercel, можно:

1. **Фронтенд на Vercel**
2. **Бэкенд на Railway/Render**
3. **База данных на Supabase**

## 📋 Проверка деплоя

После деплоя проверьте:
- [ ] Фронтенд загружается
- [ ] API работает
- [ ] База данных подключена
- [ ] SSL сертификат активен

## 🆘 Troubleshooting

### Проблемы с .NET
Если .NET не работает на Vercel:
1. Используйте Railway для бэкенда
2. Или перепишите API на Node.js

### Проблемы с базой данных
1. Проверьте connection string
2. Убедитесь, что база доступна извне
3. Проверьте переменные окружения

## 📞 Поддержка

При проблемах:
- Email: grigarian24@mail.ru
- Telegram: @grigarian24 