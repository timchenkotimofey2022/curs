import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { studentsApi, groupsApi } from '../services/api';
import type { Student, Group } from '../types';
import { useAuth } from '../context/AuthContext';

const groupColors: Record<string, string> = {
  'ИС-101': 'bg-sky-100 text-sky-700',
  'ИС-102': 'bg-blue-100 text-blue-700',
  'ИС-201': 'bg-violet-100 text-violet-700',
  'ИС-202': 'bg-purple-100 text-purple-700',
  'ИС-301': 'bg-emerald-100 text-emerald-700',
};

const getInitials = (name: string) => {
  const parts = name.split(' ');
  if (parts.length >= 2) return `${parts[0][0]}${parts[1][0]}`.toUpperCase();
  return name.slice(0, 2).toUpperCase();
};

const avatarColors = [
  'from-rose-400 to-red-500',
  'from-amber-400 to-orange-500',
  'from-emerald-400 to-green-500',
  'from-sky-400 to-blue-500',
  'from-violet-400 to-purple-500',
  'from-pink-400 to-rose-500',
];

const StudentsPage: React.FC = () => {
  const [students, setStudents] = useState<Student[]>([]);
  const [groups, setGroups] = useState<Group[]>([]);
  const [selectedGroup, setSelectedGroup] = useState<number | ''>('');
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<Student | null>(null);
  const [form, setForm] = useState({ fullName: '', email: '', groupId: 1 });
  const [error, setError] = useState('');
  const { user } = useAuth();
  const isAdminOrTeacher = user?.role === 'Admin' || user?.role === 'Teacher';

  const fetchData = async () => {
    setLoading(true);
    try {
      const [studentsRes, groupsRes] = await Promise.all([
        studentsApi.getAll(),
        groupsApi.getAll(),
      ]);
      setStudents(studentsRes.data);
      setGroups(groupsRes.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const openCreate = () => {
    setEditing(null);
    setForm({ fullName: '', email: '', groupId: groups[0]?.id || 1 });
    setError('');
    setModalOpen(true);
  };

  const openEdit = (student: Student) => {
    setEditing(student);
    setForm({ fullName: student.fullName, email: student.email, groupId: student.groupId });
    setError('');
    setModalOpen(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    try {
      if (editing) {
        await studentsApi.update(editing.id, form);
      } else {
        await studentsApi.create(form);
      }
      setModalOpen(false);
      fetchData();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Ошибка сохранения');
    }
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Удалить студента?')) return;
    try {
      await studentsApi.delete(id);
      fetchData();
    } catch (err: any) {
      alert(err.response?.data?.message || 'Ошибка удаления');
    }
  };

  const filteredStudents = selectedGroup
    ? students.filter((s) => s.groupId === selectedGroup)
    : students;

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
      </div>
    );
  }

  return (
    <div className="space-y-6 animate-fade-in">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-slate-800">Студенты</h1>
        <div className="flex items-center gap-3">
          <label className="text-sm font-medium text-slate-600">Группа:</label>
          <select
            value={selectedGroup}
            onChange={(e) => setSelectedGroup(e.target.value ? Number(e.target.value) : '')}
            className="border border-slate-200 rounded-lg px-3 py-2 bg-white text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent shadow-sm"
          >
            <option value="">Все группы</option>
            {groups.map((g) => (
              <option key={g.id} value={g.id}>
                {g.name}
              </option>
            ))}
          </select>
          {isAdminOrTeacher && (
            <button
              onClick={openCreate}
              className="px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-colors shadow-md"
            >
              + Добавить
            </button>
          )}
        </div>
      </div>

      <div className="bg-white rounded-xl shadow-sm border border-slate-100 overflow-hidden">
        <table className="min-w-full divide-y divide-slate-100">
          <thead className="bg-slate-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider">Студент</th>
              <th className="px-6 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider">Email</th>
              <th className="px-6 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider">Группа</th>
              <th className="px-6 py-3 text-right text-xs font-semibold text-slate-500 uppercase tracking-wider">Действия</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-slate-100">
            {filteredStudents.map((student, idx) => (
              <tr key={student.id} className="table-row-hover">
                <td className="px-6 py-4 whitespace-nowrap">
                  <div className="flex items-center">
                    <div className={`w-10 h-10 rounded-full bg-gradient-to-br ${avatarColors[idx % avatarColors.length]} text-white flex items-center justify-center font-bold text-sm shadow-sm`}>
                      {getInitials(student.fullName)}
                    </div>
                    <div className="ml-3">
                      <div className="text-sm font-semibold text-slate-900">{student.fullName}</div>
                    </div>
                  </div>
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-slate-500">{student.email}</td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${groupColors[student.groupName] || 'bg-slate-100 text-slate-700'}`}>
                    {student.groupName}
                  </span>
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium space-x-3">
                  <Link to={`/students/${student.id}`} className="text-indigo-600 hover:text-indigo-800">Подробнее</Link>
                  {isAdminOrTeacher && (
                    <>
                      <button onClick={() => openEdit(student)} className="text-indigo-600 hover:text-indigo-900">Изменить</button>
                      <button onClick={() => handleDelete(student.id)} className="text-red-600 hover:text-red-900">Удалить</button>
                    </>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        {filteredStudents.length === 0 && (
          <div className="text-center py-12 text-slate-400">
            <div className="text-lg font-medium text-slate-400 mb-2">Нет данных</div>
            <p>Студенты не найдены</p>
          </div>
        )}
      </div>

      {modalOpen && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
          <div className="bg-white rounded-lg shadow-xl p-6 w-full max-w-md">
            <h2 className="text-xl font-bold mb-4">{editing ? 'Редактировать' : 'Добавить'} студента</h2>
            {error && <div className="bg-red-50 text-red-700 px-4 py-2 rounded mb-4 text-sm">{error}</div>}
            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">ФИО</label>
                <input required value={form.fullName} onChange={(e) => setForm({ ...form, fullName: e.target.value })}
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-blue-500 focus:border-blue-500" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Email</label>
                <input type="email" required value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })}
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-blue-500 focus:border-blue-500" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Группа</label>
                <select value={form.groupId} onChange={(e) => setForm({ ...form, groupId: Number(e.target.value) })}
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-blue-500 focus:border-blue-500">
                  {groups.map((g) => <option key={g.id} value={g.id}>{g.name}</option>)}
                </select>
              </div>
              <div className="flex justify-end gap-2">
                <button type="button" onClick={() => setModalOpen(false)} className="px-4 py-2 text-gray-700 bg-gray-100 rounded-lg hover:bg-gray-200">Отмена</button>
                <button type="submit" className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700">Сохранить</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default StudentsPage;
