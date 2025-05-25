import React, { useState } from 'react';
import axios from 'axios';
import { getErrorMessage } from '../utils/errorMessages';

type RegisterData = {
  userName: string;
  email: string;
  password: string;
};

export default function Register() {
  const [formData, setFormData] = useState<RegisterData>({
    userName: '',
    email: '',
    password: '',
  });

  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData(prev => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMessage(null);
    setSuccessMessage(null);

    try {
      await axios.post('/user/register', formData);
      setSuccessMessage('Регистрация прошла успешно!');
    } catch (error: unknown) {
      if (axios.isAxiosError(error)) {
        if (error.response && error.response.data) {
          const backendError = error.response.data;
          setErrorMessage(getErrorMessage(backendError));
        } else if (error.request) {
          setErrorMessage('Сервер не отвечает. Попробуйте позже.');
        } else {
          setErrorMessage('Ошибка при отправке запроса.');
        }
      } else {
        setErrorMessage('Неизвестная ошибка.');
      }
    }
  };

  return (
    <div className="max-w-md mx-auto mt-10 p-6 bg-white rounded shadow">
      <h1 className="text-2xl mb-4 font-bold">Регистрация</h1>
      {errorMessage && (
        <div className="mb-4 p-3 bg-red-100 text-red-700 rounded">
          {errorMessage}
        </div>
      )}
      {successMessage && (
        <div className="mb-4 p-3 bg-green-100 text-green-700 rounded">
          {successMessage}
        </div>
      )}
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label htmlFor="userName" className="block mb-1 font-semibold">Имя пользователя</label>
          <input
            type="text"
            name="userName"
            id="userName"
            value={formData.userName}
            onChange={handleChange}
            className="w-full border px-3 py-2 rounded"
            required
          />
        </div>
        <div>
          <label htmlFor="email" className="block mb-1 font-semibold">Email</label>
          <input
            type="email"
            name="email"
            id="email"
            value={formData.email}
            onChange={handleChange}
            className="w-full border px-3 py-2 rounded"
            required
          />
        </div>
        <div>
          <label htmlFor="password" className="block mb-1 font-semibold">Пароль</label>
          <input
            type="password"
            name="password"
            id="password"
            value={formData.password}
            onChange={handleChange}
            className="w-full border px-3 py-2 rounded"
            required
          />
        </div>
        <button
          type="submit"
          className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 transition"
        >
          Зарегистрироваться
        </button>
      </form>
    </div>
  );
}