import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import TaskForm from '../components/TaskForm';
import axios from 'axios';

jest.mock('axios');

beforeEach(() => {
  process.env.VITE_API_URL = 'http://localhost:5001/api';
});

describe('TaskForm Component', () => {
  
  // Test for successfully adding a task
  test('submits the form and shows success message', async () => {
    
    axios.post.mockResolvedValue({
      data: { title: 'Test Task', description: 'Test Description', isCompleted: false },
    });

    
    render(<TaskForm onTaskAdded={jest.fn()} />);
  
    fireEvent.change(screen.getByLabelText(/Title/i), { target: { value: 'Test Task' } });
    fireEvent.change(screen.getByLabelText(/Description/i), { target: { value: 'Test Description' } });
 
    fireEvent.click(screen.getByRole('button', { name: /Add/i }));

    await waitFor(() => {
      expect(screen.getByText(/Task added successfully!/i)).toBeInTheDocument();
    });
  });

  // Test for handling API error
  test('handles API error and shows error message', async () => {
    
    axios.post.mockRejectedValue(new Error('Request failed with status code 404'));
   
    render(<TaskForm onTaskAdded={jest.fn()} />);
    
    fireEvent.change(screen.getByLabelText(/Title/i), { target: { value: 'Test Task' } });
    fireEvent.change(screen.getByLabelText(/Description/i), { target: { value: 'Test Description' } });
    
    fireEvent.click(screen.getByRole('button', { name: /Add/i }));

    await waitFor(() => {
      expect(screen.getByText(/Error adding task, please try again./i)).toBeInTheDocument();
    });
  });

});
