// Утилита для управления SEO мета-тегами и заголовками страниц

export interface SEOConfig {
  title: string;
  description: string;
  keywords: string;
  ogTitle?: string;
  ogDescription?: string;
  ogImage?: string;
  ogUrl?: string;
}

/**
 * Обновляет заголовок страницы и мета-теги для SEO
 * @param config - конфигурация SEO
 */
export const updateSEO = (config: SEOConfig): void => {
  // Обновляем заголовок страницы
  document.title = config.title;
  
  // Обновляем meta description
  let metaDescription = document.querySelector('meta[name="description"]');
  if (!metaDescription) {
    metaDescription = document.createElement('meta');
    metaDescription.setAttribute('name', 'description');
    document.head.appendChild(metaDescription);
  }
  metaDescription.setAttribute('content', config.description);
  
  // Обновляем meta keywords
  let metaKeywords = document.querySelector('meta[name="keywords"]');
  if (!metaKeywords) {
    metaKeywords = document.createElement('meta');
    metaKeywords.setAttribute('name', 'keywords');
    document.head.appendChild(metaKeywords);
  }
  metaKeywords.setAttribute('content', config.keywords);
  
  // Обновляем Open Graph теги
  if (config.ogTitle) {
    let ogTitle = document.querySelector('meta[property="og:title"]');
    if (!ogTitle) {
      ogTitle = document.createElement('meta');
      ogTitle.setAttribute('property', 'og:title');
      document.head.appendChild(ogTitle);
    }
    ogTitle.setAttribute('content', config.ogTitle);
  }
  
  if (config.ogDescription) {
    let ogDescription = document.querySelector('meta[property="og:description"]');
    if (!ogDescription) {
      ogDescription = document.createElement('meta');
      ogDescription.setAttribute('property', 'og:description');
      document.head.appendChild(ogDescription);
    }
    ogDescription.setAttribute('content', config.ogDescription);
  }
  
  if (config.ogImage) {
    let ogImage = document.querySelector('meta[property="og:image"]');
    if (!ogImage) {
      ogImage = document.createElement('meta');
      ogImage.setAttribute('property', 'og:image');
      document.head.appendChild(ogImage);
    }
    ogImage.setAttribute('content', config.ogImage);
  }
  
  if (config.ogUrl) {
    let ogUrl = document.querySelector('meta[property="og:url"]');
    if (!ogUrl) {
      ogUrl = document.createElement('meta');
      ogUrl.setAttribute('property', 'og:url');
      document.head.appendChild(ogUrl);
    }
    ogUrl.setAttribute('content', config.ogUrl);
  }
};

/**
 * Предустановленные SEO конфигурации для разных страниц
 */
export const SEO_CONFIGS = {
  home: {
    title: 'ItUpHub - IT Сообщество | Проекты, команды, развитие',
    description: 'ItUpHub - платформа для IT сообщества. Создавайте проекты, находите команду, развивайте навыки и участвуйте в инновационных разработках.',
    keywords: 'IT, проекты, разработка, команда, программирование, технологии, сообщество, стартап, веб-разработка, мобильная разработка',
    ogTitle: 'ItUpHub - IT Сообщество',
    ogDescription: 'Платформа для IT сообщества. Создавайте проекты, находите команду, развивайте навыки.',
    ogUrl: 'https://itupHub.com/'
  },
  
  projects: {
    title: 'Проекты | ItUpHub - Каталог IT проектов',
    description: 'Найдите интересные IT проекты для участия. Веб-разработка, мобильные приложения, искусственный интеллект и многое другое.',
    keywords: 'IT проекты, веб-разработка, мобильные приложения, AI, машинное обучение, программирование, команда разработчиков',
    ogTitle: 'Проекты | ItUpHub',
    ogDescription: 'Найдите интересные IT проекты для участия. Веб-разработка, мобильные приложения, AI и многое другое.',
    ogUrl: 'https://itupHub.com/projects'
  },
  
  project: (projectTitle: string, projectDescription: string) => ({
    title: `${projectTitle} | ItUpHub`,
    description: projectDescription.length > 200 ? projectDescription.substring(0, 197) + '...' : projectDescription,
    keywords: `IT проект, ${projectTitle}, разработка, команда, программирование, технологии`,
    ogTitle: projectTitle,
    ogDescription: projectDescription.length > 200 ? projectDescription.substring(0, 197) + '...' : projectDescription,
    ogUrl: `https://itupHub.com/project/${projectTitle.toLowerCase().replace(/\s+/g, '-')}`
  }),
  
  profile: (userName: string) => ({
    title: `Профиль ${userName} | ItUpHub`,
    description: `Профиль разработчика ${userName} на платформе ItUpHub. Портфолио проектов, навыки и опыт.`,
    keywords: `профиль разработчика, ${userName}, портфолио, навыки программирования, IT специалист`,
    ogTitle: `Профиль ${userName} | ItUpHub`,
    ogDescription: `Профиль разработчика ${userName} на платформе ItUpHub. Портфолио проектов, навыки и опыт.`,
    ogUrl: `https://itupHub.com/user/${userName.toLowerCase().replace(/\s+/g, '-')}`
  }),
  
  login: {
    title: 'Вход | ItUpHub - Авторизация',
    description: 'Войдите в свой аккаунт ItUpHub для доступа к проектам, командам и возможностям развития.',
    keywords: 'вход, авторизация, ItUpHub, аккаунт, логин',
    ogTitle: 'Вход | ItUpHub',
    ogDescription: 'Войдите в свой аккаунт ItUpHub для доступа к проектам и командам.',
    ogUrl: 'https://itupHub.com/login'
  },
  
  register: {
    title: 'Регистрация | ItUpHub - Создать аккаунт',
    description: 'Создайте аккаунт на ItUpHub и присоединяйтесь к IT сообществу. Начните создавать проекты и находить команду.',
    keywords: 'регистрация, создать аккаунт, ItUpHub, IT сообщество, присоединиться',
    ogTitle: 'Регистрация | ItUpHub',
    ogDescription: 'Создайте аккаунт на ItUpHub и присоединяйтесь к IT сообществу.',
    ogUrl: 'https://itupHub.com/register'
  }
}; 