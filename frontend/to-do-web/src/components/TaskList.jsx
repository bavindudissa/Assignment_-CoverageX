// src/components/TaskList.jsx
import React, { useEffect, useState } from "react";
import axios from "axios";

const TaskList = () => {
  const [tasks, setTasks] = useState([]);
  const [error, setError] = useState(null);

  const fetchTasks = async () => {
    try {
      const response = await axios.get(import.meta.env.VITE_API_URL + "/task");
      setTasks(response.data);
    } catch (error) {
      console.error("Error fetching tasks:", error);
      setError("Failed to fetch tasks");
    }
  };

  useEffect(() => {
    fetchTasks();
  }, []);

  const handleMarkAsCompleted = async (id) => {
    try {
      await axios.put(import.meta.env.VITE_API_URL + `/task/${id}/complete`);
      setTasks(tasks.map(task => 
        task.taskId === id ? { ...task, isCompleted: true } : task
      ));
      fetchTasks();
    } catch (error) {
      console.error("Error completing task:", error);
    }
  };

  return (
    <div>
      <h2>Task List</h2>
      {error ? (
        <p>{error}</p>
      ) :
      tasks.length === 0 ? (
        <p>No tasks available.</p>
      ) : (
        tasks.map((task) => (
          <div className="task-item" key={task.taskId}>
            <div className="task-content">
              <h3>{task.title}</h3>
              <p>{task.description}</p>
            </div>
            <button
              className="done-button"
              onClick={() => handleMarkAsCompleted(task.taskId)}
              disabled={task.isCompleted}
            >
              {task.isCompleted ? "Done" : "Mark as Done"}
            </button>
          </div>
        ))
      )}
    </div>
  );
};

export default TaskList;
