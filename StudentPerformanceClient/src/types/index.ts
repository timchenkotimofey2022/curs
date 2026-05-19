export interface Group {
  id: number;
  name: string;
  course: number;
  studentsCount: number;
}

export interface Student {
  id: number;
  fullName: string;
  email: string;
  groupId: number;
  groupName: string;
}

export interface Teacher {
  id: number;
  fullName: string;
  email: string;
}

export interface Subject {
  id: number;
  name: string;
  teacherId: number;
  teacherName: string;
}

export interface Grade {
  id: number;
  studentId: number;
  studentName: string;
  subjectId: number;
  subjectName: string;
  value: number;
  date: string;
  comment?: string;
}

export interface SubjectGrade {
  subjectId: number;
  subjectName: string;
  value: number;
  date: string;
  comment?: string;
}

export interface StudentPerformance {
  studentId: number;
  studentName: string;
  groupName: string;
  subjectGrades: SubjectGrade[];
  averageGrade: number;
}

export interface GroupAverage {
  groupId: number;
  groupName: string;
  course: number;
  averageGrade: number;
  totalGrades: number;
}

export interface AuthResponse {
  token: string;
  email: string;
  role: string;
  expiration: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}
