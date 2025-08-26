import { Employee, CreateEmployeeRequest, UpdateEmployeeRequest } from '../types/employee';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'https://localhost:3000/api';

class ApiService {
  private async handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(errorText || `HTTP error! status: ${response.status}`);
    }
    
    if (response.status === 204) {
      return {} as T;
    }
    
    return response.json();
  }

  async getAllEmployees(): Promise<Employee[]> {
    const response = await fetch(`${API_BASE_URL}/employees`);
    return this.handleResponse<Employee[]>(response);
  }

  async getEmployeeById(id: number): Promise<Employee> {
    const response = await fetch(`${API_BASE_URL}/employees/${id}`);
    return this.handleResponse<Employee>(response);
  }

  async createEmployee(employee: CreateEmployeeRequest): Promise<number> {
    const response = await fetch(`${API_BASE_URL}/employees`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(employee),
    });
    return this.handleResponse<number>(response);
  }

  async updateEmployee(employee: UpdateEmployeeRequest): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/employees/${employee.id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(employee),
    });
    return this.handleResponse<void>(response);
  }

  async deleteEmployee(id: number): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/employees/${id}`, {
      method: 'DELETE',
    });
    return this.handleResponse<void>(response);
  }
}

export const apiService = new ApiService();
