import { Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import toast, { Toaster } from 'react-hot-toast';
import ProfilePage from './pages/ProfilePage';
import ProfileEditPage from "./pages/ProfileEditPage";
import MyProjectsPage from "./pages/MyProjectPage";
import CreateProjectPage from "./pages/CreateProjectPage";
import ProjectPage from "./pages/ProjectPage";
import ProjectsPage from "./pages/ProjectsPage";
import UserProfilePage from "./pages/UserProfilePage";
import NotificationsPage from "./pages/NotificationPage";
import { ProjectBoardPage } from "./pages/ProjectBoardPage";
import { AdminPanel } from './pages/AdminPanel';
import AdminUsersPage from './pages/AdminUsersPage';
import { AdminRoute } from './components/AdminRoute';
import TermsPage from './pages/TermsPage';


export default function App() {
  return (
    <div className="w-full min-h-screen p-4">
      <Navbar />
      <main>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route
            path="/admin"
            element={
              <AdminRoute>
                <AdminPanel />
              </AdminRoute>
            }
          />
          <Route
            path="/admin/users"
            element={
              <AdminRoute>
                <AdminUsersPage />
              </AdminRoute>
            }
          />
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/profile/edit/:userId" element={<ProfileEditPage />} /> {/* ← Вот это исправление */}
          <Route path="/my-projects" element={<MyProjectsPage />} />
          <Route path="/projects" element={<ProjectsPage />} />
          <Route path="/create-project" element={<CreateProjectPage />} />
          <Route path="/project/:id/get" element={<ProjectPage />} />
          <Route path="/user/:userId" element={<UserProfilePage />} />
          <Route path="/notifications" element={<NotificationsPage />} />
          <Route path="/projects/:projectId/board" element={<ProjectBoardPage />} />
          <Route path="/terms" element={<TermsPage />} />
        </Routes>
      </main>
      <Toaster position="top-center" />
    </div>
  );
}