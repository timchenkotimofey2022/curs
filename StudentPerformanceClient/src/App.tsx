import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import Layout from './components/Layout';
import PrivateRoute from './components/PrivateRoute';
import LoginPage from './pages/LoginPage';
import DashboardPage from './pages/DashboardPage';
import StudentsPage from './pages/StudentsPage';
import StudentDetailPage from './pages/StudentDetailPage';
import GroupsPage from './pages/GroupsPage';
import SubjectsPage from './pages/SubjectsPage';
import TeachersPage from './pages/TeachersPage';
import GradesPage from './pages/GradesPage';
import AddGradePage from './pages/AddGradePage';

function App() {
  return (
    <AuthProvider>
      <Router>
        <Layout>
          <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/" element={<Navigate to="/dashboard" replace />} />
            <Route
              path="/dashboard"
              element={
                <PrivateRoute>
                  <DashboardPage />
                </PrivateRoute>
              }
            />
            <Route
              path="/students"
              element={
                <PrivateRoute>
                  <StudentsPage />
                </PrivateRoute>
              }
            />
            <Route
              path="/students/:id"
              element={
                <PrivateRoute>
                  <StudentDetailPage />
                </PrivateRoute>
              }
            />
            <Route
              path="/groups"
              element={
                <PrivateRoute>
                  <GroupsPage />
                </PrivateRoute>
              }
            />
            <Route
              path="/subjects"
              element={
                <PrivateRoute>
                  <SubjectsPage />
                </PrivateRoute>
              }
            />
            <Route
              path="/teachers"
              element={
                <PrivateRoute>
                  <TeachersPage />
                </PrivateRoute>
              }
            />
            <Route
              path="/grades"
              element={
                <PrivateRoute>
                  <GradesPage />
                </PrivateRoute>
              }
            />
            <Route
              path="/grades/add"
              element={
                <PrivateRoute>
                  <AddGradePage />
                </PrivateRoute>
              }
            />
          </Routes>
        </Layout>
      </Router>
    </AuthProvider>
  );
}

export default App;
