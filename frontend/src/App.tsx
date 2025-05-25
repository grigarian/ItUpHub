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
import UserProfilePage from "./pages/UserProfilePage";
import NotificationsPage from "./pages/NotificationPage";


export default function App() {
  return (
    <div className="max-w-4xl mx-auto p-4">
      <Navbar />
      <main>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/profile/edit/:userId" element={<ProfileEditPage />} /> {/* ← Вот это исправление */}
          <Route path="/my-projects" element={<MyProjectsPage />} />
          <Route path="/create-project" element={<CreateProjectPage />} />
          <Route path="/project/:id/get" element={<ProjectPage />} />
          <Route path="/user/:userId" element={<UserProfilePage />} />
          <Route path="/notifications" element={<NotificationsPage />} />
        </Routes>
      </main>
      <Toaster position="top-center" />
    </div>
  );
}