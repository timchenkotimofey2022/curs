import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Layout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const { logout, user, isAuthenticated } = useAuth();
  const location = useLocation();
  const role = user?.role;
  const isAdmin = role === 'Admin';
  const isTeacher = role === 'Teacher';
  const isAdminOrTeacher = isAdmin || isTeacher;

  const navLinks = [
    { path: '/dashboard', label: 'Дашборд' },
    { path: '/students', label: 'Студенты' },
    { path: '/groups', label: 'Группы' },
    { path: '/subjects', label: 'Предметы' },
    { path: '/teachers', label: 'Преподаватели', adminOnly: true },
    { path: '/grades', label: 'Оценки' },
    { path: '/grades/add', label: 'Добавить оценку', adminOrTeacherOnly: true },
  ].filter((link) => {
    if (link.adminOnly && !isAdmin) return false;
    if (link.adminOrTeacherOnly && !isAdminOrTeacher) return false;
    return true;
  });

  return (
    <div className="min-h-screen bg-slate-50">
      <nav className="bg-gradient-to-r from-indigo-600 to-blue-700 shadow-lg">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between h-16">
            <div className="flex items-center space-x-8">
              <Link to="/" className="text-xl font-bold text-white flex items-center gap-2">
                <span>Успеваемость</span>
              </Link>
              {isAuthenticated && (
                <div className="hidden md:flex space-x-1">
                  {navLinks.map((link) => (
                    <Link
                      key={link.path}
                      to={link.path}
                      className={`px-3 py-2 rounded-lg text-sm font-medium transition-all duration-200 ${
                        location.pathname === link.path
                          ? 'bg-white/20 text-white shadow-inner'
                          : 'text-indigo-100 hover:text-white hover:bg-white/10'
                      }`}
                    >
                      {link.label}
                    </Link>
                  ))}
                </div>
              )}
            </div>
            <div className="flex items-center space-x-4">
              {isAuthenticated ? (
                <>
                  <span className="text-sm text-indigo-100 bg-white/10 px-3 py-1 rounded-full">
                    {user?.email} · <span className="font-semibold text-white">{user?.role}</span>
                  </span>
                  <button
                    onClick={logout}
                    className="px-4 py-2 text-sm font-medium text-white bg-red-500/80 hover:bg-red-500 rounded-lg transition-all duration-200 shadow-md hover:shadow-lg"
                  >
                    Выйти
                  </button>
                </>
              ) : (
                <Link
                  to="/login"
                  className="px-4 py-2 text-sm font-medium text-indigo-700 bg-white hover:bg-indigo-50 rounded-lg transition-all duration-200 shadow-md"
                >
                  Войти
                </Link>
              )}
            </div>
          </div>
        </div>
      </nav>
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {children}
      </main>
      <footer className="bg-white border-t mt-auto">
        <div className="max-w-7xl mx-auto px-4 py-6 text-center text-sm text-slate-500">
          © 2026 ИС Учета Успеваемости · ГБПОУ СКС · Группа ИВ-234
        </div>
      </footer>
    </div>
  );
};

export default Layout;
