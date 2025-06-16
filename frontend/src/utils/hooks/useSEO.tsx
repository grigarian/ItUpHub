import { useEffect } from 'react';
import { updateSEO, SEOConfig } from '../seo';

/**
 * React Hook для управления SEO мета-тегами
 * @param config - конфигурация SEO
 */
export const useSEO = (config: SEOConfig): void => {
  useEffect(() => {
    updateSEO(config);
  }, [config.title, config.description, config.keywords]);
};

/**
 * Hook для быстрого применения предустановленных SEO конфигураций
 */
export const usePageSEO = {
  home: () => useSEO({
    title: 'ItUpHub - IT Сообщество | Проекты, команды, развитие',
    description: 'ItUpHub - платформа для IT сообщества. Создавайте проекты, находите команду, развивайте навыки и участвуйте в инновационных разработках.',
    keywords: 'IT, проекты, разработка, команда, программирование, технологии, сообщество, стартап, веб-разработка, мобильная разработка',
    ogTitle: 'ItUpHub - IT Сообщество',
    ogDescription: 'Платформа для IT сообщества. Создавайте проекты, находите команду, развивайте навыки.',
    ogUrl: 'https://itupHub.com/'
  }),

  projects: () => useSEO({
    title: 'Проекты | ItUpHub - Каталог IT проектов',
    description: 'Найдите интересные IT проекты для участия. Веб-разработка, мобильные приложения, искусственный интеллект и многое другое.',
    keywords: 'IT проекты, веб-разработка, мобильные приложения, AI, машинное обучение, программирование, команда разработчиков',
    ogTitle: 'Проекты | ItUpHub',
    ogDescription: 'Найдите интересные IT проекты для участия. Веб-разработка, мобильные приложения, AI и многое другое.',
    ogUrl: 'https://itupHub.com/projects'
  }),

  project: (projectTitle: string, projectDescription: string) => useSEO({
    title: `${projectTitle} | ItUpHub`,
    description: projectDescription.length > 200 ? projectDescription.substring(0, 197) + '...' : projectDescription,
    keywords: `IT проект, ${projectTitle}, разработка, команда, программирование, технологии`,
    ogTitle: projectTitle,
    ogDescription: projectDescription.length > 200 ? projectDescription.substring(0, 197) + '...' : projectDescription,
    ogUrl: `https://itupHub.com/project/${projectTitle.toLowerCase().replace(/\s+/g, '-')}`
  }),

  profile: (userName: string) => useSEO({
    title: `Профиль ${userName} | ItUpHub`,
    description: `Профиль разработчика ${userName} на платформе ItUpHub. Портфолио проектов, навыки и опыт.`,
    keywords: `профиль разработчика, ${userName}, портфолио, навыки программирования, IT специалист`,
    ogTitle: `Профиль ${userName} | ItUpHub`,
    ogDescription: `Профиль разработчика ${userName} на платформе ItUpHub. Портфолио проектов, навыки и опыт.`,
    ogUrl: `https://itupHub.com/user/${userName.toLowerCase().replace(/\s+/g, '-')}`
  }),

  login: () => useSEO({
    title: 'Вход | ItUpHub - Авторизация',
    description: 'Войдите в свой аккаунт ItUpHub для доступа к проектам, командам и возможностям развития.',
    keywords: 'вход, авторизация, ItUpHub, аккаунт, логин',
    ogTitle: 'Вход | ItUpHub',
    ogDescription: 'Войдите в свой аккаунт ItUpHub для доступа к проектам и командам.',
    ogUrl: 'https://itupHub.com/login'
  }),

  register: () => useSEO({
    title: 'Регистрация | ItUpHub - Создать аккаунт',
    description: 'Создайте аккаунт на ItUpHub и присоединяйтесь к IT сообществу. Начните создавать проекты и находить команду.',
    keywords: 'регистрация, создать аккаунт, ItUpHub, IT сообщество, присоединиться',
    ogTitle: 'Регистрация | ItUpHub',
    ogDescription: 'Создайте аккаунт на ItUpHub и присоединяйтесь к IT сообществу.',
    ogUrl: 'https://itupHub.com/register'
  })
}; 