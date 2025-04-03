// src/components/TaskForm.jsx
import React, { useState } from "react";
import axios from "axios";

const TaskForm = ({ onTaskAdded }) => {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [isCompleted, setIsCompleted] = useState(false);
  const [message, setMessage] = useState("");
  const [messageType, setMessageType] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

    const newTask = { title, description, isCompleted };

    try {
      await axios.post(import.meta.env.VITE_API_URL + "/task", newTask);

      setMessage("Task added successfully!");
      setMessageType("success");

      setTitle("");
      setDescription("");
      setIsCompleted(false);

      onTaskAdded();
    } catch (error) {
      console.error("Error adding task:", error);

      setMessage("Error adding task, please try again.");
      setMessageType("error");
    }
  };

  return (
    <div>
      <h2>Add a Task</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="taskTitle">Title</label>
          <input
            type="text"
            id="taskTitle"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
        </div>
        <div>
          <label htmlFor="taskDescription">Description</label>
          <textarea
            id="taskDescription"
            required
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>
        <button type="submit">Add</button>
      </form>
      {message && (
        <p className={`message ${messageType}`}>{message}</p>
      )}
    </div>
  );
};

export default TaskForm;
