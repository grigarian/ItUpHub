// Утилита для работы с куками
export const getCookie = (name: string): string | null => {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) {
    const result = parts.pop()?.split(';').shift() || null;
    console.log(`Cookie ${name}:`, result ? 'found' : 'not found');
    return result;
  }
  console.log(`Cookie ${name}: not found`);
  return null;
};

export const setCookie = (name: string, value: string, days: number = 7): void => {
  const expires = new Date();
  expires.setTime(expires.getTime() + (days * 24 * 60 * 60 * 1000));
  document.cookie = `${name}=${value};expires=${expires.toUTCString()};path=/;SameSite=None`;
  console.log(`Cookie ${name} set with expiration:`, expires.toUTCString());
};

export const deleteCookie = (name: string): void => {
  document.cookie = `${name}=;expires=Thu, 01 Jan 1970 00:00:00 UTC;path=/;`;
  console.log(`Cookie ${name} deleted`);
};

// Функция для отладки всех куков
export const debugCookies = (): void => {
  console.log('All cookies:', document.cookie);
}; 