import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import TaskForm from '../components/TaskForm';  // Adjust the import path if necessary
import axios from 'axios';

// Mocking Axios
jest.mock('axios');

beforeEach(() => {
  process.env.VITE_API_URL = 'http://localhost:5133/api';
});


// Test suite for TaskForm component
describe('TaskForm Component', () => {
  
  // Test for successfully adding a task
  test('submits the form and shows success message', async () => {
    // Mocking the API call to simulate a successful task addition
    axios.post.mockResolvedValue({
      data: { title: 'Test Task', description: 'Test Description', isCompleted: false },
    });

    // Render the TaskForm component
    render(<TaskForm onTaskAdded={jest.fn()} />);

    // Fill out the form
    fireEvent.change(screen.getByLabelText(/Title/i), { target: { value: 'Test Task' } });
    fireEvent.change(screen.getByLabelText(/Description/i), { target: { value: 'Test Description' } });

    // Submit the form
    fireEvent.click(screen.getByRole('button', { name: /Add/i }));

    // Wait for the success message to appear
    await waitFor(() => {
      expect(screen.getByText(/Task added successfully!/i)).toBeInTheDocument();
    });
  });

  // Test for handling API error
  test('handles API error and shows error message', async () => {
    // Mocking the API call to simulate a failed task addition (404)
    axios.post.mockRejectedValue(new Error('Request failed with status code 404'));

    // Render the TaskForm component
    render(<TaskForm onTaskAdded={jest.fn()} />);

    // Fill out the form
    fireEvent.change(screen.getByLabelText(/Title/i), { target: { value: 'Test Task' } });
    fireEvent.change(screen.getByLabelText(/Description/i), { target: { value: 'Test Description' } });

    // Submit the form
    fireEvent.click(screen.getByRole('button', { name: /Add/i }));

    // Wait for the error message to appear
    await waitFor(() => {
      expect(screen.getByText(/Error adding task, please try again./i)).toBeInTheDocument();
    });
  });

});
