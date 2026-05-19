import React, { useEffect, useState } from 'react';
import { gradesApi, studentsApi, subjectsApi } from '../services/api';
import type { Grade, Student, Subject } from '../types';
import { useAuth } from '../context/AuthContext';

const GradesPage: React.FC = () => {
  const [grades, setGrades] = useState<Grade[]>([]);
  const [students, setStudents] = useState<Student[]>([]);
  const [subjects, setSubjects] = useState<Subject[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<Grade | null>(null);
  const [form, setForm] = useState({ studentId: 1, subjectId: 1, value: 5, date: new Date().toISOString().split('T')[0], comment: '' });
  const [error, setError] = useState('');
  const { user } = useAuth();
  const isAdminOrTeacher = user?.role === 'Admin' || user?.role === 'Teacher';

  const fetchData = async () => {
    setLoading(true);
    try {
      const [gradesRes, studentsRes, subjectsRes] = await Promise.all([
        gradesApi.getAll(),
        studentsApi.getAll(),
        subjectsApi.getAll(),
      ]);
      setGrades(gradesRes.data);
      setStudents(studentsRes.data);
      setSubjects(subjectsRes.data);
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
    setForm({
      studentId: students[0]?.id || 1,
      subjectId: subjects[0]?.id || 1,
      value: 5,
      date: new Date().toISOString().split('T')[0],
      comment: '',
    });
    setError('');
    setModalOpen(true);
  };

  const openEdit = (grade: Grade) => {
    setEditing(grade);
    setForm({
      studentId: grade.studentId,
      subjectId: grade.subjectId,
      value: grade.value,
      date: grade.date.split('T')[0],
      comment: grade.comment || '',
    });
    setError('');
    setModalOpen(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    try {
      const data = { ...form, date: new Date(form.date).toISOString() };
      if (editing) {
        await gradesApi.update(editing.id, data);
      } else {
        await gradesApi.create(data);
      }
      setModalOpen(false);
      fetchData();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Ошибка сохранения');
    }
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Удалить оценку?')) return;
    try {
      await gradesApi.delete(id);
      fetchData();
    } catch (err: any) {
      alert(err.response?.data?.message || 'Ошибка удаления');
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-gray-900">Оценки</h1>
        {isAdminOrTeacher && (
          <button
            onClick={openCreate}
            className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors shadow-md"
          >
            + Добавить
          </button>
        )}
      </div>

      <div className="bg-white rounded-lg shadow overflow-hidden">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Студент</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Предмет</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Оценка</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Дата</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Комментарий</th>
              {isAdminOrTeacher && <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Действия</th>}
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {grades.map((grade) => (
              <tr key={grade.id} className="hover:bg-gray-50">
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{grade.studentName}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{grade.subjectName}</td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <span className={`px-3 py-1 rounded-full text-sm font-bold ${
                    grade.value >= 4 ? 'bg-green-100 text-green-800' :
                    grade.value >= 3 ? 'bg-yellow-100 text-yellow-800' :
                    'bg-red-100 text-red-800'
                  }`}>
                    {grade.value}
                  </span>
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                  {new Date(grade.date).toLocaleDateString('ru-RU')}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{grade.comment || '-'}</td>
                {isAdminOrTeacher && (
                  <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium space-x-2">
                    <button onClick={() => openEdit(grade)} className="text-indigo-600 hover:text-indigo-900">Изменить</button>
                    <button onClick={() => handleDelete(grade.id)} className="text-red-600 hover:text-red-900">Удалить</button>
                  </td>
                )}
              </tr>
            ))}
          </tbody>
        </table>
        {grades.length === 0 && (
          <div className="text-center py-12 text-gray-400">Нет данных</div>
        )}
      </div>

      {modalOpen && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
          <div className="bg-white rounded-lg shadow-xl p-6 w-full max-w-md">
            <h2 className="text-xl font-bold mb-4">{editing ? 'Редактировать' : 'Добавить'} оценку</h2>
            {error && <div className="bg-red-50 text-red-700 px-4 py-2 rounded mb-4 text-sm">{error}</div>}
            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Студент</label>
                <select value={form.studentId} onChange={(e) => setForm({ ...form, studentId: Number(e.target.value) })}
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-blue-500 focus:border-blue-500">
                  {students.map((s) => <option key={s.id} value={s.id}>{s.fullName}</option>)}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Предмет</label>
                <select value={form.subjectId} onChange={(e) => setForm({ ...form, subjectId: Number(e.target.value) })}
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-blue-500 focus:border-blue-500">
                  {subjects.map((s) => <option key={s.id} value={s.id}>{s.name}</option>)}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Оценка</label>
                <select value={form.value} onChange={(e) => setForm({ ...form, value: Number(e.target.value) })}
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-blue-500 focus:border-blue-500">
                  {[5, 4, 3, 2, 1].map((v) => <option key={v} value={v}>{v}</option>)}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Дата</label>
                <input type="date" required value={form.date} onChange={(e) => setForm({ ...form, date: e.target.value })}
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-blue-500 focus:border-blue-500" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Комментарий</label>
                <textarea value={form.comment} onChange={(e) => setForm({ ...form, comment: e.target.value })}
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-blue-500 focus:border-blue-500" rows={2} />
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

export default GradesPage;
