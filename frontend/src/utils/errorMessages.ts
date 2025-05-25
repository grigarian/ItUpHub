// src/utils/errorMessages.ts

export const errorMessagesMap: Record<string, string> = {
  "invalid.password": "Пароль должен содержать минимум 8 символов, хотя бы одну цифру и специальный символ",
  "invalid.email": "Некорректный адрес электронной почты",
  "failed.to.login": "Неверный логин или пароль",
  "value.is.invalid": "Введено некорректное значение",
  "record.not.found": "Запись не найдена",
  "unauthorized": "Пользователь не авторизован",
  // сюда добавляй остальные коды ошибок
};

/**
 * Функция для получения понятного сообщения по коду ошибки.
 * Если код неизвестен, возвращает либо сообщение из backendError.message, либо дефолтное сообщение.
 */
export function getErrorMessage(backendError: { code?: string; message?: string }): string {
  if (backendError.code && errorMessagesMap[backendError.code]) {
    return errorMessagesMap[backendError.code];
  }
  if (backendError.message) {
    return backendError.message;
  }
  return 'Произошла ошибка на сервере';
}