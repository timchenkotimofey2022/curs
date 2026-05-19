import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { studentsApi, gradesApi, subjectsApi } from '../services/api';
import type { StudentPerformance, Subject } from '../types';
import { useAuth } from '../context/AuthContext';

const StudentDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [performance, setPerformance] = useState<StudentPerformance | null>(null);
  const [subjects, setSubjects] = useState<Subject[]>([]);
  const [loading, setLoading] = useState(true);
  const [editModal, setEditModal] = useState(false);
  const [editingGrade, setEditingGrade] = useState<{subjectId: number; value: number; date: string; comment?: string} | null>(null);
  const [editSubjectId, setEditSubjectId] = useState(0);
  const { user } = useAuth();
  const isAdminOrTeacher = user?.role === 'Admin' || user?.role === 'Teacher';

  const fetchData = async () => {
    setLoading(true);
    try {
      const [perfRes, subjRes] = await Promise.all([
        studentsApi.getPerformance(Number(id)),
        subjectsApi.getAll(),
      ]);
      setPerformance(perfRes.data);
      setSubjects(subjRes.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, [id]);

  const handleDeleteGrade = async (gradeId: number) => {
    if (!confirm('Удалить оценку?')) return;
    try {
      await gradesApi.delete(gradeId);
      fetchData();
    } catch (err: any) {
      alert(err.response?.data?.message || 'Ошибка удаления');
    }
  };

  const openEditGrade = (sg: {subjectId: number; value: number; date: string; comment?: string}) => {
    setEditingGrade(sg);
    setEditSubjectId(sg.subjectId);
    setEditModal(true);
  };

  const handleEditSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!editingGrade || !performance) return;
    // Find grade id by studentId and subjectId
    try {
      const gradesRes = await gradesApi.getAll(Number(id), editingGrade.subjectId);
      const grade = gradesRes.data[0];
      if (!grade) return;
      await gradesApi.update(grade.id, {
        studentId: Number(id),
        subjectId: editSubjectId,
        value: editingGrade.value,
        date: new Date(editingGrade.date).toISOString(),
        comment: editingGrade.comment,
      });
      setEditModal(false);
      fetchData();
    } catch (err: any) {
      alert(err.response?.data?.message || 'Ошибка обновления');
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  if (!performance) {
    return (
      <div className="text-center py-12">
        <p className="text-gray-500">Студент не найден</p>
        <Link to="/students" className="text-blue-600 hover:text-blue-800 mt-4 inline-block">
          ← Назад к списку
        </Link>
      </div>
    );
  }

  const chartData = performance.subjectGrades.map((sg) => ({
    subject: sg.subjectName,
    оценка: sg.value,
    date: new Date(sg.date).toLocaleDateString('ru-RU'),
  }));

  return (
    <div className="space-y-8">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">{performance.studentName}</h1>
          <p className="text-gray-500 mt-1">Группа: {performance.groupName}</p>
        </div>
        <div className="text-right">
          <p className="text-sm text-gray-500">Средний балл</p>
          <p className={`text-3xl font-bold ${
            performance.averageGrade >= 4 ? 'text-green-600' :
            performance.averageGrade >= 3 ? 'text-yellow-600' :
            'text-red-600'
          }`}>
            {performance.averageGrade.toFixed(2)}
          </p>
        </div>
      </div>

      <div className="bg-white rounded-lg shadow p-6">
        <h2 className="text-lg font-semibold text-gray-900 mb-4">График успеваемости</h2>
        <ResponsiveContainer width="100%" height={300}>
          <LineChart data={chartData}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="subject" />
            <YAxis domain={[1, 5]} ticks={[1, 2, 3, 4, 5]} />
            <Tooltip />
            <Legend />
            <Line type="monotone" dataKey="оценка" stroke="#2563eb" strokeWidth={2} dot={{ fill: '#2563eb', r: 5 }} />
          </LineChart>
        </ResponsiveContainer>
      </div>

      <div className="bg-white rounded-lg shadow">
        <div className="px-6 py-4 border-b flex items-center justify-between">
          <h2 className="text-lg font-semibold text-gray-900">Оценки по предметам</h2>
        </div>
        <div className="divide-y">
          {performance.subjectGrades.map((sg) => (
            <div key={sg.subjectId} className="px-6 py-4 flex items-center justify-between">
              <div>
                <p className="font-medium text-gray-900">{sg.subjectName}</p>
                <p className="text-sm text-gray-500">
                  {new Date(sg.date).toLocaleDateString('ru-RU')} · {sg.comment}
                </p>
              </div>
              <div className="flex items-center gap-3">
                <span className={`px-3 py-1 rounded-full text-sm font-bold ${
                  sg.value >= 4 ? 'bg-green-100 text-green-800' :
                  sg.value >= 3 ? 'bg-yellow-100 text-yellow-800' :
                  'bg-red-100 text-red-800'
                }`}>
                  {sg.value}
                </span>
                {isAdminOrTeacher && (
                  <div className="flex gap-2">
                    <button onClick={() => openEditGrade(sg)} className="text-xs text-indigo-600 hover:text-indigo-900">Изменить</button>
                    <button onClick={() => handleDeleteGrade(sg.subjectId)} className="text-xs text-red-600 hover:text-red-900">Удалить</button>
                  </div>
                )}
              </div>
            </div>
          ))}
        </div>
      </div>

      <Link to="/students" className="text-blue-600 hover:text-blue-800 inline-block">
        ← Назад к списку студентов
      </Link>

      {editModal && editingGrade && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
          <div className="bg-white rounded-lg shadow-xl p-6 w-full max-w-md">
            <h2 className="text-xl font-bold mb-4">Редактировать оценку</h2>
            <form onSubmit={handleEditSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Предмет</label>
                <select value={editSubjectId} onChange={(e) => setEditSubjectId(Number(e.target.value))}
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-blue-500 focus:border-blue-500">
                  {subjects.map((s) => <option key={s.id} value={s.id}>{s.name}</option>)}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Оценка</label>
                <select value={editingGrade.value} onChange={(e) => setEditingGrade({ ...editingGrade, value: Number(e.target.value) })}
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-blue-500 focus:border-blue-500">
                  {[5, 4, 3, 2, 1].map((v) => <option key={v} value={v}>{v}</option>)}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Дата</label>
                <input type="date" required value={editingGrade.date.split('T')[0]} onChange={(e) => setEditingGrade({ ...editingGrade, date: e.target.value })}
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-blue-500 focus:border-blue-500" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Комментарий</label>
                <textarea value={editingGrade.comment || ''} onChange={(e) => setEditingGrade({ ...editingGrade, comment: e.target.value })}
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-blue-500 focus:border-blue-500" rows={2} />
              </div>
              <div className="flex justify-end gap-2">
                <button type="button" onClick={() => setEditModal(false)} className="px-4 py-2 text-gray-700 bg-gray-100 rounded-lg hover:bg-gray-200">Отмена</button>
                <button type="submit" className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700">Сохранить</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default StudentDetailPage;
