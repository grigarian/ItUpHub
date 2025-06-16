import { useEffect } from 'react';
import { updateSEO, SEOConfig, SEO_CONFIGS } from '../seo';

/**
 * React Hook для управления SEO мета-тегами
 * @param config - конфигурация SEO
 */
export const useSEO = (config: SEOConfig): void => {
  useEffect(() => {
    updateSEO(config);
  }, [config.title, config.description, config.keywords]);
};

// Экспортируйте SEO_CONFIGS для использования в компонентах
export { SEO_CONFIGS }; 