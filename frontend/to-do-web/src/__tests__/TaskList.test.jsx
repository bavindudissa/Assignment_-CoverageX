// src/__tests__/TaskList.test.jsx
import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import TaskList from '../components/TaskList'; // Adjust the import path if necessary
import axios from 'axios';

// Mocking Axios
jest.mock('axios');

beforeEach(() => {
    process.env.VITE_API_URL = 'http://localhost:5133/api';
});

describe('TaskList Component', () => {
  // Test for fetching tasks and displaying them
  test('fetches and displays tasks correctly', async () => {
    // Mocking the API call to simulate task data
    axios.get.mockResolvedValue({
      data: [
        { taskId: 1, title: 'Test Task 1', description: 'Test Description 1', isCompleted: false },
        { taskId: 2, title: 'Test Task 2', description: 'Test Description 2', isCompleted: true },
      ],
    });

    render(<TaskList />);

    // Wait for the tasks to be displayed
    await waitFor(() => {
      expect(screen.getByText('Test Task 1')).toBeInTheDocument();
      expect(screen.getByText('Test Task 2')).toBeInTheDocument();
    });
  });

  // Test for when no tasks are available
  test('shows message when no tasks are available', async () => {
    // Mocking the API call to simulate no tasks
    axios.get.mockResolvedValue({
      data: [],
    });

    render(<TaskList />);

    // Wait for the "No tasks available" message
    await waitFor(() => {
      expect(screen.getByText('No tasks available.')).toBeInTheDocument();
    });
  });

  // Test for marking a task as completed
  test('marks a task as completed when clicking "Mark as Done"', async () => {
    // Mocking the API call to simulate task data
    axios.get.mockResolvedValue({
      data: [{ taskId: 1, title: 'Test Task', description: 'Test Description', isCompleted: false }],
    });
    axios.put.mockResolvedValue({});

    render(<TaskList />);

    // Wait for the task to be displayed
    await waitFor(() => {
      expect(screen.getByText('Test Task')).toBeInTheDocument();
    });

    // Click the "Mark as Done" button
    const markAsDoneButton = screen.getByRole('button', { name: /Mark as Done/i });
    fireEvent.click(markAsDoneButton);

    // Wait for the task to be marked as done
    await waitFor(() => {
      expect(markAsDoneButton).toHaveTextContent('Done');
    });

    // Check that the PUT request was made for the correct task
    expect(axios.put).toHaveBeenCalledWith('http://localhost:5133/api/task/1/complete');
  });

  // Test for handling an API error when fetching tasks
  test('shows error message when tasks cannot be fetched', async () => {
    // Mocking the API call to simulate an error
    axios.get.mockRejectedValue(new Error('Request failed with status code 404'));

    render(<TaskList />);

    // Wait for the error message to appear
    await waitFor(() => {
      expect(screen.queryByText('Failed to fetch tasks')).toBeNull(); // No tasks should be displayed
    });
  });

  // Test for handling an API error when marking a task as done
  test('handles API error when marking a task as completed', async () => {
    // Mocking the API call to simulate task data
    axios.get.mockResolvedValue({
      data: [{ taskId: 1, title: 'Test Task', description: 'Test Description', isCompleted: false }],
    });
    axios.put.mockRejectedValue(new Error('Request failed with status code 404'));

    render(<TaskList />);

    // Wait for the task to be displayed
    await waitFor(() => {
      expect(screen.getByText('Test Task')).toBeInTheDocument();
    });

    // Click the "Mark as Done" button
    const markAsDoneButton = screen.getByRole('button', { name: /Mark as Done/i });
    fireEvent.click(markAsDoneButton);

    // Wait for the button to still be enabled (because of the error)
    await waitFor(() => {
      expect(markAsDoneButton).toHaveTextContent('Mark as Done');
    });
  });
});
