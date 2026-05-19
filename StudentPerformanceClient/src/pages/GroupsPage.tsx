import React, { useEffect, useState } from 'react';
import { groupsApi } from '../services/api';
import type { Group } from '../types';
import { useAuth } from '../context/AuthContext';

const courseColors: Record<number, string> = {
  1: 'bg-sky-100 text-sky-700 border-sky-200',
  2: 'bg-violet-100 text-violet-700 border-violet-200',
  3: 'bg-emerald-100 text-emerald-700 border-emerald-200',
  4: 'bg-amber-100 text-amber-700 border-amber-200',
  5: 'bg-rose-100 text-rose-700 border-rose-200',
  6: 'bg-slate-100 text-slate-700 border-slate-200',
};

const GroupsPage: React.FC = () => {
  const [groups, setGroups] = useState<Group[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<Group | null>(null);
  const [form, setForm] = useState({ name: '', course: 1 });
  const [error, setError] = useState('');
  const { user } = useAuth();
  const isAdminOrTeacher = user?.role === 'Admin' || user?.role === 'Teacher';

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await groupsApi.getAll();
      setGroups(res.data);
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
    setForm({ name: '', course: 1 });
    setError('');
    setModalOpen(true);
  };

  const openEdit = (group: Group) => {
    setEditing(group);
    setForm({ name: group.name, course: group.course });
    setError('');
    setModalOpen(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    try {
      if (editing) {
        await groupsApi.update(editing.id, form);
      } else {
        await groupsApi.create(form);
      }
      setModalOpen(false);
      fetchData();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Ошибка сохранения');
    }
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Удалить группу?')) return;
    try {
      await groupsApi.delete(id);
      fetchData();
    } catch (err: any) {
      alert(err.response?.data?.message || 'Ошибка удаления');
    }
  };

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
        <h1 className="text-3xl font-bold text-slate-800">Учебные группы</h1>
        <div className="flex items-center gap-3">
          <span className="text-sm text-slate-500 bg-white px-3 py-1 rounded-full border shadow-sm">
            Всего: {groups.length}
          </span>
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

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {groups.map((group, idx) => (
          <div
            key={group.id}
            className="stat-card relative overflow-hidden"
            style={{ animationDelay: `${idx * 0.05}s` }}
          >
            <div className="absolute top-0 right-0 w-24 h-24 bg-gradient-to-br from-indigo-50 to-transparent rounded-bl-full -mr-4 -mt-4 opacity-60"></div>
            <div className="relative">
              <div className="flex items-center justify-between mb-4">
                <div className="flex items-center gap-3">
                  <div className="w-10 h-10 rounded-xl bg-gradient-to-br from-indigo-500 to-blue-600 text-white flex items-center justify-center text-lg font-bold shadow-md">
                    {group.name.split('-')[1]?.[0] || group.name[0]}
                  </div>
                  <h2 className="text-xl font-bold text-slate-800">{group.name}</h2>
                </div>
                <span className={`px-3 py-1 rounded-full text-sm font-semibold border ${courseColors[group.course] || courseColors[1]}`}>
                  Курс {group.course}
                </span>
              </div>
              
              <div className="flex items-center gap-4 mt-4">
                <div className="flex-1 bg-slate-50 rounded-lg p-3 text-center">
                  <div className="text-2xl font-bold text-indigo-600">{group.studentsCount}</div>
                  <div className="text-xs text-slate-500 uppercase tracking-wide mt-1">студентов</div>
                </div>
                <div className="flex-1 bg-slate-50 rounded-lg p-3 text-center">
                  <div className="text-2xl font-bold text-emerald-600">
                    {Math.round((group.studentsCount || 0) * 2.5)}
                  </div>
                  <div className="text-xs text-slate-500 uppercase tracking-wide mt-1">оценок</div>
                </div>
              </div>

              {isAdminOrTeacher && (
                <div className="flex justify-end gap-2 mt-4">
                  <button onClick={() => openEdit(group)} className="text-sm text-indigo-600 hover:text-indigo-800 font-medium">Изменить</button>
                  <button onClick={() => handleDelete(group.id)} className="text-sm text-red-600 hover:text-red-800 font-medium">Удалить</button>
                </div>
              )}
            </div>
          </div>
        ))}
      </div>

      {modalOpen && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
          <div className="bg-white rounded-lg shadow-xl p-6 w-full max-w-md">
            <h2 className="text-xl font-bold mb-4">{editing ? 'Редактировать' : 'Добавить'} группу</h2>
            {error && <div className="bg-red-50 text-red-700 px-4 py-2 rounded mb-4 text-sm">{error}</div>}
            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Название</label>
                <input required value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })}
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-blue-500 focus:border-blue-500" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Курс</label>
                <input type="number" min={1} max={6} required value={form.course} onChange={(e) => setForm({ ...form, course: Number(e.target.value) })}
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-blue-500 focus:border-blue-500" />
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

export default GroupsPage;
