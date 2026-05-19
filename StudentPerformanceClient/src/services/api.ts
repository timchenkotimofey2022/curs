import axios from 'axios';
import type { Group, Student, Subject, Grade, StudentPerformance, GroupAverage, Teacher } from '../types';

const API_BASE_URL = 'http://localhost:5002/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;

export const authApi = {
  login: (email: string, password: string) =>
    api.post('/auth/login', { email, password }),
  register: (data: { fullName: string; email: string; password: string; role: string }) =>
    api.post('/auth/register', data),
};

export const groupsApi = {
  getAll: () => api.get<Group[]>('/groups'),
  getById: (id: number) => api.get<Group>(`/groups/${id}`),
  create: (data: { name: string; course: number }) => api.post('/groups', data),
  update: (id: number, data: { name: string; course: number }) => api.put(`/groups/${id}`, data),
  delete: (id: number) => api.delete(`/groups/${id}`),
};

export const studentsApi = {
  getAll: (groupId?: number) => api.get<Student[]>(`/students${groupId ? `?groupId=${groupId}` : ''}`),
  getById: (id: number) => api.get<Student>(`/students/${id}`),
  getPerformance: (id: number) => api.get<StudentPerformance>(`/students/${id}/performance`),
  create: (data: { fullName: string; email: string; groupId: number }) => api.post('/students', data),
  update: (id: number, data: { fullName: string; email: string; groupId: number }) => api.put(`/students/${id}`, data),
  delete: (id: number) => api.delete(`/students/${id}`),
};

export const teachersApi = {
  getAll: () => api.get<Teacher[]>('/teachers'),
  getById: (id: number) => api.get<Teacher>(`/teachers/${id}`),
  create: (data: { fullName: string; email: string }) => api.post('/teachers', data),
  update: (id: number, data: { fullName: string; email: string }) => api.put(`/teachers/${id}`, data),
  delete: (id: number) => api.delete(`/teachers/${id}`),
};

export const subjectsApi = {
  getAll: (teacherId?: number) => api.get<Subject[]>(`/subjects${teacherId ? `?teacherId=${teacherId}` : ''}`),
  getById: (id: number) => api.get<Subject>(`/subjects/${id}`),
  create: (data: { name: string; teacherId: number }) => api.post('/subjects', data),
  update: (id: number, data: { name: string; teacherId: number }) => api.put(`/subjects/${id}`, data),
  delete: (id: number) => api.delete(`/subjects/${id}`),
};

export const gradesApi = {
  getAll: (studentId?: number, subjectId?: number) =>
    api.get<Grade[]>(`/grades${studentId || subjectId ? `?${studentId ? `studentId=${studentId}` : ''}${studentId && subjectId ? '&' : ''}${subjectId ? `subjectId=${subjectId}` : ''}` : ''}`),
  getById: (id: number) => api.get<Grade>(`/grades/${id}`),
  getGroupAverage: (groupId: number) => api.get<GroupAverage>(`/grades/group/${groupId}/average`),
  create: (data: { studentId: number; subjectId: number; value: number; date: string; comment?: string }) =>
    api.post('/grades', data),
  update: (id: number, data: { studentId: number; subjectId: number; value: number; date: string; comment?: string }) =>
    api.put(`/grades/${id}`, data),
  delete: (id: number) => api.delete(`/grades/${id}`),
};
