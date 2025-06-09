import { Link } from 'react-router-dom';
import { ArrowLeft, Shield, Users, Code, Heart } from 'lucide-react';

export default function TermsPage() {
  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <div className="bg-white shadow-sm border-b">
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <div className="flex items-center justify-between">
            <div className="flex items-center space-x-4">
              <Link
                to="/"
                className="text-2xl font-bold bg-gradient-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent"
              >
                ItUpHub
              </Link>
              <div className="h-6 w-px bg-gray-300"></div>
              <h1 className="text-2xl font-bold text-gray-900">Условия пользования</h1>
            </div>
            <Link
              to="/"
              className="inline-flex items-center px-4 py-2 text-gray-600 hover:text-gray-900 transition-colors"
            >
              <ArrowLeft className="w-4 h-4 mr-2" />
              На главную
            </Link>
          </div>
        </div>
      </div>

      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
        <div className="bg-white rounded-2xl shadow-lg p-8">
          {/* Introduction */}
          <div className="text-center mb-12">
            <div className="w-16 h-16 bg-gradient-to-br from-indigo-500 to-purple-600 rounded-2xl flex items-center justify-center text-white mx-auto mb-6">
              <Shield className="w-8 h-8" />
            </div>
            <h2 className="text-3xl font-bold text-gray-900 mb-4">
              Условия пользования ItUpHub
            </h2>
            <p className="text-lg text-gray-600 max-w-2xl mx-auto">
              Последнее обновление: {new Date().toLocaleDateString('ru-RU')}
            </p>
          </div>

          <div className="prose prose-lg max-w-none">
            {/* General Terms */}
            <section className="mb-12">
              <h3 className="text-2xl font-bold text-gray-900 mb-6 flex items-center">
                <Users className="w-6 h-6 mr-3 text-indigo-600" />
                1. Общие положения
              </h3>
              <div className="space-y-4 text-gray-700">
                <p>
                  Добро пожаловать в ItUpHub! Используя нашу платформу, вы соглашаетесь с настоящими условиями пользования. 
                  ItUpHub — это платформа для поиска команд и реализации проектов в сфере разработки программного обеспечения.
                </p>
                <p>
                  Платформа предназначена для разработчиков, дизайнеров, менеджеров проектов и других специалистов, 
                  которые хотят участвовать в совместной работе над проектами или создавать собственные команды.
                </p>
                <p>
                  Используя ItUpHub, вы подтверждаете, что вам исполнилось 18 лет или вы получили согласие родителей/опекунов 
                  на использование платформы.
                </p>
              </div>
            </section>

            {/* Account Registration */}
            <section className="mb-12">
              <h3 className="text-2xl font-bold text-gray-900 mb-6 flex items-center">
                <Code className="w-6 h-6 mr-3 text-indigo-600" />
                2. Регистрация и аккаунт
              </h3>
              <div className="space-y-4 text-gray-700">
                <p>
                  Для использования всех функций платформы необходимо создать аккаунт. При регистрации вы обязуетесь:
                </p>
                <ul className="list-disc pl-6 space-y-2">
                  <li>Предоставить достоверную и актуальную информацию</li>
                  <li>Не использовать чужие данные или создавать фальшивые аккаунты</li>
                  <li>Нести ответственность за безопасность своих учетных данных</li>
                  <li>Немедленно уведомить нас о любом несанкционированном использовании аккаунта</li>
                  <li>Не передавать доступ к аккаунту третьим лицам</li>
                </ul>
                <p>
                  Мы оставляем за собой право отказать в регистрации или приостановить аккаунт в случае нарушения 
                  настоящих условий.
                </p>
              </div>
            </section>

            {/* User Conduct */}
            <section className="mb-12">
              <h3 className="text-2xl font-bold text-gray-900 mb-6 flex items-center">
                <Heart className="w-6 h-6 mr-3 text-indigo-600" />
                3. Правила поведения
              </h3>
              <div className="space-y-4 text-gray-700">
                <p>
                  Используя ItUpHub, вы соглашаетесь соблюдать следующие правила:
                </p>
                <div className="grid md:grid-cols-2 gap-6">
                  <div>
                    <h4 className="font-semibold text-gray-900 mb-3">Что разрешено:</h4>
                    <ul className="list-disc pl-6 space-y-2 text-sm">
                      <li>Создавать и участвовать в проектах</li>
                      <li>Общаться с другими участниками</li>
                      <li>Делиться опытом и знаниями</li>
                      <li>Использовать платформу для профессионального развития</li>
                      <li>Создавать портфолио проектов</li>
                    </ul>
                  </div>
                  <div>
                    <h4 className="font-semibold text-gray-900 mb-3">Что запрещено:</h4>
                    <ul className="list-disc pl-6 space-y-2 text-sm">
                      <li>Оскорбления и дискриминация</li>
                      <li>Спам и реклама без разрешения</li>
                      <li>Нарушение авторских прав</li>
                      <li>Попытки взлома или вредоносная активность</li>
                      <li>Распространение незаконного контента</li>
                    </ul>
                  </div>
                </div>
              </div>
            </section>

            {/* Privacy and Data */}
            <section className="mb-12">
              <h3 className="text-2xl font-bold text-gray-900 mb-6 flex items-center">
                <Shield className="w-6 h-6 mr-3 text-indigo-600" />
                4. Конфиденциальность и данные
              </h3>
              <div className="space-y-4 text-gray-700">
                <p>
                  Мы серьезно относимся к защите ваших персональных данных. Наша политика конфиденциальности 
                  описывает, как мы собираем, используем и защищаем вашу информацию.
                </p>
                <p>
                  Вы соглашаетесь, что мы можем:
                </p>
                <ul className="list-disc pl-6 space-y-2">
                  <li>Собирать и обрабатывать данные, необходимые для работы платформы</li>
                  <li>Использовать cookies и аналогичные технологии</li>
                  <li>Отправлять уведомления о важных изменениях</li>
                  <li>Анализировать использование платформы для улучшения сервиса</li>
                </ul>
                <p>
                  Мы не продаем ваши персональные данные третьим лицам и используем их только 
                  в соответствии с нашей политикой конфиденциальности.
                </p>
              </div>
            </section>

            {/* Intellectual Property */}
            <section className="mb-12">
              <h3 className="text-2xl font-bold text-gray-900 mb-6 flex items-center">
                <Code className="w-6 h-6 mr-3 text-indigo-600" />
                5. Интеллектуальная собственность
              </h3>
              <div className="space-y-4 text-gray-700">
                <p>
                  <strong>Платформа ItUpHub:</strong> Все права на платформу, включая дизайн, код, 
                  логотипы и торговые марки, принадлежат нам.
                </p>
                <p>
                  <strong>Пользовательский контент:</strong> Вы сохраняете права на контент, который создаете 
                  на платформе (проекты, сообщения, профили). Размещая контент, вы предоставляете нам 
                  лицензию на его использование в рамках работы платформы.
                </p>
                <p>
                  <strong>Проекты:</strong> Права на проекты, созданные с помощью платформы, 
                  определяются участниками проекта и их соглашениями.
                </p>
              </div>
            </section>

            {/* Liability */}
            <section className="mb-12">
              <h3 className="text-2xl font-bold text-gray-900 mb-6 flex items-center">
                <Shield className="w-6 h-6 mr-3 text-indigo-600" />
                6. Ответственность
              </h3>
              <div className="space-y-4 text-gray-700">
                <p>
                  <strong>Ограничение ответственности:</strong> ItUpHub предоставляется "как есть" без каких-либо 
                  гарантий. Мы не несем ответственности за:
                </p>
                <ul className="list-disc pl-6 space-y-2">
                  <li>Качество или результат проектов, созданных на платформе</li>
                  <li>Действия других пользователей</li>
                  <li>Потерю данных или прерывание работы сервиса</li>
                  <li>Косвенные или случайные убытки</li>
                </ul>
                <p>
                  <strong>Ваша ответственность:</strong> Вы несете ответственность за:
                </p>
                <ul className="list-disc pl-6 space-y-2">
                  <li>Соблюдение настоящих условий</li>
                  <li>Контент, который вы размещаете</li>
                  <li>Взаимодействие с другими пользователями</li>
                  <li>Соблюдение законов при использовании платформы</li>
                </ul>
              </div>
            </section>

            {/* Termination */}
            <section className="mb-12">
              <h3 className="text-2xl font-bold text-gray-900 mb-6 flex items-center">
                <Users className="w-6 h-6 mr-3 text-indigo-600" />
                7. Прекращение использования
              </h3>
              <div className="space-y-4 text-gray-700">
                <p>
                  Вы можете прекратить использование платформы в любое время, удалив свой аккаунт.
                </p>
                <p>
                  Мы можем приостановить или удалить ваш аккаунт в случае:
                </p>
                <ul className="list-disc pl-6 space-y-2">
                  <li>Нарушения настоящих условий</li>
                  <li>Неактивности в течение длительного периода</li>
                  <li>Запроса правоохранительных органов</li>
                  <li>Технических проблем или обновлений платформы</li>
                </ul>
                <p>
                  При удалении аккаунта ваши данные будут удалены в соответствии с нашей политикой 
                  конфиденциальности, за исключением случаев, когда требуется их сохранение по закону.
                </p>
              </div>
            </section>

            {/* Changes to Terms */}
            <section className="mb-12">
              <h3 className="text-2xl font-bold text-gray-900 mb-6 flex items-center">
                <Code className="w-6 h-6 mr-3 text-indigo-600" />
                8. Изменения условий
              </h3>
              <div className="space-y-4 text-gray-700">
                <p>
                  Мы оставляем за собой право изменять настоящие условия в любое время. 
                  О значительных изменениях мы будем уведомлять вас через платформу или по email.
                </p>
                <p>
                  Продолжение использования платформы после внесения изменений означает 
                  ваше согласие с новыми условиями.
                </p>
                <p>
                  Рекомендуем периодически проверять эту страницу для ознакомления с актуальными условиями.
                </p>
              </div>
            </section>

            {/* Contact Information */}
            <section className="mb-12">
              <h3 className="text-2xl font-bold text-gray-900 mb-6 flex items-center">
                <Heart className="w-6 h-6 mr-3 text-indigo-600" />
                9. Контактная информация
              </h3>
              <div className="space-y-4 text-gray-700">
                <p>
                  Если у вас есть вопросы по настоящим условиям, свяжитесь с нами:
                </p>
                <div className="bg-gray-50 rounded-xl p-6">
                  <div className="grid md:grid-cols-2 gap-4">
                    <div>
                      <h4 className="font-semibold text-gray-900 mb-2">Email:</h4>
                      <a 
                        href="mailto:grigarian24@mail.ru" 
                        className="text-indigo-600 hover:text-indigo-700"
                      >
                        grigarian24@mail.ru
                      </a>
                    </div>
                    <div>
                      <h4 className="font-semibold text-gray-900 mb-2">Telegram:</h4>
                      <a 
                        href="https://t.me/grigarian24" 
                        target="_blank" 
                        rel="noopener noreferrer"
                        className="text-indigo-600 hover:text-indigo-700"
                      >
                        @grigarian24
                      </a>
                    </div>
                  </div>
                </div>
              </div>
            </section>

            {/* Final Statement */}
            <section className="bg-gradient-to-r from-indigo-50 to-purple-50 rounded-2xl p-8 text-center">
              <h3 className="text-xl font-bold text-gray-900 mb-4">
                Спасибо за использование ItUpHub!
              </h3>
              <p className="text-gray-700 mb-6">
                Мы стремимся создать безопасную и полезную платформу для сообщества разработчиков. 
                Ваше соблюдение этих условий помогает нам поддерживать качество сервиса.
              </p>
              <Link
                to="/"
                className="inline-flex items-center px-6 py-3 bg-indigo-600 text-white font-medium rounded-xl hover:bg-indigo-700 transition-all duration-300"
              >
                Вернуться на главную
              </Link>
            </section>
          </div>
        </div>
      </div>
    </div>
  );
} 