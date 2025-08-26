import { useState, useEffect } from 'react';
import { Employee, CreateEmployeeRequest, UpdateEmployeeRequest } from '../types/employee';
import { apiService } from '../services/api';

export const useEmployees = () => {
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');

  const fetchEmployees = async () => {
    setLoading(true);
    setError('');
    try {
      const data = await apiService.getAllEmployees();
      setEmployees(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to fetch employees');
    } finally {
      setLoading(false);
    }
  };

  const createEmployee = async (employee: CreateEmployeeRequest) => {
    try {
      await apiService.createEmployee(employee);
      await fetchEmployees();
    } catch (err) {
      throw new Error(err instanceof Error ? err.message : 'Failed to create employee');
    }
  };

  const updateEmployee = async (employee: UpdateEmployeeRequest) => {
    try {
      await apiService.updateEmployee(employee);
      await fetchEmployees();
    } catch (err) {
      throw new Error(err instanceof Error ? err.message : 'Failed to update employee');
    }
  };

  const deleteEmployee = async (id: number) => {
    try {
      await apiService.deleteEmployee(id);
      await fetchEmployees();
    } catch (err) {
      throw new Error(err instanceof Error ? err.message : 'Failed to delete employee');
    }
  };

  useEffect(() => {
    fetchEmployees();
  }, []);

  return {
    employees,
    loading,
    error,
    setError,
    fetchEmployees,
    createEmployee,
    updateEmployee,
    deleteEmployee,
  };
};