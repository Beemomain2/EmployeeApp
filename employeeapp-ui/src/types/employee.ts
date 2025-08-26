export enum EmployeeRole {
  Employee = 0,
  TeamLead = 1,
  Manager = 2,
  Director = 3
}

export interface Employee {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  role: EmployeeRole;
  hireDate: string;
  salary: number;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateEmployeeRequest {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  role: EmployeeRole;
  hireDate: string;
  salary: number;
}

export interface UpdateEmployeeRequest extends CreateEmployeeRequest {
  id: number;
  isActive: boolean;
}

export interface ApiError {
  message: string;
  statusCode: number;
}