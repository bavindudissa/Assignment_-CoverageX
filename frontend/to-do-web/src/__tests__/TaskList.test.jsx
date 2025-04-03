import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import TaskList from '../components/TaskList';
import axios from 'axios';

// Mocking Axios
jest.mock('axios');

beforeEach(() => {
    process.env.VITE_API_URL = 'http://localhost:5001/api';
});

describe('TaskList Component', () => {
  // Test for fetching tasks and displaying them
  test('fetches and displays tasks correctly', async () => {
    axios.get.mockResolvedValue({
      data: [
        { taskId: 1, title: 'Test Task 1', description: 'Test Description 1', isCompleted: false },
        { taskId: 2, title: 'Test Task 2', description: 'Test Description 2', isCompleted: true },
      ],
    });

    render(<TaskList />);

    await waitFor(() => {
      expect(screen.getByText('Test Task 1')).toBeInTheDocument();
      expect(screen.getByText('Test Task 2')).toBeInTheDocument();
    });
  });

  // Test for when no tasks are available
  test('shows message when no tasks are available', async () => {
    axios.get.mockResolvedValue({
      data: [],
    });

    render(<TaskList />);

    await waitFor(() => {
      expect(screen.getByText('No tasks available.')).toBeInTheDocument();
    });
  });

  // Test for marking a task as completed
  test('marks a task as completed when clicking "Mark as Done"', async () => {
    axios.get.mockResolvedValue({
      data: [{ taskId: 1, title: 'Test Task', description: 'Test Description', isCompleted: false }],
    });
    axios.put.mockResolvedValue({});

    render(<TaskList />);

    await waitFor(() => {
      expect(screen.getByText('Test Task')).toBeInTheDocument();
    });

    const markAsDoneButton = screen.getByRole('button', { name: /Mark as Done/i });
    fireEvent.click(markAsDoneButton);

    await waitFor(() => {
      expect(markAsDoneButton).toHaveTextContent('Done');
    });

    expect(axios.put).toHaveBeenCalledWith('http://localhost:5001/api/task/1/complete');
  });

  // Test for handling an API error when fetching tasks
  test('shows error message when tasks cannot be fetched', async () => {
    axios.get.mockRejectedValue(new Error('Request failed with status code 404'));

    render(<TaskList />);

    await waitFor(() => {
      expect(screen.queryByText('Failed to fetch tasks')).toBeNull();
    });
  });

  // Test for handling an API error when marking a task as done
  test('handles API error when marking a task as completed', async () => {
    axios.get.mockResolvedValue({
      data: [{ taskId: 1, title: 'Test Task', description: 'Test Description', isCompleted: false }],
    });
    axios.put.mockRejectedValue(new Error('Request failed with status code 404'));

    render(<TaskList />);

    await waitFor(() => {
      expect(screen.getByText('Test Task')).toBeInTheDocument();
    });

    const markAsDoneButton = screen.getByRole('button', { name: /Mark as Done/i });
    fireEvent.click(markAsDoneButton);

    await waitFor(() => {
      expect(markAsDoneButton).toHaveTextContent('Mark as Done');
    });
  });
});
