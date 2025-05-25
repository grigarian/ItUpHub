export default function LoadingSpinner({ size = 'medium' }: { size?: 'small' | 'medium' }) {
  const dimensions = {
    small: 'w-4 h-4',
    medium: 'w-6 h-6'
  };

  return (
    <div className={`animate-spin rounded-full border-2 border-t-transparent ${dimensions[size]}`} />
  );
}