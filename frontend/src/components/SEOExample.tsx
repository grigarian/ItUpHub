import React from 'react';
import { useSEO } from '../utils/hooks/useSEO';
import { SEO_CONFIGS } from '../utils/seo';

/**
 * Компонент-пример для демонстрации SEO оптимизации
 * Показывает различные варианты заголовков, описаний и ключевых слов
 */
export const SEOExample: React.FC = () => {
  // Пример SEO для главной страницы
  useSEO(SEO_CONFIGS.home);

  return (
    <div className="max-w-4xl mx-auto p-6 bg-white rounded-lg shadow-lg">
      <h1 className="text-3xl font-bold text-gray-900 mb-6">
        Примеры SEO оптимизации для ItUpHub
      </h1>

      <div className="space-y-8">
        {/* Главная страница */}
        <section className="border-l-4 border-indigo-500 pl-6">
          <h2 className="text-2xl font-semibold text-gray-800 mb-4">
            Главная страница
          </h2>
          
          <div className="bg-gray-50 p-4 rounded-lg">
            <h3 className="font-medium text-gray-700 mb-2">Заголовок (title):</h3>
            <p className="text-sm bg-white p-2 rounded border font-mono">
              ItUpHub - IT Сообщество | Проекты, команды, развитие
            </p>
            <p className="text-xs text-gray-500 mt-1">
              Длина: 58 символов (рекомендуется до 70)
            </p>
          </div>

          <div className="bg-gray-50 p-4 rounded-lg mt-4">
            <h3 className="font-medium text-gray-700 mb-2">Описание (description):</h3>
            <p className="text-sm bg-white p-2 rounded border font-mono">
              ItUpHub - платформа для IT сообщества. Создавайте проекты, находите команду, развивайте навыки и участвуйте в инновационных разработках.
            </p>
            <p className="text-xs text-gray-500 mt-1">
              Длина: 158 символов (рекомендуется до 200)
            </p>
          </div>

          <div className="bg-gray-50 p-4 rounded-lg mt-4">
            <h3 className="font-medium text-gray-700 mb-2">Ключевые слова (keywords):</h3>
            <p className="text-sm bg-white p-2 rounded border font-mono">
              IT, проекты, разработка, команда, программирование, технологии, сообщество, стартап, веб-разработка, мобильная разработка
            </p>
          </div>
        </section>

        {/* Страница проектов */}
        <section className="border-l-4 border-green-500 pl-6">
          <h2 className="text-2xl font-semibold text-gray-800 mb-4">
            Страница проектов (каталог)
          </h2>
          
          <div className="bg-gray-50 p-4 rounded-lg">
            <h3 className="font-medium text-gray-700 mb-2">Заголовок (title):</h3>
            <p className="text-sm bg-white p-2 rounded border font-mono">
              Проекты | ItUpHub - Каталог IT проектов
            </p>
            <p className="text-xs text-gray-500 mt-1">
              Длина: 47 символов
            </p>
          </div>

          <div className="bg-gray-50 p-4 rounded-lg mt-4">
            <h3 className="font-medium text-gray-700 mb-2">Описание (description):</h3>
            <p className="text-sm bg-white p-2 rounded border font-mono">
              Найдите интересные IT проекты для участия. Веб-разработка, мобильные приложения, искусственный интеллект и многое другое.
            </p>
            <p className="text-xs text-gray-500 mt-1">
              Длина: 142 символа
            </p>
          </div>

          <div className="bg-gray-50 p-4 rounded-lg mt-4">
            <h3 className="font-medium text-gray-700 mb-2">Ключевые слова (keywords):</h3>
            <p className="text-sm bg-white p-2 rounded border font-mono">
              IT проекты, веб-разработка, мобильные приложения, AI, машинное обучение, программирование, команда разработчиков
            </p>
          </div>
        </section>

        {/* Страница отдельного проекта */}
        <section className="border-l-4 border-purple-500 pl-6">
          <h2 className="text-2xl font-semibold text-gray-800 mb-4">
            Страница отдельного проекта
          </h2>
          
          <div className="bg-gray-50 p-4 rounded-lg">
            <h3 className="font-medium text-gray-700 mb-2">Заголовок (title):</h3>
            <p className="text-sm bg-white p-2 rounded border font-mono">
              Веб-приложение для управления задачами | ItUpHub
            </p>
            <p className="text-xs text-gray-500 mt-1">
              Длина: 52 символа
            </p>
          </div>

          <div className="bg-gray-50 p-4 rounded-lg mt-4">
            <h3 className="font-medium text-gray-700 mb-2">Описание (description):</h3>
            <p className="text-sm bg-white p-2 rounded border font-mono">
              Современное веб-приложение для эффективного управления задачами и проектами. Использует React, Node.js и современные технологии разработки.
            </p>
            <p className="text-xs text-gray-500 mt-1">
              Длина: 156 символов
            </p>
          </div>

          <div className="bg-gray-50 p-4 rounded-lg mt-4">
            <h3 className="font-medium text-gray-700 mb-2">Ключевые слова (keywords):</h3>
            <p className="text-sm bg-white p-2 rounded border font-mono">
              IT проект, веб-приложение, управление задачами, React, Node.js, разработка, команда, программирование, технологии
            </p>
          </div>
        </section>

        {/* Страница профиля */}
        <section className="border-l-4 border-blue-500 pl-6">
          <h2 className="text-2xl font-semibold text-gray-800 mb-4">
            Страница профиля пользователя
          </h2>
          
          <div className="bg-gray-50 p-4 rounded-lg">
            <h3 className="font-medium text-gray-700 mb-2">Заголовок (title):</h3>
            <p className="text-sm bg-white p-2 rounded border font-mono">
              Профиль Иван Петров | ItUpHub
            </p>
            <p className="text-xs text-gray-500 mt-1">
              Длина: 38 символов
            </p>
          </div>

          <div className="bg-gray-50 p-4 rounded-lg mt-4">
            <h3 className="font-medium text-gray-700 mb-2">Описание (description):</h3>
            <p className="text-sm bg-white p-2 rounded border font-mono">
              Профиль разработчика Иван Петров на платформе ItUpHub. Портфолио проектов, навыки и опыт в веб-разработке.
            </p>
            <p className="text-xs text-gray-500 mt-1">
              Длина: 134 символа
            </p>
          </div>

          <div className="bg-gray-50 p-4 rounded-lg mt-4">
            <h3 className="font-medium text-gray-700 mb-2">Ключевые слова (keywords):</h3>
            <p className="text-sm bg-white p-2 rounded border font-mono">
              профиль разработчика, Иван Петров, портфолио, навыки программирования, IT специалист, веб-разработка
            </p>
          </div>
        </section>

        {/* Рекомендации */}
        <section className="border-l-4 border-yellow-500 pl-6">
          <h2 className="text-2xl font-semibold text-gray-800 mb-4">
            Рекомендации по SEO
          </h2>
          
          <div className="bg-yellow-50 p-4 rounded-lg">
            <h3 className="font-medium text-gray-700 mb-2">Заголовки страниц:</h3>
            <ul className="text-sm space-y-1">
              <li>• Длина не более 70 символов</li>
              <li>• Содержат ключевые слова</li>
              <li>• Уникальные для каждой страницы</li>
              <li>• Привлекательные для пользователей</li>
            </ul>
          </div>

          <div className="bg-yellow-50 p-4 rounded-lg mt-4">
            <h3 className="font-medium text-gray-700 mb-2">Описания:</h3>
            <ul className="text-sm space-y-1">
              <li>• Длина не более 200 символов</li>
              <li>• Описывают ценность страницы</li>
              <li>• Содержат призыв к действию</li>
              <li>• Релевантные для поисковых запросов</li>
            </ul>
          </div>

          <div className="bg-yellow-50 p-4 rounded-lg mt-4">
            <h3 className="font-medium text-gray-700 mb-2">Ключевые слова:</h3>
            <ul className="text-sm space-y-1">
              <li>• Релевантные и популярные</li>
              <li>• Не спамятся</li>
              <li>• Охватывают различные аспекты контента</li>
              <li>• Включают длинные хвосты</li>
            </ul>
          </div>
        </section>
      </div>
    </div>
  );
};

export default SEOExample; 