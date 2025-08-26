import React, { useState } from 'react';
import { Trash2, Edit, Plus, Save, X } from 'lucide-react';
import { Employee, EmployeeRole, CreateEmployeeRequest, UpdateEmployeeRequest } from '../types/employee';
import { useEmployees } from '../hooks/useEmployees';

const EmployeeRoleLabels: Record<EmployeeRole, string> = {
  [EmployeeRole.Employee]: 'Employee',
  [EmployeeRole.TeamLead]: 'Team Lead',
  [EmployeeRole.Manager]: 'Manager',
  [EmployeeRole.Director]: 'Director',
};

interface EmployeeFormData {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  role: EmployeeRole;
  hireDate: string;
  salary: string;
  isActive: boolean;
}

const EmployeeCRUD: React.FC = () => {
  const {
    employees,
    loading,
    error,
    setError,
    createEmployee,
    updateEmployee,
    deleteEmployee,
  } = useEmployees();

  const [editingEmployee, setEditingEmployee] = useState<Employee | null>(null);
  const [showAddForm, setShowAddForm] = useState(false);
  const [formData, setFormData] = useState<EmployeeFormData>({
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    role: EmployeeRole.Employee,
    hireDate: '',
    salary: '',
    isActive: true,
  });

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value, type } = e.target;
    const checked = (e.target as HTMLInputElement).checked;
    
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value,
    }));
  };

  const resetForm = () => {
    setFormData({
      firstName: '',
      lastName: '',
      email: '',
      phoneNumber: '',
      role: EmployeeRole.Employee,
      hireDate: '',
      salary: '',
      isActive: true,
    });
    setEditingEmployee(null);
    setShowAddForm(false);
  };

  const handleCreate = async () => {
    try {
      const createRequest: CreateEmployeeRequest = {
        firstName: formData.firstName,
        lastName: formData.lastName,
        email: formData.email,
        phoneNumber: formData.phoneNumber,
        role: Number(formData.role) as EmployeeRole,
        hireDate: formData.hireDate,
        salary: parseFloat(formData.salary),
      };
      
      await createEmployee(createRequest);
      resetForm();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to create employee');
    }
  };

  const handleUpdate = async () => {
    if (!editingEmployee) return;

    try {
      const updateRequest: UpdateEmployeeRequest = {
        id: editingEmployee.id,
        firstName: formData.firstName,
        lastName: formData.lastName,
        email: formData.email,
        phoneNumber: formData.phoneNumber,
        role: Number(formData.role) as EmployeeRole,
        hireDate: formData.hireDate,
        salary: parseFloat(formData.salary),
        isActive: formData.isActive,
      };
      
      await updateEmployee(updateRequest);
      resetForm();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to update employee');
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Are you sure you want to delete this employee?')) return;

    try {
      await deleteEmployee(id);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to delete employee');
    }
  };

  const startEdit = (employee: Employee) => {
    setEditingEmployee(employee);
    setFormData({
      firstName: employee.firstName,
      lastName: employee.lastName,
      email: employee.email,
      phoneNumber: employee.phoneNumber,
      role: employee.role,
      hireDate: employee.hireDate.split('T')[0],
      salary: employee.salary.toString(),
      isActive: employee.isActive,
    });
    setShowAddForm(false);
  };

  const startAdd = () => {
    resetForm();
    setShowAddForm(true);
  };

  const formatCurrency = (amount: number): string => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
    }).format(amount);
  };

  const formatDate = (dateString: string): string => {
    return new Date(dateString).toLocaleDateString();
  };

  return (
    <div className="min-h-screen bg-gray-50 p-6">
      <div className="max-w-7xl mx-auto">
        <div className="bg-white rounded-lg shadow-lg">
          <div className="px-6 py-4 border-b border-gray-200">
            <div className="flex justify-between items-center">
              <h1 className="text-3xl font-bold text-gray-900">Employee Management</h1>
              <button
                onClick={startAdd}
                className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 flex items-center gap-2"
              >
                <Plus className="w-4 h-4" />
                Add Employee
              </button>
            </div>
          </div>

          {error && (
            <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded m-4">
              {error}
              <button 
                onClick={() => setError('')}
                className="float-right text-red-700 hover:text-red-900"
              >
                ×
              </button>
            </div>
          )}

          {(showAddForm || editingEmployee) && (
            <div className="bg-gray-50 p-6 border-b">
              <h2 className="text-xl font-semibold mb-4">
                {editingEmployee ? 'Edit Employee' : 'Add New Employee'}
              </h2>
              <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <input
                  type="text"
                  name="firstName"
                  placeholder="First Name"
                  value={formData.firstName}
                  onChange={handleInputChange}
                  className="p-2 border rounded-md"
                />
                <input
                  type="text"
                  name="lastName"
                  placeholder="Last Name"
                  value={formData.lastName}
                  onChange={handleInputChange}
                  className="p-2 border rounded-md"
                />
                <input
                  type="email"
                  name="email"
                  placeholder="Email"
                  value={formData.email}
                  onChange={handleInputChange}
                  className="p-2 border rounded-md"
                />
                <input
                  type="text"
                  name="phoneNumber"
                  placeholder="Phone Number"
                  value={formData.phoneNumber}
                  onChange={handleInputChange}
                  className="p-2 border rounded-md"
                />
                <select
                  name="role"
                  value={formData.role}
                  onChange={handleInputChange}
                  className="p-2 border rounded-md"
                >
                  {Object.entries(EmployeeRoleLabels).map(([value, label]) => (
                    <option key={value} value={value}>{label}</option>
                  ))}
                </select>
                <input
                  type="date"
                  name="hireDate"
                  value={formData.hireDate}
                  onChange={handleInputChange}
                  className="p-2 border rounded-md"
                />
                <input
                  type="number"
                  name="salary"
                  placeholder="Salary"
                  value={formData.salary}
                  onChange={handleInputChange}
                  className="p-2 border rounded-md"
                />
                {editingEmployee && (
                  <div className="flex items-center">
                    <label className="flex items-center">
                      <input
                        type="checkbox"
                        name="isActive"
                        checked={formData.isActive}
                        onChange={handleInputChange}
                        className="mr-2"
                      />
                      Active
                    </label>
                  </div>
                )}
              </div>
              <div className="flex gap-2 mt-4">
                <button
                  onClick={editingEmployee ? handleUpdate : handleCreate}
                  className="bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700 flex items-center gap-2"
                >
                  <Save className="w-4 h-4" />
                  {editingEmployee ? 'Update' : 'Create'}
                </button>
                <button
                  onClick={resetForm}
                  className="bg-gray-600 text-white px-4 py-2 rounded-md hover:bg-gray-700 flex items-center gap-2"
                >
                  <X className="w-4 h-4" />
                  Cancel
                </button>
              </div>
            </div>
          )}

          <div className="p-6">
            {loading ? (
              <div className="text-center py-8">
                <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
                <p className="mt-4 text-gray-600">Loading employees...</p>
              </div>
            ) : employees.length === 0 ? (
              <div className="text-center py-8">
                <p className="text-gray-500">No employees found</p>
              </div>
            ) : (
              <div className="overflow-x-auto">
                <table className="min-w-full table-auto">
                  <thead className="bg-gray-100">
                    <tr>
                      <th className="px-4 py-2 text-left font-semibold">Name</th>
                      <th className="px-4 py-2 text-left font-semibold">Email</th>
                      <th className="px-4 py-2 text-left font-semibold">Phone</th>
                      <th className="px-4 py-2 text-left font-semibold">Role</th>
                      <th className="px-4 py-2 text-left font-semibold">Hire Date</th>
                      <th className="px-4 py-2 text-left font-semibold">Salary</th>
                      <th className="px-4 py-2 text-left font-semibold">Status</th>
                      <th className="px-4 py-2 text-left font-semibold">Actions</th>
                    </tr>
                  </thead>
                  <tbody className="bg-white">
                    {employees.map((employee) => (
                      <tr key={employee.id} className="border-b border-gray-200 hover:bg-gray-50">
                        <td className="px-4 py-3">
                          {employee.firstName} {employee.lastName}
                        </td>
                        <td className="px-4 py-3">{employee.email}</td>
                        <td className="px-4 py-3">{employee.phoneNumber}</td>
                        <td className="px-4 py-3">
                          <span className="px-2 py-1 bg-blue-100 text-blue-800 rounded-full text-sm">
                            {EmployeeRoleLabels[employee.role]}
                          </span>
                        </td>
                        <td className="px-4 py-3">{formatDate(employee.hireDate)}</td>
                        <td className="px-4 py-3">{formatCurrency(employee.salary)}</td>
                        <td className="px-4 py-3">
                          <span className={`px-2 py-1 rounded-full text-sm ${
                            employee.isActive 
                              ? 'bg-green-100 text-green-800' 
                              : 'bg-red-100 text-red-800'
                          }`}>
                            {employee.isActive ? 'Active' : 'Inactive'}
                          </span>
                        </td>
                        <td className="px-4 py-3">
                          <div className="flex gap-2">
                            <button
                              onClick={() => startEdit(employee)}
                              className="text-blue-600 hover:text-blue-800 p-1"
                              title="Edit"
                            >
                              <Edit className="w-4 h-4" />
                            </button>
                            <button
                              onClick={() => handleDelete(employee.id)}
                              className="text-red-600 hover:text-red-800 p-1"
                              title="Delete"
                            >
                              <Trash2 className="w-4 h-4" />
                            </button>
                          </div>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default EmployeeCRUD;