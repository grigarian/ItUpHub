import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

interface AdminRouteProps {
  children: React.ReactNode;
}

export const AdminRoute = ({ children }: AdminRouteProps) => {
  const { user } = useAuth();

  console.log(user)
  if (!user || !user.isAdmin) {
    return <Navigate to="/" replace />;
  }

  return <>{children}</>;
}; 