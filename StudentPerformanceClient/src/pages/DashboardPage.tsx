import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { groupsApi, studentsApi, subjectsApi, gradesApi } from '../services/api';
import type { GroupAverage } from '../types';

const DashboardPage: React.FC = () => {
  const [stats, setStats] = useState({
    students: 0,
    groups: 0,
    subjects: 0,
    grades: 0,
  });
  const [groupAverages, setGroupAverages] = useState<GroupAverage[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [studentsRes, groupsRes, subjectsRes, gradesRes] = await Promise.all([
          studentsApi.getAll(),
          groupsApi.getAll(),
          subjectsApi.getAll(),
          gradesApi.getAll(),
        ]);

        setStats({
          students: studentsRes.data.length,
          groups: groupsRes.data.length,
          subjects: subjectsRes.data.length,
          grades: gradesRes.data.length,
        });

        const averages = await Promise.all(
          groupsRes.data.map((g) => gradesApi.getGroupAverage(g.id))
        );
        setGroupAverages(averages.map((r) => r.data));
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
      </div>
    );
  }

  const statCards = [
    { label: 'Студентов', value: stats.students, icon: "С", color: 'from-blue-500 to-indigo-600', bg: 'bg-blue-50', text: 'text-blue-700' },
    { label: 'Групп', value: stats.groups, icon: "Г", color: 'from-emerald-500 to-green-600', bg: 'bg-emerald-50', text: 'text-emerald-700' },
    { label: 'Предметов', value: stats.subjects, icon: "П", color: 'from-violet-500 to-purple-600', bg: 'bg-violet-50', text: 'text-violet-700' },
    { label: 'Оценок', value: stats.grades, icon: "О", color: 'from-amber-500 to-orange-600', bg: 'bg-amber-50', text: 'text-amber-700' },
  ];

  const getGradeColor = (avg: number) => {
    if (avg >= 4.5) return 'from-emerald-500 to-green-500';
    if (avg >= 4) return 'from-blue-500 to-indigo-500';
    if (avg >= 3.5) return 'from-violet-500 to-purple-500';
    if (avg >= 3) return 'from-amber-500 to-yellow-500';
    return 'from-red-500 to-rose-500';
  };

  const getGradeBg = (avg: number) => {
    if (avg >= 4.5) return 'bg-emerald-100 text-emerald-800';
    if (avg >= 4) return 'bg-blue-100 text-blue-800';
    if (avg >= 3.5) return 'bg-violet-100 text-violet-800';
    if (avg >= 3) return 'bg-amber-100 text-amber-800';
    return 'bg-rose-100 text-rose-800';
  };

  return (
    <div className="space-y-8 animate-fade-in">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-slate-800">Общая статистика</h1>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {statCards.map((card, idx) => (
          <div key={card.label} className="stat-card" style={{ animationDelay: `${idx * 0.05}s` }}>
            <div className="flex items-center">
              <div className={`w-12 h-12 rounded-xl bg-gradient-to-br ${card.color} text-white flex items-center justify-center text-2xl shadow-md`}>
                {card.icon}
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-slate-500">{card.label}</p>
                <p className="text-3xl font-bold text-slate-800">{card.value}</p>
              </div>
            </div>
          </div>
        ))}
      </div>

      <div className="bg-white rounded-xl shadow-sm border border-slate-100 overflow-hidden">
        <div className="px-6 py-4 border-b border-slate-100 bg-gradient-to-r from-slate-50 to-white">
          <h2 className="text-lg font-bold text-slate-800">Средний балл по группам</h2>
        </div>
        <div className="divide-y divide-slate-100">
          {groupAverages.map((ga) => (
            <div key={ga.groupId} className="px-6 py-4 flex items-center justify-between hover:bg-slate-50/50 transition-colors">
              <div className="flex items-center gap-4">
                <div className="w-10 h-10 rounded-lg bg-gradient-to-br from-indigo-500 to-blue-600 text-white flex items-center justify-center font-bold text-sm">
                  {ga.groupName.split('-')[1]}
                </div>
                <div>
                  <Link to={`/groups`} className="text-indigo-600 hover:text-indigo-800 font-semibold">
                    {ga.groupName}
                  </Link>
                  <p className="text-sm text-slate-500">Курс {ga.course} · {ga.totalGrades} оценок</p>
                </div>
              </div>
              <div className="flex items-center gap-4">
                <div className="w-32 hidden sm:block">
                  <div className="h-2 bg-slate-100 rounded-full overflow-hidden">
                    <div
                      className={`h-full rounded-full bg-gradient-to-r ${getGradeColor(ga.averageGrade)}`}
                      style={{ width: `${Math.min((ga.averageGrade / 5) * 100, 100)}%` }}
                    ></div>
                  </div>
                </div>
                <div className={`px-3 py-1 rounded-full text-sm font-bold ${getGradeBg(ga.averageGrade)}`}>
                  {ga.averageGrade.toFixed(2)}
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default DashboardPage;
